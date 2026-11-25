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

public class MainService : IMainService
{
    private readonly DataBase _db;
    
    public MainService(DataBase db)
    {
        _db = db;
    }
    
    public async Task<IActionResult> GetAllBooks()
    {
        List<BookCatalogRowDto> books = await _db.Books
            .Select(b => new BookCatalogRowDto
            {
                Isbn = b.Isbn,
                Title = b.Title,
                Author = b.Author.FirstName + " " + b.Author.LastName,
                Year = b.Year,
                Genre = b.Genre,
                Count = b.Count,
                Rating = b.Feedbacks.Any()
                    ? Math.Round(b.Feedbacks.Average(f => f.Rating), 1)
                    : 0
            })
            .ToListAsync();
        
        return new OkObjectResult(new ApiResponse<List<BookCatalogRowDto>>()
        {
            Status = true,
            Message = "Books retrieved successfully",
            Data = books
        });
    }

    public async Task<IActionResult> GetBookByIsbn(string isbn)
    {
        // 1. Загружаем книгу вместе с автором и отзывами (и авторами отзывов) в память
        var bookEntity = await _db.Books
            .Include(b => b.Author)
            .Include(b => b.Feedbacks)
            .ThenInclude(f => f.User)
            .FirstOrDefaultAsync(b => b.Isbn == isbn);

        if (bookEntity == null)
        {
            return new NotFoundObjectResult(new ApiResponse<BookDto>
            {
                Status = false,
                Message = "Книга с таким ISBN не найдена"
            });
        }

        // 2. Собираем DTO и считаем рейтинг уже в C#, где любые вычисления работают без ограничений SQL
        var bookDto = new BookDto
        {
            Isbn = bookEntity.Isbn,
            Title = bookEntity.Title,
            Author = bookEntity.Author != null 
                ? $"{bookEntity.Author.FirstName} {bookEntity.Author.LastName}" 
                : "Неизвестный автор",
            Year = bookEntity.Year,
            Genre = bookEntity.Genre,
            Count = bookEntity.Count,
        
            Rating = bookEntity.Feedbacks.Any()
                ? Math.Round(bookEntity.Feedbacks.Average(f => f.Rating), 1)
                : 0,
            
            Feedbacks = bookEntity.Feedbacks.Select(f => new FeedbackDto
            {
                Author = f.User?.Username ?? "Пользователь",
                Content = f.Comment,
                Rating = f.Rating
            }).ToList()
        };

        return new OkObjectResult(new ApiResponse<BookDto>
        {
            Status = true,
            Message = "Book retrieved successfully",
            Data = bookDto
        });
    }

    public async Task<IActionResult> AddFeedback(CreateFeedbackDto model, Guid userId)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Isbn == model.BookIsbn);
        if (book == null)
        {
            return new NotFoundObjectResult(new ApiResponse<string>
            {
                Status = false,
                Message = "Книга не найдена"
            });
        }

        var feedback = new Feedback
        {
            Id = Guid.NewGuid(),
            BookId = book.Id,
            UserId = userId,
            Rating = model.Rating,
            Comment = model.Content
        };

        _db.Feedbacks.Add(feedback);
        await _db.SaveChangesAsync();

        return new OkObjectResult(new ApiResponse<string>
        {
            Status = true,
            Message = "Отзыв успешно добавлен"
        });
    }

    
    public async Task<IActionResult> AddBook(CreateBookDto model)
    {
        if (await _db.Books.AnyAsync(b => b.Isbn == model.Isbn))
        {
            return new BadRequestObjectResult(new ApiResponse<string>
            {
                Status = false,
                Message = "Книга с таким ISBN уже существует"
            });
        }

        // Ищем автора в базе или создаем нового, если его еще нет
        var author = await _db.Authors.FirstOrDefaultAsync(a => 
            a.FirstName == model.AuthorFirstName && a.LastName == model.AuthorLastName);

        if (author == null)
        {
            author = new Author
            {
                FirstName = model.AuthorFirstName,
                LastName = model.AuthorLastName
            };
            _db.Authors.Add(author);
        }

        var book = new Book
        {
            Isbn = model.Isbn,
            Title = model.Title,
            Author = author,
            Year = model.Year,
            Genre = model.Genre,
            Count = model.Count
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return new OkObjectResult(new ApiResponse<string>
        {
            Status = true,
            Message = "Книга успешно добавлена"
        });
    }


}