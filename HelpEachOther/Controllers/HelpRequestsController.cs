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
    private static SelectList GetCategoriesSelectList() => new(HelpCategories.All);

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
        ViewBag.Categories = GetCategoriesSelectList();
        return View(new CreateHelpRequestViewModel());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateHelpRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = GetCategoriesSelectList();
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
    public async Task<IActionResult> Edit(int id)
    {
        var request = await context.HelpRequests.FirstOrDefaultAsync(r => r.Id == id);
        if (request is null) return NotFound();

        var userId = userManager.GetUserId(User);
        if (userId is null) return Challenge();

        if (request.OwnerId != userId)
        {
            TempData["Error"] = "You are not allowed to modify this request.";
            return RedirectToAction(nameof(Index));
        }

        if (request.Status != HelpRequestStatus.Open)
        {
            TempData["Error"] = "Only open requests can be edited.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var model = new EditHelpRequestViewModel
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            City = request.City
        };

        ViewBag.Categories = GetCategoriesSelectList();
        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditHelpRequestViewModel model)
    {
        if (id != model.Id) return NotFound();

        var userId = userManager.GetUserId(User);
        if (userId is null) return Challenge();

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = GetCategoriesSelectList();
            return View(model);
        }

        var request = await context.HelpRequests.FirstOrDefaultAsync(r => r.Id == id);
        if (request is null) return NotFound();

        if (request.OwnerId != userId)
        {
            TempData["Error"] = "You are not allowed to modify this request.";
            return RedirectToAction(nameof(Index));
        }

        if (request.Status != HelpRequestStatus.Open)
        {
            TempData["Error"] = "Only open requests can be edited.";
            return RedirectToAction(nameof(Details), new { id });
        }

        request.Title = model.Title.Trim();
        request.Description = model.Description.Trim();
        request.Category = model.Category;
        request.City = model.City.Trim();

        await context.SaveChangesAsync();

        TempData["Success"] = "Help request updated successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var request = await context.HelpRequests
            .Include(r => r.Owner)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (request is null) return NotFound();

        var userId = userManager.GetUserId(User);
        if (userId is null) return Challenge();

        if (request.OwnerId != userId)
        {
            TempData["Error"] = "You are not allowed to modify this request.";
            return RedirectToAction(nameof(Index));
        }

        if (request.Status != HelpRequestStatus.Open)
        {
            TempData["Error"] = "Only open requests can be deleted.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(request);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var request = await context.HelpRequests.FirstOrDefaultAsync(r => r.Id == id);
        if (request is null) return NotFound();

        var userId = userManager.GetUserId(User);
        if (userId is null) return Challenge();

        if (request.OwnerId != userId)
        {
            TempData["Error"] = "You are not allowed to modify this request.";
            return RedirectToAction(nameof(Index));
        }

        if (request.Status != HelpRequestStatus.Open)
        {
            TempData["Error"] = "Only open requests can be deleted.";
            return RedirectToAction(nameof(Details), new { id });
        }

        context.HelpRequests.Remove(request);
        await context.SaveChangesAsync();

        TempData["Success"] = "Help request deleted successfully.";
        return RedirectToAction("MyRequests", "Profile");
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
