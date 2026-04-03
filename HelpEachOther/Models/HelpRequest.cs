using System.ComponentModel.DataAnnotations;

namespace HelpEachOther.Models;

public class HelpRequest
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    public HelpRequestStatus Status { get; set; } = HelpRequestStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    [Required]
    public string OwnerId { get; set; } = string.Empty;
    public ApplicationUser Owner { get; set; } = default!;

    public string? HelperId { get; set; }
    public ApplicationUser? Helper { get; set; }
}
