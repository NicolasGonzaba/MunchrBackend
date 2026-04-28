using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MunchrBackend.Models;
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

    [HttpPost("UploadImage")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName)
    {
        if (file == null || file.Length == 0) return BadRequest("Invalid file.");

        using var stream = file.OpenReadStream();
        var fileUrl = await _userServices.UploadFileAsync(stream, fileName);

        Console.WriteLine($"File: {file?.FileName}");
        Console.WriteLine($"FileName param: {fileName}");

        return Ok(new { FileUrl = fileUrl });
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
    {
        bool success = await _userServices.CreateAccount(user);

        if (success) return Ok(new { Success = true, Message = "User Created." });

        return BadRequest(new { Success = false, Message = "User Creation failed Email is already in use." });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LogInDTO user)
    {
        var success = await _userServices.Login(user);

        if (success != null) return Ok(new { Token = success });

        return Unauthorized(new { Message = "Login was unsuccesful" });
    }

    [HttpGet("GetUserByUsername/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userServices.GetUserInfoByUsernameAsync(username);

        if (user != null) return Ok(user);

        return BadRequest(new { Message = "No user Found" });
    }
    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteContact(UserModel user)
    {
        var success = await _userServices.DeleteAccount(user);

        if (success) return Ok(new { success });

        return BadRequest(new { success });
    }

    [HttpPost("CreateUserWithImage")]
    public async Task<IActionResult> CreateUserWithImage([FromForm] IFormFile file, [FromForm] string username, [FromForm] string email,[FromForm] string password, [FromForm] string buissness)
    {
        string imageUrl = "";

        if (file != null)
        {
            using var stream = file.OpenReadStream();
            imageUrl = await _userServices.UploadFileAsync(stream, file.FileName);
        }

        UserDTO user = new()
        {
            Username = username,
            Email = email,
            Password = password,
            Buissness = buissness,
            ProfilePic = imageUrl
        };

        bool success = await _userServices.CreateAccount(user);

        if (success) return Ok(new { Success = true });

        return BadRequest(new { Success = false });
    }


}
