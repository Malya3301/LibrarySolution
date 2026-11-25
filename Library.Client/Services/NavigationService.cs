using CommunityToolkit.Mvvm.ComponentModel;
using Library.Client.ViewModels;

namespace Library.Client.Services;

public partial class NavigationService : ObservableObject
{
    // Единый экземпляр сервиса на всё приложение (если не используете DI-контейнер)
    public static NavigationService Instance { get; } = new();

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    // Метод для перехода на любую ViewModel
    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentViewModel = viewModel;
    }

    // Удобный generic-метод, если у ViewModel пустой конструктор: NavigateTo<BooksViewModel>()
    public void NavigateTo<T>() where T : ViewModelBase, new()
    {
        CurrentViewModel = new T();
    }
}