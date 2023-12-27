using TradingAlpha.App.Data;
using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services.Interfaces;

public interface IPortfolioManager
{
    Task AddNewEntry(PortfolioEntry entry, ApplicationUser user);
    
    Task SaveEntryChanges();

    Task<ICollection<PortfolioEntry>> GetEntries(ApplicationUser user);

    Task DeleteEntry(PortfolioEntry entry);
}