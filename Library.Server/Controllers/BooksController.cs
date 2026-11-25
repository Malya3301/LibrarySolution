using System.Security.Claims;
using Library.Data.DTOs;
using Library.Data.Responses;
using Library.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("books")]
[Authorize] 
public class BooksController(IMainService main) : ControllerBase
{
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        return await main.GetAllBooks();
    }

    [HttpPost("add")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> AddBook([FromBody] CreateBookDto model)
    {
        return await main.AddBook(model);
    }

    [HttpPost("feedback")]
    public async Task<IActionResult> AddFeedback([FromBody] CreateFeedbackDto model)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return Unauthorized(new ApiResponse<string> 
            { 
                Status = false, 
                Message = "Не удалось определить пользователя" 
            });
        }

        return await main.AddFeedback(model, userId);
    }


    [HttpGet("{isbn}")]
    public async Task<IActionResult> GetBookByIsbn(string isbn)
    {
        return await main.GetBookByIsbn(isbn);
    }
}