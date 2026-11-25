using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Library.Client.Views;

public partial class LoginPage : UserControl
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void Register_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Content = new RegisterPage();
    }
    
    
    
}