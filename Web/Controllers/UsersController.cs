﻿using Application.Users.Commands.CreateUser;
using Application.Users.Queries.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[Route("users")]
public class UsersController : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var command = new CreateUserCommand
        {
            Email = registerDto.Email,
            Password = registerDto.Password,
            Role = registerDto.Role
        };
        var accessToken = await Mediator.Send(command);

        HttpContext.Response.Cookies.Append("token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            MaxAge = TimeSpan.FromHours(12)
        });

        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var query = new LoginQuery
        {
            Email = loginDto.Email,
            Password = loginDto.Password
        };
        var accessToken = await Mediator.Send(query);   
        
        HttpContext.Response.Cookies.Append("token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            MaxAge = TimeSpan.FromHours(12)
        });

        return Ok();
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await Task.Run(() => Parallel.ForEach(Request.Cookies.Keys, Response.Cookies.Delete));
        return StatusCode(205);
    }
    
    [HttpGet, Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetAllUserData()
    {
        return Ok();
    }
}