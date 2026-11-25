namespace Library.Data.DTOs;

public class CreateBookDto
{
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string AuthorFirstName { get; set; } = string.Empty;
    public string AuthorLastName { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Count { get; set; }
}