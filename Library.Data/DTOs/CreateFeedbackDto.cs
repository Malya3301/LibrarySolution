using System.ComponentModel.DataAnnotations;

namespace Library.Data.DTOs;

public class CreateFeedbackDto
{
    public string BookIsbn { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;
}