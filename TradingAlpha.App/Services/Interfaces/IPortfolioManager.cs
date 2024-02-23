using TradingAlpha.App.Data;
using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services.Interfaces;

public interface IPortfolioManager
{
    Task ChangeEntryForBuyTransaction(Transaction transaction);
    
    void ChangeEntryForSellTransaction(Transaction transaction, PortfolioEntry entry);
    
    Task<PortfolioEntry[]> GetEntries(ApplicationUser user);

    Task<Portfolio> GetUserPortfolio(ApplicationUser user);
}