using DietPlanner.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.DAL.Context;

public class DietPlannerContext : IdentityDbContext
{
    public DietPlannerContext(DbContextOptions<DietPlannerContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public DbSet<AppUser> AppUsers { get; set; }
}
