using Library.Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Interfaces;

public interface IMainService
{
    public Task<IActionResult> GetAllBooks();
    
    public Task<IActionResult> GetBookByIsbn(string isbn);
    
    public Task<IActionResult> AddBook(CreateBookDto model);
    
    public Task<IActionResult> AddFeedback(CreateFeedbackDto model, Guid userId);
}