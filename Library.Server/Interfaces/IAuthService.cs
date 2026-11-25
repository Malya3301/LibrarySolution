using Library.Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Interfaces;

public interface IAuthService
{
    public Task<IActionResult> Register(RegisterDto model);
    
    public Task<IActionResult> Login(LoginDto model);
    
    
}