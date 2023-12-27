using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class ApplicationUserManager(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext db,
    IWebHostEnvironment hostEnvironment)
    : IApplicationUserManager
{
    private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;

    private async Task<IdentityResult> AddUserToDb(ApplicationUser user, string password)
    {
        var portfolio = new Portfolio();
        user.Portfolio = portfolio;
        user.PortfolioId = user.Portfolio.Id;
        
        return await userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> CreateDefaultUser(ApplicationUser user, string password)
    {
        IdentityResult r1= await AddUserToDb(user, password);
        IdentityResult r2= await userManager.AddToRoleAsync(user, Roles.User.ToString());

        if (r1 == IdentityResult.Success && r2 == IdentityResult.Success)
        {
            return IdentityResult.Success;
        } 
        return IdentityResult.Failed();
    }
    
    public async Task CreateUser(ApplicationUser user, string password, List<Roles> roles)
    {
        await AddUserToDb(user, password);

        foreach (Roles role in roles)
        {
            await userManager.AddToRoleAsync(user, role.ToString());
        }
    }

    public async Task<IdentityResult> DeleteUser(ApplicationUser user)
    {
        IdentityResult result = await userManager.DeleteAsync(user);

        if (!result.Succeeded) return result;
        
        var userBooks = GetUserPortfolioEntries(user);
        foreach (PortfolioEntry entry in userBooks)
        {
            db.PortfolioEntries.Remove(entry);
        }

        db.Portfolios.Remove(GetUserPortfolio(user));
            
        await db.SaveChangesAsync();

        return result;
    }

    public Portfolio GetUserPortfolio(ApplicationUser user)
    {
        var b = db.Portfolios.Where(bl => user.PortfolioId == bl.Id);
        return b.First();
    }
    
    public List<PortfolioEntry> GetUserPortfolioEntries(ApplicationUser user)
    {
        return db.PortfolioEntries.Where(b => b.PortfolioId == user.PortfolioId).ToList();
    }

    public async Task<ApplicationUser?> GetUserByAuthState(AuthenticationState authState)
    {
        ClaimsPrincipal claims = authState.User;
        ApplicationUser? user = await userManager.GetUserAsync(claims);
        return user;
    }

    public async Task<IList<string>> GetUserRole(ApplicationUser user)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> ChangePrivacySetting(ApplicationUser user, PrivacyStatus status)
    {
        user.PrivacyStatus = status;
        return await userManager.UpdateAsync(user);
    }

    public async Task<(bool, bool)> GetIsAuthorized(ApplicationUser owner, ApplicationUser? currentUser)
    {
        PrivacyStatus status = owner.PrivacyStatus;
        var isDisplayed = false;
        var isEditable = false;

        if (status == PrivacyStatus.Public)
        {
            isDisplayed = true;
        }
        if (currentUser is null) return (isDisplayed, false);
        
        if (owner.Equals(currentUser))
        {
            isDisplayed = true;
            isEditable = true;
        }
        else
        {
            var roles = await GetUserRole(currentUser);

            if (!roles.Contains(Roles.Admin.ToString()))
                return (isDisplayed, isEditable);
            isDisplayed = true;
            isEditable = true;
        }

        return (isDisplayed, isEditable);
    }

    public List<ApplicationUser> GetUsers()
    {
        return userManager.Users.ToList();
    }

    public Task<decimal> CalcPortfolioWorth(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}