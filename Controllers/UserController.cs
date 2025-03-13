using Microsoft.AspNetCore.Mvc;
using PennyPal.Data;
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

        [HttpGet("Getusers")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userService.GetUsers();
        }
    }
    
}