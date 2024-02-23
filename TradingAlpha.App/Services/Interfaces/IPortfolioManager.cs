using TradingAlpha.App.Data;
using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services.Interfaces;

public interface IPortfolioManager
{
    Task ChangeEntryForBuyTransaction(Transaction transaction);
    
    Task SaveEntryChanges();

    Task<ICollection<PortfolioEntry>> GetEntries(ApplicationUser user);

    Task DeleteEntry(PortfolioEntry entry);
}