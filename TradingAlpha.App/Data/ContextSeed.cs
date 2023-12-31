using Microsoft.AspNetCore.Identity;
using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Data;

public class ContextSeed
{
    public static async Task SeedDefaultRoles(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.PremiumMember.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    }
}