using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.Data.Models;

public class Feedback
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("BookId")]
    public Guid BookId { get; set; } = Guid.Empty;
    
    [ForeignKey("UserId")]
    public Guid UserId { get; set; } = Guid.Empty;
    
    
    public int Rating { get; set; } = 1;
    
    
    public string Comment { get; set; } = string.Empty;
    
    [JsonIgnore]
    public Book? Book { get; set; }
    
    [JsonIgnore]
    public User? User { get; set; }
}