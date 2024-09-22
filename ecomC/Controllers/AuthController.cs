using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ecomC.DTOs;
using ecomC.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;


namespace BackendServices.Controllers;

// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _configuration;

    public AuthController(IMongoDatabase database, IConfiguration configuration)
    {
        _users = database.GetCollection<User>("Users");
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        var userExists = await _users.Find(u => u.Username == registerDto.Username).FirstOrDefaultAsync();
        if (userExists != null)
        {
            return BadRequest("User already exists.");
        }

        /*var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password); */
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
    
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var user = await _users.Find(u => u.Username == loginDto.Username).FirstOrDefaultAsync();
        if (user == null || !BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid credentials.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Use the key from appsettings.json
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
    
}