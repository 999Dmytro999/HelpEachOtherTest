using HelpEachOther.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpEachOther.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<HelpRequest> HelpRequests => Set<HelpRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<HelpRequest>()
            .HasOne(r => r.Owner)
            .WithMany(u => u.CreatedRequests)
            .HasForeignKey(r => r.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<HelpRequest>()
            .HasOne(r => r.Helper)
            .WithMany(u => u.AcceptedRequests)
            .HasForeignKey(r => r.HelperId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
