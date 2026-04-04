using HelpEachOther.Data;
using HelpEachOther.Models;
using HelpEachOther.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpEachOther.Controllers;

[Authorize]
public class ProfileController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var createdRequests = await context.HelpRequests
            .Where(r => r.OwnerId == user.Id)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var helpingRequests = await context.HelpRequests
            .Where(r => r.HelperId == user.Id)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var vm = new ProfileViewModel
        {
            Username = user.UserName ?? string.Empty,
            DisplayName = user.DisplayName ?? user.UserName ?? string.Empty,
            SoulPoints = user.SoulPoints,
            CreatedRequestsCount = createdRequests.Count,
            AcceptedHelpsCount = helpingRequests.Count,
            CompletedHelpsCount = helpingRequests.Count(r => r.Status == HelpRequestStatus.Completed),
            RecentCreatedRequests = createdRequests.Take(5).ToList(),
            RecentHelpingRequests = helpingRequests.Take(5).ToList()
        };

        return View(vm);
    }

    public async Task<IActionResult> MyRequests(HelpRequestStatus? status)
    {
        var userId = userManager.GetUserId(User);
        if (userId is null) return Challenge();

        var query = context.HelpRequests
            .Include(r => r.Helper)
            .Where(r => r.OwnerId == userId)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        ViewBag.Status = status;

        return View(items);
    }

    public async Task<IActionResult> MyHelping()
    {
        var userId = userManager.GetUserId(User);
        if (userId is null) return Challenge();

        var items = await context.HelpRequests
            .Include(r => r.Owner)
            .Where(r => r.HelperId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return View(items);
    }
}
