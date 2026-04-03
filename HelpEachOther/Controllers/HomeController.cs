using HelpEachOther.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpEachOther.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var openRequestsCount = await context.HelpRequests.CountAsync(r => r.Status == Models.HelpRequestStatus.Open);
        var completedCount = await context.HelpRequests.CountAsync(r => r.Status == Models.HelpRequestStatus.Completed);

        ViewBag.OpenRequestsCount = openRequestsCount;
        ViewBag.CompletedCount = completedCount;
        return View();
    }
}
