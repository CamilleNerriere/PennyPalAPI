using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using PennyPal.Data.Repositories;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Helpers;
using PennyPal.Models;

namespace PennyPal.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly AuthHelper _authHelper;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger, IAuthRepository authRepository, IUserRepository userRepository, IConfiguration config, IHttpContextAccessor httpContextAccessor, IRefreshTokenRepository refreshTokenRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserLoginDto, UserDto>();
            }));
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepository = refreshTokenRepository;
            _authHelper = new AuthHelper(config, _httpContextAccessor);
            _logger = logger;
        }

        public async Task Register(UserForRegistrationDto user)
        {
            if (user.Password != user.ConfirmPassword)
            {
                throw new CustomValidationException("Password and Confirm Password must be the same");
            }

            var auth = await _authRepository.GetAuthByEmail(user.Email);

            if (auth != null)
            {
                throw new CustomValidationException("user already exists");
            }

            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, passwordSalt);

            Auth authToRegister = new()
            {
                Email = user.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            User userToRegister = new()
            {
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                Email = user.Email,
            };

            IExecutionStrategy strategy = _authRepository.GetExecutionStrategy();



            _ = await strategy.ExecuteAsync(
                state: (authToRegister, userToRegister),
                operation: async (dbContext, state, cancellationToken) =>
                {
                    using var transaction = await _authRepository.BeginTransactionAsync(cancellationToken);
                    try
                    {

                        await _userRepository.AddUser(state.userToRegister, cancellationToken);
                        await _userRepository.SaveChangesAsync(cancellationToken);
                        await _authRepository.AddAuth(state.authToRegister, cancellationToken);
                        await _authRepository.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);

                        _logger.LogInformation("New User successfully added, with {Email}", state.userToRegister.Email);
                        return true;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        _logger.LogError("Error during registration with email {Email}", state.userToRegister.Email);
                        throw new Exception("Unable to register user");
                    }
                },
                    verifySucceeded: null
            );

        }

        public async Task<(string accessToken, string refreshToken, DateTime refreshExpiry)> Login(UserLoginDto user)
        {
            if (user == null)
            {
                throw new CustomValidationException("No user informations given");
            }

            var userToLog = await _authRepository.GetAuthByEmail(user.Email) ?? throw new NotFoundException("Invalid credentials");

            byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, userToLog.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userToLog.PasswordHash[index])
                {
                    throw new CustomValidationException("Invalid Credentials");
                }
            }

            UserDto UserMapped = _mapper.Map<UserDto>(user);

            var userComplete = await _userRepository.GetUserByEmail(UserMapped) ?? throw new NotFoundException("User not found");

            var accessToken = _authHelper.CreateToken(userComplete.Id);
            var refreshToken = _authHelper.GenerateRefreshToken();
            var refreshExpiry = DateTime.UtcNow.AddMinutes(15);
            var sessionDuration = TimeSpan.FromHours(12);

            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expires = refreshExpiry,
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = _authHelper.GetClientIp(),
                Revoked = false,
                SessionExpiresAt = DateTime.UtcNow.Add(sessionDuration),
                UserId = userComplete.Id
            };

            await _refreshTokenRepository.AddToken(tokenEntity);

            return (accessToken, refreshToken, refreshExpiry);

        }

        public async Task<(string accessToken, string refreshToken, DateTime refreshExpiry)> RefreshToken(string OldRefreshToken)
        {
            var token = await _refreshTokenRepository.GetByToken(OldRefreshToken);

            if (token == null || token.Expires < DateTime.UtcNow || token.Revoked || token.SessionExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid refresh token attempt (partial): {TokenPrefix}...", OldRefreshToken[..10]);
                throw new Unauthorized(401, "Invalid Token");
            }

            await _refreshTokenRepository.InvalidateToken(token);

            var newRefreshToken = _authHelper.GenerateRefreshToken();

            var now = DateTime.UtcNow;
            var sessionLimit = token.SessionExpiresAt;
            var refreshExpiry = now.AddMinutes(15) > sessionLimit ? sessionLimit : now.AddMinutes(15);

            var newToken = new RefreshToken
            {
                Token = newRefreshToken,
                Expires = refreshExpiry,
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = _authHelper.GetClientIp(),
                UserId = token.UserId,
                ReplacedByToken = token.Token,
                SessionExpiresAt = token.SessionExpiresAt
            };

            await _refreshTokenRepository.AddToken(newToken);

            var accessToken = _authHelper.CreateToken(token.UserId);

            return (accessToken, newRefreshToken, refreshExpiry);
        }


        public async Task UpdatePassword(UserUpdatePasswordDto userToUpdate, int userId)
        {
            if (userToUpdate == null)
            {
                throw new CustomValidationException("Missing credentials");
            }

            if (userToUpdate.Password != userToUpdate.ConfirmPassword)
            {
                throw new Exception("Password and Confirm Password must match");
            }

            var userConnected = await _userRepository.GetUserById(userId) ?? throw new NotFoundException("User not found");

            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = _authHelper.GetPasswordHash(userToUpdate.Password, passwordSalt);

            Auth updatedAuth = new()
            {
                Email = userConnected.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            await _authRepository.UpdateAuth(updatedAuth);

        }

        public async Task LogOut(string refreshToken)
        {

            var token = await _refreshTokenRepository.GetByToken(refreshToken);
            if (token != null)
            {
                await _refreshTokenRepository.InvalidateToken(token);
            }

        }

        public async Task DeleteAccount(int userId)
        {
            await _userRepository.DeleteUser(userId);
            await _userRepository.SaveChangesAsync();

        }
    }
}