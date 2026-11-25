using Library.Data.DTOs;
using Library.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Reg([FromBody] RegisterDto model)
    {
        return await auth.Register(model);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        return await auth.Login(model);
    }
}