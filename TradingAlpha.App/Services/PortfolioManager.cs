using Microsoft.EntityFrameworkCore;
using TradingAlpha.App.Data;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.EntryTypes;
using TradingAlpha.App.Models.TransactionTypes;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class PortfolioManager(
    ApplicationDbContext db,
    ILoggerFactory loggerFactory) 
    : IPortfolioManager
{
    private ILogger<PortfolioManager> logger = loggerFactory.CreateLogger<PortfolioManager>();

    public async Task ChangeEntryForBuyTransaction(Transaction transaction)
    {
        if (transaction.GetType() == typeof(StockTransaction))
        {
            var stockTransaction = (StockTransaction)transaction;
            ApplicationUser user = stockTransaction.User;
            
            List<PortfolioEntry> portfolio = await GetUserPortfolioEntries(user);
            var stockEntriesWithSameSymbol = portfolio
                .Where(x => x.GetType() == typeof(StockEntry) 
                            && ((StockEntry)x).Symbol.Equals(stockTransaction.Symbol))
                .ToList(); 
            
            if (stockEntriesWithSameSymbol.Count == 0)
            {
                var stockEntry = new StockEntry
                {
                    Amount = stockTransaction.Amount,
                    CurrentPrice = stockTransaction.AtPrice,
                    LastUpdate = stockTransaction.Timestamp,
                    PortfolioId = stockTransaction.User.PortfolioId,
                    Symbol = stockTransaction.Symbol
                };
                
                stockEntry.PortfolioId = GetUserPortfolio(user).Id;
                //Add new portfolio entry
                await db.PortfolioEntries.AddAsync(stockEntry);
            }
            else if (stockEntriesWithSameSymbol.Count == 1)
            {
                var entry = stockEntriesWithSameSymbol.First();
                entry.CurrentPrice = stockTransaction.AtPrice;
                entry.LastUpdate = stockTransaction.Timestamp;
                entry.Amount += stockTransaction.Amount;
                db.Update(entry);
            }
            else
            {
                logger.LogCritical("Db failure! Multiple portfolio entries for same stock");
            }
        }
    }

    public async Task<Portfolio> GetUserPortfolio(ApplicationUser user)
    {
        return await db.Portfolios.FirstAsync(x => x.User.Equals(user));
    }

    public async Task<List<PortfolioEntry>> GetUserPortfolioEntries(ApplicationUser user)
    {
        var port = await GetUserPortfolio(user);
        return await db.PortfolioEntries.Where(x => x.PortfolioId == port.Id).ToListAsync();
    }

    public Task SaveEntryChanges()
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<PortfolioEntry>> GetEntries(ApplicationUser user)
    {
        var portfolio = await GetUserPortfolio(user);
        return portfolio.PortfolioEntries;
    }

    public Task DeleteEntry(PortfolioEntry entry)
    {
        throw new NotImplementedException();
    }
}