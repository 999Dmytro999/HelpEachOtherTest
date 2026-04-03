using HelpEachOther.Data;
using HelpEachOther.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpEachOther.Services;

public class HelpRequestService(ApplicationDbContext context)
{
    public const int SoulPointsPerCompletedHelp = 10;

    public async Task<(bool success, string error)> CompleteRequestAsync(int requestId, string currentUserId)
    {
        var request = await context.HelpRequests
            .Include(r => r.Helper)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request is null) return (false, "Request not found.");
        if (request.OwnerId != currentUserId) return (false, "Only the owner can complete this request.");
        if (request.Status != HelpRequestStatus.InProgress) return (false, "Only in-progress requests can be completed.");
        if (request.Helper is null) return (false, "Cannot complete request without an assigned helper.");

        request.Status = HelpRequestStatus.Completed;
        request.CompletedAt = DateTime.UtcNow;
        request.Helper.SoulPoints += SoulPointsPerCompletedHelp;

        await context.SaveChangesAsync();
        return (true, string.Empty);
    }
}
