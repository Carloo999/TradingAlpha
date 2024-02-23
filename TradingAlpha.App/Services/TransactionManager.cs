using Microsoft.AspNetCore.Identity;
using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.TransactionTypes;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class TransactionManager
    (ApplicationDbContext db, 
        IPortfolioManager portfolioManager,
        IStockDataService stockData, 
        ICryptoDataService cryptoData,
        UserManager<ApplicationUser> testREMOVE): ITransactionManager
{
    public async Task Buy(ApplicationUser user, BaseDataType type, decimal amount, decimal price, string symbol)
    {
        if (type == BaseDataType.Stock)
        {
            decimal currentPrice = await CheckForValidity(user, BaseDataType.Stock, amount, symbol);
            
            var transaction = new StockTransaction
            {
                Timestamp = DateTime.Now,
                Symbol = symbol,
                Amount = amount,
                AtPrice = currentPrice,
                User = user
            };
            await portfolioManager.ChangeEntryForBuyTransaction(transaction);
            await db.StockTransactions.AddAsync(transaction);
            user.Balance -= transaction.Amount * transaction.AtPrice;
            db.Update(user);
            await db.SaveChangesAsync();
        }
    }

    public async Task Sell(ApplicationUser user, BaseDataType type, decimal amount, decimal price, string symbol)
    {
        throw new NotImplementedException();
    }
    
    private async Task<decimal> CheckForValidity(ApplicationUser user, BaseDataType type, decimal amount, string symbol)
    {
        decimal currentPrice = type switch
        {
            BaseDataType.Stock => await stockData.GetLatestBar(symbol, "EUR"),
            BaseDataType.Crypto => await cryptoData.GetLatestBar(symbol),
            _ => throw new Exception("Invalid symbol, balance check failed!")
        };
        return user.Balance - amount * currentPrice > 0 ? 
            currentPrice : throw new Exception("Balance too low to process transaction");
    }
}