using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Client.Services;
using Library.Data.DTOs;

namespace Library.Client.ViewModels;

public partial class AddBookViewModel : ViewModelBase
{
    private readonly BookApiService _bookApiService = new();

    [ObservableProperty] private string _isbn = string.Empty;
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _authorFirstName = string.Empty;
    [ObservableProperty] private string _authorLastName = string.Empty;
    [ObservableProperty] private int _year = 2024;
    [ObservableProperty] private string _genre = string.Empty;
    [ObservableProperty] private int _count = 1;
    [ObservableProperty] private string? _errorMessage;

    [RelayCommand]
    private async Task SaveBookAsync()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(Isbn) || string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "Заполните ISBN и название книги";
            return;
        }

        var dto = new CreateBookDto
        {
            Isbn = Isbn,
            Title = Title,
            AuthorFirstName = AuthorFirstName,
            AuthorLastName = AuthorLastName,
            Year = Year,
            Genre = Genre,
            Count = Count
        };

        var response = await _bookApiService.AddBookAsync(dto);

        if (response.Status)
        {
            NavigationService.Instance.NavigateTo<BooksViewModel>();
        }
        else
        {
            ErrorMessage = response.Message;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigationService.Instance.NavigateTo<BooksViewModel>();
    }
}