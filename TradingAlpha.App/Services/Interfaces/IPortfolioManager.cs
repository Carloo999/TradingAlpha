using TradingAlpha.App.Data;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.EntryTypes;
using TradingAlpha.App.Models.TransactionTypes;

namespace TradingAlpha.App.Services.Interfaces;

public interface IPortfolioManager
{
    Task ChangeEntryForBuyTransaction(Transaction transaction);
    
    void ChangeEntryForSellTransaction(Transaction transaction, PortfolioEntry entry);
    
    Task<PortfolioEntry[]> GetEntries(ApplicationUser user);

    // Uncasted
    Task<Portfolio> GetUserPortfolio(ApplicationUser user);
    
    // Already casted!!
    Task<StockEntry[]> GetStockEntries(ApplicationUser user);
    Task<CryptoEntry[]> GetCryptoEntries(ApplicationUser user);

    Task UpdateAllPricesInPort(ApplicationUser user);
}