using HelpEachOther.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpEachOther.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await context.HelpRequests.AnyAsync()) return;

        var seedUser = await userManager.FindByNameAsync("demo@help.com");
        if (seedUser is null)
        {
            seedUser = new ApplicationUser
            {
                UserName = "demo@help.com",
                Email = "demo@help.com",
                EmailConfirmed = true,
                DisplayName = "Demo User"
            };
            await userManager.CreateAsync(seedUser, "Password1!");
        }

        var demoRequests = new List<HelpRequest>
        {
            new()
            {
                Title = "Need groceries for this week",
                Description = "I have limited mobility this week and need someone to help buy basic groceries.",
                Category = "Groceries",
                City = "New York",
                OwnerId = seedUser.Id
            },
            new()
            {
                Title = "Need ride to clinic",
                Description = "Looking for a ride to a nearby clinic tomorrow morning.",
                Category = "Transport",
                City = "Chicago",
                OwnerId = seedUser.Id
            },
            new()
            {
                Title = "Help moving boxes",
                Description = "Need help moving a few boxes into my new apartment.",
                Category = "Moving",
                City = "Austin",
                OwnerId = seedUser.Id
            }
        };

        context.HelpRequests.AddRange(demoRequests);
        await context.SaveChangesAsync();
    }
}
