using System.Security.Cryptography;
using System.Text;
using ecomC.DTOs;
using ecomC.Models;
using MongoDB.Driver;

namespace ecomC.Controllers;

// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMongoCollection<User> _users;

    public AuthController(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var userExists = await _users.Find(u => u.Username == registerDto.Username).FirstOrDefaultAsync();
        if (userExists != null)
        {
            return BadRequest("User already exists.");
        }

        var hashedPassword = BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = hashedPassword,
            Role = "User" // Default role for regular users
        };

        await _users.InsertOneAsync(user);

        return Ok("User registered successfully.");
    }
}

