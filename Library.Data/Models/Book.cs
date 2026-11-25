using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.Data.Models;

public class Book
{
    [Key]
    public Guid Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Genre { get; set; } = string.Empty;
    
    [ForeignKey("AuthorId")]
    public Guid AuthorId { get; set; } = Guid.Empty;
    
    
    public string Isbn { get; set; } = string.Empty;
    
    public int Year { get; set; } = DateTime.UtcNow.Year;

    public int Count { get; set; } = 0;
    
    [JsonIgnore]
    public Author? Author { get; set; }
    
    [JsonIgnore]
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    
}