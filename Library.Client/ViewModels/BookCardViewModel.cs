using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Client.Services;
using Library.Data.DTOs;

namespace Library.Client.ViewModels;

public partial class BookCardViewModel : ViewModelBase
{
    private readonly BookApiService _bookApiService = new();
    
    [ObservableProperty] 
    private BookDto? _book;

    [ObservableProperty]
    private int _selectedRating = 5;

    [ObservableProperty]
    private string _newComment = string.Empty;

    [ObservableProperty]
    private string? _feedbackError;

    public List<int> RatingOptions { get; } = new List<int>(){1,2,3,4,5};

    public BookCardViewModel(string isbn)
    {
        _ = LoadBookAsync(isbn);
    }

    private async Task LoadBookAsync(string isbn)
    {
        Book = await _bookApiService.GetBookByIsbnAsync(isbn);
    }

    [RelayCommand]
    private async Task SendFeedbackAsync()
    {
        FeedbackError = null;

        if (Book == null)
            return;

        if (string.IsNullOrWhiteSpace(NewComment))
        {
            FeedbackError = "Напишите текст отзыва";
            return;
        }

        var dto = new CreateFeedbackDto
        {
            BookIsbn = Book.Isbn,
            Rating = SelectedRating,
            Content = NewComment
        };

        var response = await _bookApiService.AddFeedbackAsync(dto);

        if (response.Status)
        {
             NewComment = string.Empty;
            SelectedRating = 5;
            await LoadBookAsync(Book.Isbn);
        }
        else
        {
            FeedbackError = response.Message;
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        NavigationService.Instance.NavigateTo<BooksViewModel>();
    }
}