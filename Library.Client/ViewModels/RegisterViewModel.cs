using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Client.Services;
using Library.Data.DTOs;

namespace Library.Client.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly AuthApiService _api = new();

    [ObservableProperty]
    private string _login = string.Empty;
    
    [ObservableProperty]
    private string _email = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Заполните все поля";
            return;
        }

        var dto = new RegisterDto
        {
            Login = Login,
            Password = Password,
            Email = Email
        };

        var response = await _api.RegisterAsync(dto);

        if (response.Status)
        {
            // Важно: чтобы после регистрации пустило в BooksViewModel, 
            // сервер в методе Register тоже должен вызывать SignInAsync!
            NavigationService.Instance.NavigateTo<BooksViewModel>();
        }
        else
        {
            ErrorMessage = response.Data;
        }
    }

    // Генерирует свойство GoToLoginCommand для кнопки "Войти"
    [RelayCommand]
    private void GoToLogin()
    {
        NavigationService.Instance.NavigateTo<LoginPageViewModel>();
    }
}