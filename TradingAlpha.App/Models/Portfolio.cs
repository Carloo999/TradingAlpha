using TradingAlpha.App.Data;

namespace TradingAlpha.App.Models;

public class Portfolio
{ 
    public int Id { get; set; }
    public ICollection<PortfolioEntry> PortfolioEntries { get; set; }
    
    public ApplicationUser User { get; set; }
}