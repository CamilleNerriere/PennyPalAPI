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

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _authHelper = new AuthHelper(config);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserLoginDto, UserDto>();
            }));
        }

        public async Task Register(UserForRegistrationDto user)
        {
            if (user.Password != user.ConfirmPassword)
            {
                throw new CustomValidationException("Password and Confirm Password must be the same");
            }

            if (_authRepository.GetAuthByEmail(user.Email) != null)
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
                Role = user.Role
            };

            User userToRegister = new()
            {
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                Email = user.Email,
            };

            using IDbContextTransaction transaction = await _authRepository.BeginTransactionAsync();
            try
            {
                await _authRepository.AddAuth(authToRegister);
                await _userRepository.AddUser(userToRegister);
                await _authRepository.SaveChangesAsync();
                await _userRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception("Unable to register user");
            }
        }

        public async Task<Dictionary<string, string>> Login(UserLoginDto user)
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

            return new Dictionary<string, string>{
                {
                    "token", _authHelper.CreateToken(userComplete.Id)
                }
            };

        }

        public async Task UpdatePassword(UserLoginDto userToUpdate)
        {
            if(userToUpdate == null)
            {
                throw new CustomValidationException("Missing credentials");
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

        public async Task DeleteAccount(string userId)
        {
            int userIdInt = Int32.Parse(userId);
            User? userToDelete = await _userRepository.GetUserById(userIdInt) ?? throw new NotFoundException("User not found");

            Auth? authToDelete = await _authRepository.GetAuthByEmail(userToDelete.Email) ?? throw new NotFoundException("User not found");

            using IDbContextTransaction transaction = await _authRepository.BeginTransactionAsync();
            try
            {
                await _authRepository.DeleteAuth(authToDelete);
                await _userRepository.DeleteUser(userToDelete.Id);
                await _authRepository.SaveChangesAsync();
                await _userRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (System.Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception("Unable to delete user");
            }
        }
    }
}