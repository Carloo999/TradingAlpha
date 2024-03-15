using Microsoft.EntityFrameworkCore;
using TradingAlpha.App.Data;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.EntryTypes;
using TradingAlpha.App.Models.TransactionTypes;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class PortfolioManager(
    ApplicationDbContext db,
    ILoggerFactory loggerFactory,
    IStockDataService stockData,
    ICryptoDataService cryptoData) 
    : IPortfolioManager
{
    private readonly ILogger<PortfolioManager> _logger = loggerFactory.CreateLogger<PortfolioManager>();

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
                _logger.LogCritical("Db failure! Multiple portfolio entries for same stock");
            }
        }

        if (transaction.GetType() == typeof(CryptoTransaction))
        {
            var cryptoTransaction = (CryptoTransaction)transaction;
            ApplicationUser user = cryptoTransaction.User;
            
            List<PortfolioEntry> portfolio = await GetUserPortfolioEntries(user);
            var cryptoEntriesWithSameSymbol = portfolio
                .Where(x => x.GetType() == typeof(CryptoEntry) 
                            && ((CryptoEntry)x).Name.Equals(cryptoTransaction.Name))
                .ToList(); 
            
            if (cryptoEntriesWithSameSymbol.Count == 0)
            {
                var cryptoEntry = new CryptoEntry
                {
                    Amount = cryptoTransaction.Amount,
                    CurrentPrice = cryptoTransaction.AtPrice,
                    LastUpdate = cryptoTransaction.Timestamp,
                    PortfolioId = cryptoTransaction.User.PortfolioId,
                    Name = cryptoTransaction.Name
                };
                
                cryptoEntry.PortfolioId = GetUserPortfolio(user).Id;
                //Add new portfolio entry
                await db.PortfolioEntries.AddAsync(cryptoEntry);
            }
            else if (cryptoEntriesWithSameSymbol.Count == 1)
            {
                var entry = cryptoEntriesWithSameSymbol.First();
                entry.CurrentPrice = cryptoTransaction.AtPrice;
                entry.LastUpdate = cryptoTransaction.Timestamp;
                entry.Amount += cryptoTransaction.Amount;
                db.Update(entry);
            }
            else
            {
                _logger.LogCritical("Db failure! Multiple portfolio entries for same crypto");
            }
        }
    }

    public void ChangeEntryForSellTransaction(Transaction transaction, PortfolioEntry entry)
    {
        if (entry.Amount - transaction.Amount == 0)
        {
            db.Remove(entry);
        }
        else if (entry.Amount - transaction.Amount > 0)
        {
            entry.Amount -= transaction.Amount;
            db.Update(entry);
        }
        else
        {
            throw new Exception("Entry amount is negative!");
        }
    }

    public async Task<Portfolio> GetUserPortfolio(ApplicationUser user)
    {
        return await db.Portfolios.FirstAsync(x => x.User.Equals(user));
    }

    public async Task<StockEntry[]> GetStockEntries(ApplicationUser user)
    {
        var entries = await GetUserPortfolioEntries(user);
        return entries.Where(e => e is StockEntry).Cast<StockEntry>().ToArray();
    }

    public async Task<CryptoEntry[]> GetCryptoEntries(ApplicationUser user)
    {
        var entries = await GetUserPortfolioEntries(user);
        return entries.Where(e => e is CryptoEntry).Cast<CryptoEntry>().ToArray();
    }

    public async Task UpdateAllPricesInPort(ApplicationUser user)
    {
        var stockEntries = await GetStockEntries(user);

        foreach (var stockEntry in stockEntries)
        {
            stockEntry.CurrentPrice = await stockData.GetLatestBar(stockEntry.Symbol, "EUR");
            stockEntry.LastUpdate = DateTime.Now;
        }
        
        var cryptoEntries =  await GetCryptoEntries(user);

        foreach (var cryptoEntry in cryptoEntries)
        {
            cryptoEntry.CurrentPrice = await cryptoData.GetLatestBar(cryptoEntry.Name);
            cryptoEntry.LastUpdate = DateTime.Now;
        }

        await db.SaveChangesAsync();
    }

    public async Task<List<PortfolioEntry>> GetUserPortfolioEntries(ApplicationUser user)
    {
        var port = await GetUserPortfolio(user);
        return await db.PortfolioEntries.Where(x => x.PortfolioId == port.Id).ToListAsync();
    }

    public async Task<PortfolioEntry[]> GetEntries(ApplicationUser user)
    {
        var portfolio = await GetUserPortfolio(user);
        return await db.PortfolioEntries.Where(x => x.PortfolioId == portfolio.Id).ToArrayAsync();
    }
}