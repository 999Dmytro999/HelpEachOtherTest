using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HelpEachOther.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string? DisplayName { get; set; }

    public int SoulPoints { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<HelpRequest> CreatedRequests { get; set; } = new List<HelpRequest>();
    public ICollection<HelpRequest> AcceptedRequests { get; set; } = new List<HelpRequest>();
}
