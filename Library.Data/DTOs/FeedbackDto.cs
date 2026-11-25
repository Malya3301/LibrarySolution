using System.ComponentModel.DataAnnotations;

namespace Library.Data.DTOs;

public class FeedbackDto
{
    public string Author { get; set; }
    
    public string Content { get; set; }
    
    [Range(1,5)]
    public int Rating { get; set; }
}