using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using PennyPal.Data;
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

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, IConfiguration config, IHttpContextAccessor httpContextAccessor, IRefreshTokenRepository refreshTokenRepository)
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
        }

        public async Task Register(UserForRegistrationDto user)
        {
            if (user.Password != user.ConfirmPassword)
            {
                throw new CustomValidationException("Password and Confirm Password must be the same");
            }

            Auth? auth = await _authRepository.GetAuthByEmail(user.Email);

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
                Role = "user"
            };

            User userToRegister = new()
            {
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                Email = user.Email,
            };

            IExecutionStrategy strategy = _authRepository.GetExecutionStrategy();

            await strategy.ExecuteAsync(
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

                        return true;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync(cancellationToken);
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

            Auth? userToLog = await _authRepository.GetAuthByEmail(user.Email) ?? throw new NotFoundException("Invalid credentials");

            byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, userToLog.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userToLog.PasswordHash[index])
                {
                    throw new CustomValidationException("Invalid Credentials");
                }
            }

            UserDto UserMapped = _mapper.Map<UserDto>(user);

            User? userComplete = await _userRepository.GetUserByEmail(UserMapped) ?? throw new NotFoundException("User not found");

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


        public async Task UpdatePassword(UserLoginDto userToUpdate, int userId)
        {
            if (userToUpdate == null)
            {
                throw new CustomValidationException("Missing credentials");
            }

            User userConnected = await _userRepository.GetUserById(userId) ?? throw new NotFoundException("User not found");

            if (userToUpdate.Email != userConnected.Email)
            {
                throw new Unauthorized(401, "Unauthorized Operation");
            }

            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = _authHelper.GetPasswordHash(userToUpdate.Password, passwordSalt);

            Auth updatedAuth = new()
            {
                Email = userToUpdate.Email,
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