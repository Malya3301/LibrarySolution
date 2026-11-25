using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Models;

public class Issuance
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("BookId")]
    public Guid BookId { get; set; } = Guid.Empty;
    
    [ForeignKey("UserId")]
    public Guid UserId { get; set; } = Guid.Empty;
    
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    
    public int DurationDays { get; set; } = 14;
    
    public DateTime? ReturnDate { get; set; }
    
    [AllowedValues("Активна", "Возвращена", "Просрочена", 
        ErrorMessage = "Статус должен быть 'Активна', 'Возвращена' или 'Просрочена'.")]
    public string Status { get; set; } = string.Empty;
    
    
}