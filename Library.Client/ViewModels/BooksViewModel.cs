using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Client.Services;
using Library.Data.DTOs;

namespace Library.Client.ViewModels;

public partial class BooksViewModel : ViewModelBase
{
    private readonly BookApiService _bookApiService = new();
    
    // Полный кэш книг с сервера, чтобы не терять данные при фильтрации
    private List<BookCatalogRowDto> _allBooks = [];

    // Отфильтрованный список, который отображается в DataGrid
    [ObservableProperty]
    private List<BookCatalogRowDto> _books = [];
    
    [ObservableProperty]
    private BookCatalogRowDto? _selectedBook;

    // Свойства поиска и фильтров (обязательно [ObservableProperty]!)
    [ObservableProperty] 
    private string _searchTb = string.Empty;

    [ObservableProperty]
    private List<string> _authorFilter = ["Все авторы"];

    [ObservableProperty]
    private string _selectedAuthor = "Все авторы";

    [ObservableProperty]
    private List<string> _genreFilter = ["Все жанры"];

    [ObservableProperty]
    private string _selectedGenre = "Все жанры";

    public bool IsAdmin => UserSession.IsAdmin;

    public BooksViewModel()
    {
        _ = LoadBooksAsync();
    }

    [RelayCommand]
    private async Task LoadBooksAsync()
    {
        _allBooks = await _bookApiService.GetBooksAsync();

        // Формируем списки для ComboBox с пунктом для сброса фильтра
        AuthorFilter = ["Все авторы", .. _allBooks.Select(b => b.Author).Distinct().Order()];
        GenreFilter = ["Все жанры", .. _allBooks.Select(b => b.Genre).Distinct().Order()];

        SelectedAuthor = "Все авторы";
        SelectedGenre = "Все жанры";

        ApplyFilters();
    }

    // Эти методы автоматически вызываются CommunityToolkit.Mvvm при любом изменении поля поиска или комбобоксов
    partial void OnSearchTbChanged(string value) => ApplyFilters();
    partial void OnSelectedAuthorChanged(string value) => ApplyFilters();
    partial void OnSelectedGenreChanged(string value) => ApplyFilters();

    private void ApplyFilters()
    {
        IEnumerable<BookCatalogRowDto> query = _allBooks;

        // 1. Поиск по названию, автору или ISBN (без учета регистра)
        if (!string.IsNullOrWhiteSpace(SearchTb))
        {
            query = query.Where(b =>
                b.Title.Contains(SearchTb, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(SearchTb, StringComparison.OrdinalIgnoreCase) ||
                b.Isbn.Contains(SearchTb, StringComparison.OrdinalIgnoreCase));
        }

        // 2. Фильтр по автору
        if (!string.IsNullOrEmpty(SelectedAuthor) && SelectedAuthor != "Все авторы")
        {
            query = query.Where(b => b.Author == SelectedAuthor);
        }

        // 3. Фильтр по жанру
        if (!string.IsNullOrEmpty(SelectedGenre) && SelectedGenre != "Все жанры")
        {
            query = query.Where(b => b.Genre == SelectedGenre);
        }

        Books = query.ToList();
    }

    [RelayCommand]
    private void ResetFilters()
    {
        SearchTb = string.Empty;
        SelectedAuthor = "Все авторы";
        SelectedGenre = "Все жанры";
    }

    [RelayCommand]
    private void OpenAddBook()
    {
        NavigationService.Instance.NavigateTo<AddBookViewModel>();
    }

    [RelayCommand]
    private void OpenBookCard()
    {
        if (SelectedBook != null)
        {
            NavigationService.Instance.NavigateTo(new BookCardViewModel(SelectedBook.Isbn));
        }
    }
}
