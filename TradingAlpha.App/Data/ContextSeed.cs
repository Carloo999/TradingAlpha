using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Data;

public static class ContextSeed
{
    public static async Task SeedDefaultRoles(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.PremiumMember.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    }

    public static async Task SeedOwnerUser(UserManager<ApplicationUser> userManager, IApplicationUserManager applicationUserManager)
    {
        var cred = await GetOwnerCredentials();
        var owner = new ApplicationUser
        {
            FirstName = "Owner",
            LastName = "Alpha",
            UserName = cred.Item1,
            Email = cred.Item1,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            Balance = Decimal.MaxValue
        };

        if (userManager.Users.All(u => u.Id != owner.Id))
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(owner.Email);

            if (user is null)
            {
                await applicationUserManager.CreateUser(owner, cred.Item2, [Roles.Admin, Roles.PremiumMember]);
            }
        }
    }

    private static async Task<Tuple<string,string>> GetOwnerCredentials()
    {
        using var streamReader = new StreamReader("credentials.json");
        var jsonString = await streamReader.ReadToEndAsync();
        
        using JsonDocument document = JsonDocument.Parse(jsonString);
        var credentials = document.RootElement.GetProperty("OwnerCredentials");
        return new Tuple<string, string>(
            credentials.GetProperty("Email").ToString(),
            credentials.GetProperty("Password").ToString());
    }
}