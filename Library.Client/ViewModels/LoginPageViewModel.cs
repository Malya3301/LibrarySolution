using System.Threading.Tasks;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Client.Services;
using Library.Client.Views;
using Library.Data.DTOs;

namespace Library.Client.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
    private readonly AuthApiService api = new AuthApiService();

    [ObservableProperty]
    private string login = string.Empty;
    
    [ObservableProperty]
    private string password = string.Empty;
    
    [ObservableProperty] private string? _errorMessage;

    [RelayCommand]
    private async Task LoginAsync()
    {
        
        ErrorMessage = string.Empty;
        
        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            return;
        }

        var dto = new LoginDto
        {
            Login = Login,
            Password = Password
        };

        var response = await api.LoginAsync(dto);

        if (response.Status)
        {
            UserSession.Role = response.Data ?? "User";
            NavigationService.Instance.NavigateTo<BooksViewModel>();

        }
        else
        {
            ErrorMessage = "Неправильный пароль.";
        }
    }

    [RelayCommand]
    private void GoToRegister()
    {
        // Должно быть строго так:
        NavigationService.Instance.NavigateTo<RegisterViewModel>();
    }

    
}