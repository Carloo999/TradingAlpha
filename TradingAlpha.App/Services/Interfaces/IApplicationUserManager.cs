using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Services.Interfaces;

public interface IApplicationUserManager
{
    Task CreateUser(ApplicationUser user, string password, List<Roles> roles);

    Task<IdentityResult> DeleteUser(ApplicationUser user);

    Task<ApplicationUser?> GetUserByAuthState(AuthenticationState authenticationState);
    
    Task<IList<string>> GetUserRole(ApplicationUser user);

    Task<IdentityResult> CreateDefaultUser(ApplicationUser user, string password);

    Task<(bool, bool)> GetIsAuthorized(ApplicationUser owner, ApplicationUser? currentUser);

    Task<IdentityResult> ChangePrivacySetting(ApplicationUser user, PrivacyStatus status);

    List<ApplicationUser> GetUsers();

    Task<decimal> CalcPortfolioWorth(ApplicationUser user);
}