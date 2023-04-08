using Microsoft.AspNetCore.Identity;

namespace DietPlanner.DAL.Entities;

public class AppUser : IdentityUser
{
    public string SpoonacularUserName { get; set; }
    public string Hash { get; set; }
}
