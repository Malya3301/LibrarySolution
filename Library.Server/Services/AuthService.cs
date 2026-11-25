using System.Security.Claims;
using Library.Data.DTOs;
using Library.Data.Models;
using Library.Data.Data;
using Library.Data.Responses;
using Library.Server.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Server.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordHasher _passwordHasher;
    private readonly DataBase _db;

    public AuthService(
        IHttpContextAccessor httpContextAccessor,
        IPasswordHasher passwordHasher,
        DataBase db)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordHasher = passwordHasher;
        _db = db;
    }

    public async Task<IActionResult> Register(RegisterDto model)
    {
        // 1. Используем AnyAsync вместо FirstAsync, чтобы не падать с исключением
        if (await _db.Users.AnyAsync(u => u.Email == model.Email || u.Username == model.Login))
        {
            return new BadRequestObjectResult(new ApiResponse<string> 
            { 
                Status = false, 
                Data = "Пользователь с таким логином или Email уже существует" 
            });
        }

        // 2. Сохраняем Username, чтобы потом по нему можно было войти в Login
        var user = new User
        {
            Username = model.Login,
            Email = model.Email,
            Password = _passwordHasher.Hash(model.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // 3. Сразу авторизуем пользователя (выдаем куку), так как клиент сразу переходит на BooksViewModel
        await SignInUserAsync(user);

        return new OkObjectResult(new ApiResponse<string> { Status = true, Data = "Регистрация успешна" });
    }

    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == model.Login);

        if (user == null || !_passwordHasher.Verify(model.Password, user.Password))
        {
            return new UnauthorizedObjectResult(new ApiResponse<string> 
            { 
                Status = false, 
                Data = "Неверный логин или пароль" 
            });
        }

        await SignInUserAsync(user);

        return new OkObjectResult(new ApiResponse<string> { Status = true, Message = "Вход выполнен",
            Data = user.Role ?? "User" });
    }

    private async Task SignInUserAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "User") 
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        if (_httpContextAccessor.HttpContext != null)
        {
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                principal);
        }
    }

}
