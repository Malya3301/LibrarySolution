using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library.Data.Models;

public class Author
{
    [Key]
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string Biography { get; set; } = string.Empty;
    
    public int BirthYear { get; set; } = DateTime.UtcNow.Year;
    
    [JsonIgnore]
    public ICollection<Book>? Books = new List<Book>();
}