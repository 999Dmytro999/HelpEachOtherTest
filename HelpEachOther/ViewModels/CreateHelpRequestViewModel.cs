using System.ComponentModel.DataAnnotations;

namespace HelpEachOther.ViewModels;

public class CreateHelpRequestViewModel
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;
}
