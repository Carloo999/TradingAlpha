using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;

namespace TradingAlpha.App.Data;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; } 
    public PrivacyStatus PrivacyStatus { get; set; }

    [ForeignKey("PortfolioId")]
    public int PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }
}