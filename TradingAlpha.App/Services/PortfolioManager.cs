using TradingAlpha.App.Data;
using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services;

public class PortfolioManager : IPortfolioManager
{
    public Task AddNewEntry(PortfolioEntry entry, ApplicationUser user)
    {
        throw new NotImplementedException();
    }

    public Task SaveEntryChanges()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<PortfolioEntry>> GetEntries(ApplicationUser user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEntry(PortfolioEntry entry)
    {
        throw new NotImplementedException();
    }
}