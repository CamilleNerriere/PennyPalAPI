using AutoMapper;
using PennyPal.Data.Repositories;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IAuthRepository authRepository)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, User>();
            }));
        }

        public async Task<IEnumerable<User>> GetUsers(int userId)
        {
            User user = await _userRepository.GetUserById(userId) ?? throw new NotFoundException("User Not Found");

            Auth auth = await _authRepository.GetAuthByEmail(user.Email) ?? throw new NotFoundException("Auth not found");

            if (auth.Role != "admin")
            {
                throw new Unauthorized(401, "Unauthorized Operation");
            }
            return await _userRepository.GetUsers();

        }

        public async Task<User?> GetUserById(int userId, int userConnectedId)
        {
            User user = await _userRepository.GetUserById(userId) ?? throw new NotFoundException("User not found");

            if (user.Id != userConnectedId)
            {
                throw new Unauthorized(401, "Unauthorized Operation");
            }

            return user;
        }

        public async Task UpdateUser(UserUpdateDto user, int userConnectedId)
        {
            if(user.Id != userConnectedId)
            {
                throw new Unauthorized(401, "Unauthorized Operation");
            }
            await _userRepository.UpdateUser(user);
        }

    }
}