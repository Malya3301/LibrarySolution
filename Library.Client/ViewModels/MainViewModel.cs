using CommunityToolkit.Mvvm.ComponentModel;
using Library.Client.Services;

namespace Library.Client.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public NavigationService Navigation => NavigationService.Instance;

    public MainViewModel()
    {
        Navigation.NavigateTo<LoginPageViewModel>();
    }
}