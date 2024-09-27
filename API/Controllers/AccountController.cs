using System;
//using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseAPIController
{

    // Register(string username, string password)
    [HttpPost("register")]  // POST /api/account/register
    public async Task<ActionResult<UserDTO>>Register(RegisterDTO registerDTO)
    {
        if (await UserExists(registerDTO.Username))
        {
            return BadRequest("Username is taken");
        }

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDTO.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok(new UserDTO
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        });
    }

    // Login(string username, string password)
    [HttpPost("login")]  // POST /api/account/login
    public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.username.ToLower());
        if (user == null) return Unauthorized("Invalid username");  // Change message eventually

        // Validate credentials
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
        for (int i=0; i<computedHash.Length; i++) {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }

        // Return the UserDTO with the token
        return Ok(new UserDTO
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        });
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}
