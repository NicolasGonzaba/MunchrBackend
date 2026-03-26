using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MunchrBackend.Models.DTOS;
using MunchrBackend.Services;

namespace MunchrBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserServices _userServices;
    public UserController(UserServices userServices)
    {
        _userServices = userServices;
    }
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
    {
        bool success = await _userServices.CreateAccount(user);

        if (success) return Ok(new { Success = true, Message = "User Created." });

        return BadRequest(new { Success = false, Message = "User Creation failed Email is already in use." });
    }

    [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            var success = await _userServices.Login(user);

            if(success != null) return Ok(new {Token = success});

            return Unauthorized(new {Message = "Login was unsuccesful"});
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userServices.GetUserInfoByUsernameAsync(username);

            if(user != null) return Ok(user);

            return BadRequest(new {Message = "No user Found"});
        }


}
