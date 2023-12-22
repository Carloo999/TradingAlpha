using System.ComponentModel.DataAnnotations;

namespace TradingAlpha.App.Enums;

public enum Roles
{
    Admin,
    [Display(Name="Premium Member")]
    PremiumMember,
    User
}