using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using PennyPal.Data;
using PennyPal.Dtos;
using PennyPal.Models;
using PennyPal.Services;

namespace PennyPal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IUserService _userService;

        public UserController(IConfiguration config, IUserService userService)
        {
            _dapper = new DataContextDapper(config);
            _userService = userService;
        }


        [HttpGet("TestConnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("Users")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userService.GetUsers();
        }

        [HttpGet("{id}")]
        public async Task<User?> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }

        [HttpPost()]
        public async Task AddUser(UserDto userDto)
        {
            await _userService.AddUser(userDto);
        }
        [HttpPut()]
        public async Task UpdateUser(UserUpdateDto user)
        {
            await _userService.UpdateUser(user);
        }

        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
        }
    }

}