using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.EntryTypes;
using TradingAlpha.App.Models.TransactionTypes;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class TransactionManager
    (ApplicationDbContext db, 
        IPortfolioManager portfolioManager,
        IStockDataService stockData, 
        ICryptoDataService cryptoData,
        IDateTimeProvider dateTimeProvider): ITransactionManager
{
    public async Task Buy(ApplicationUser user, BaseDataType type, decimal amount, string symbol)
    {
        if (type == BaseDataType.Stock)
        {
            decimal currentPrice = await CheckForBuyValidity(user, BaseDataType.Stock, amount, symbol);
            
            var transaction = new StockTransaction
            {
                Timestamp = dateTimeProvider.Now,
                Symbol = symbol,
                Amount = amount,
                AtPrice = currentPrice,
                User = user,
                TransactionBaseType = TransactionBaseType.BuyAction
            };
            await portfolioManager.ChangeEntryForBuyTransaction(transaction);
            await db.StockTransactions.AddAsync(transaction);
            user.Balance -= transaction.Amount * transaction.AtPrice;
            db.Update(user);
            await db.SaveChangesAsync();
        }

        if (type == BaseDataType.Crypto)
        {
            decimal currentPrice = await CheckForBuyValidity(user, BaseDataType.Crypto, amount, symbol);
            
            var transaction = new CryptoTransaction()
            {
                Timestamp = dateTimeProvider.Now,
                Name = symbol,
                Amount = amount,
                AtPrice = currentPrice,
                User = user,
                TransactionBaseType = TransactionBaseType.BuyAction
            };
            await portfolioManager.ChangeEntryForBuyTransaction(transaction);
            await db.CryptoTransactions.AddAsync(transaction);
            user.Balance -= transaction.Amount * transaction.AtPrice;
            db.Update(user);
            await db.SaveChangesAsync();
        }
    }

    public async Task Sell(ApplicationUser user, BaseDataType type, decimal amount, string symbol)
    {
        if (type == BaseDataType.Stock)
        {
            var (currentPrice, portfolioEntry) = await CheckForSellValidity(user, BaseDataType.Stock, amount, symbol);
            
            var transaction = new StockTransaction
            {
                Timestamp = dateTimeProvider.Now,
                Symbol = symbol,
                Amount = amount,
                AtPrice = currentPrice,
                User = user,
                TransactionBaseType = TransactionBaseType.SellAction
            };
            
            portfolioManager.ChangeEntryForSellTransaction(transaction, portfolioEntry);
            await db.StockTransactions.AddAsync(transaction);
            user.Balance += transaction.Amount * transaction.AtPrice;
            db.Update(user);
            await db.SaveChangesAsync();
        }

        if (type == BaseDataType.Crypto)
        {
            var (currentPrice, portfolioEntry) = await CheckForSellValidity(user, BaseDataType.Crypto, amount, symbol);

            var transaction = new CryptoTransaction
            {
                Timestamp = dateTimeProvider.Now,
                Name = symbol,
                Amount = amount,
                AtPrice = currentPrice,
                User = user,
                TransactionBaseType = TransactionBaseType.SellAction
            };
            
            portfolioManager.ChangeEntryForSellTransaction(transaction, portfolioEntry);
            await db.CryptoTransactions.AddAsync(transaction);
            user.Balance += transaction.Amount * transaction.AtPrice;
            db.Update(user);
            await db.SaveChangesAsync();
        }
    }
    
    private async Task<decimal> CheckForBuyValidity(ApplicationUser user, BaseDataType type, decimal amount, string symbol)
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
    
    private async Task<Tuple<decimal, PortfolioEntry>> CheckForSellValidity(ApplicationUser user, BaseDataType type, decimal amount, string symbol)
    {
        var portfolio = await portfolioManager.GetEntries(user);
        decimal currentPrice;
        
        if (type == BaseDataType.Stock)
        {
            currentPrice = await stockData.GetLatestBar(symbol, "EUR");
            var stockEntriesWithSameSymbol = portfolio
                .Where(x => x.GetType() == typeof(StockEntry) 
                            && ((StockEntry)x).Symbol.Equals(symbol))
                .ToList();
            
            if (stockEntriesWithSameSymbol.Count == 0)
            {
                throw new Exception("No crypto in the portfolio found with matching name!");
            }
            if (stockEntriesWithSameSymbol.Count == 1)
            {
                return stockEntriesWithSameSymbol.First().Amount >= amount
                    ? new Tuple<decimal, PortfolioEntry>(currentPrice, stockEntriesWithSameSymbol.First())
                    : throw new Exception("Amount in portfolio too low");
            }
            else
            {
                throw new Exception("Multiple crypto entries for same name!");
            }
        }
        else if (type == BaseDataType.Crypto)
        {
            currentPrice = await cryptoData.GetLatestBar(symbol);
            var cryptoEntriesWithSameSymbol = portfolio
                .Where(x => x.GetType() == typeof(CryptoEntry) 
                            && ((CryptoEntry)x).Name.Equals(symbol))
                .ToList();

            if (cryptoEntriesWithSameSymbol.Count == 0)
            {
                throw new Exception("No crypto in the portfolio found with matching name!");
            }
            if (cryptoEntriesWithSameSymbol.Count == 1)
            {
                return cryptoEntriesWithSameSymbol.First().Amount >= amount
                    ? new Tuple<decimal, PortfolioEntry>(currentPrice, cryptoEntriesWithSameSymbol.First())
                    : throw new Exception("Amount in portfolio too low");
            }
            else
            {
                throw new Exception("Multiple crypto entries for same name!");
            }
        }
        throw new Exception("Invalid symbol, balance check failed!");
    }
}