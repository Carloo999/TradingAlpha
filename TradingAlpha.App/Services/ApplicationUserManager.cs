using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services;

public class ApplicationUserManager : IApplicationUserManager
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;
    
    public ApplicationUserManager(UserManager<ApplicationUser> userManager, ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
    {
        _userManager = userManager;
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    private async Task<IdentityResult> AddUserToDb(ApplicationUser user, string password)
    {
        var portfolio = new Portfolio();
        user.Portfolio = portfolio;
        user.PortfolioId = user.Portfolio.Id;
        
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> CreateDefaultUser(ApplicationUser user, string password)
    {
        IdentityResult r1= await AddUserToDb(user, password);
        IdentityResult r2= await _userManager.AddToRoleAsync(user, Roles.User.ToString());

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
            await _userManager.AddToRoleAsync(user, role.ToString());
        }
    }

    public async Task<IdentityResult> DeleteUser(ApplicationUser user)
    {
        IdentityResult result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded) return result;
        
        var userBooks = GetUserPortfolioEntries(user);
        foreach (PortfolioEntry entry in userBooks)
        {
            _db.PortfolioEntries.Remove(entry);
        }

        _db.Portfolios.Remove(GetUserPortfolio(user));
            
        await _db.SaveChangesAsync();

        return result;
    }

    public Portfolio GetUserPortfolio(ApplicationUser user)
    {
        var b = _db.Portfolios.Where(bl => user.PortfolioId == bl.Id);
        return b.First();
    }
    
    public List<PortfolioEntry> GetUserPortfolioEntries(ApplicationUser user)
    {
        return _db.PortfolioEntries.Where(b => b.PortfolioId == user.PortfolioId).ToList();
    }

    public async Task<ApplicationUser?> GetUserByAuthState(AuthenticationState authState)
    {
        ClaimsPrincipal claims = authState.User;
        ApplicationUser? user = await _userManager.GetUserAsync(claims);
        return user;
    }

    public async Task<IList<string>> GetUserRole(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> ChangePrivacySetting(ApplicationUser user, PrivacyStatus status)
    {
        user.PrivacyStatus = status;
        return await _userManager.UpdateAsync(user);
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
        return _userManager.Users.ToList();
    }

    public Task<decimal> CalcPortfolioWorth(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}