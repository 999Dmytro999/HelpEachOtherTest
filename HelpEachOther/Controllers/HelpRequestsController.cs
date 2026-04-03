using HelpEachOther.Data;
using HelpEachOther.Models;
using HelpEachOther.Services;
using HelpEachOther.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelpEachOther.Controllers;

public class HelpRequestsController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    HelpRequestService helpRequestService) : Controller
{
    public async Task<IActionResult> Index(HelpRequestStatus? status)
    {
        var query = context.HelpRequests
            .Include(r => r.Owner)
            .Include(r => r.Helper)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        ViewBag.Status = status;
        return View(await query.OrderByDescending(r => r.CreatedAt).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var request = await context.HelpRequests
            .Include(r => r.Owner)
            .Include(r => r.Helper)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request is null) return NotFound();

        return View(request);
    }

    [Authorize]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(HelpCategories.All);
        return View(new CreateHelpRequestViewModel());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateHelpRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(HelpCategories.All);
            return View(model);
        }

        var userId = userManager.GetUserId(User);
        if (userId is null) return Challenge();

        var request = new HelpRequest
        {
            Title = model.Title.Trim(),
            Description = model.Description.Trim(),
            Category = model.Category,
            City = model.City.Trim(),
            OwnerId = userId,
            Status = HelpRequestStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        context.HelpRequests.Add(request);
        await context.SaveChangesAsync();

        TempData["Success"] = "Help request created successfully.";
        return RedirectToAction(nameof(Details), new { id = request.Id });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Volunteer(int id)
    {
        var request = await context.HelpRequests.FirstOrDefaultAsync(r => r.Id == id);
        if (request is null) return NotFound();

        var currentUserId = userManager.GetUserId(User);
        if (currentUserId is null) return Challenge();

        if (request.OwnerId == currentUserId)
        {
            TempData["Error"] = "You cannot volunteer for your own request.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (request.Status != HelpRequestStatus.Open || !string.IsNullOrEmpty(request.HelperId))
        {
            TempData["Error"] = "This request is no longer available.";
            return RedirectToAction(nameof(Details), new { id });
        }

        request.HelperId = currentUserId;
        request.Status = HelpRequestStatus.InProgress;
        await context.SaveChangesAsync();

        TempData["Success"] = "You are now assigned as helper.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var currentUserId = userManager.GetUserId(User);
        if (currentUserId is null) return Challenge();

        var result = await helpRequestService.CompleteRequestAsync(id, currentUserId);
        TempData[result.success ? "Success" : "Error"] = result.success
            ? "Request marked as completed. Soul points awarded."
            : result.error;

        return RedirectToAction(nameof(Details), new { id });
    }
}
