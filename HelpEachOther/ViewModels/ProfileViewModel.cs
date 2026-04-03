using HelpEachOther.Models;

namespace HelpEachOther.ViewModels;

public class ProfileViewModel
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int SoulPoints { get; set; }
    public int CompletedHelpsCount { get; set; }
    public int CreatedRequestsCount { get; set; }
    public int AcceptedHelpsCount { get; set; }
    public List<HelpRequest> RecentCreatedRequests { get; set; } = [];
    public List<HelpRequest> RecentHelpingRequests { get; set; } = [];
}
