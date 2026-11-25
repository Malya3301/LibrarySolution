using Library.Data.Models;

namespace Library.Data.DTOs;

public class BookCatalogRowDto
{
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public int Count { get; set; }
    public double Rating { get; set; }
}