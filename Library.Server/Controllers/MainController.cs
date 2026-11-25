using Library.Data.DTOs;
using Library.Data.Responses;
using Library.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("main")]
[Authorize]
public class MainController(IMainService main) : ControllerBase
{
    
    
    [HttpGet ("catalog")]
    public async Task<IActionResult> GetCatalog()
    {
        return await main.GetAllBooks();
    }
}