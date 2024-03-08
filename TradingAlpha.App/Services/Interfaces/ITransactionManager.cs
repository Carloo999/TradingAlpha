using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.TransactionTypes;

namespace TradingAlpha.App.Services.Interfaces;

public interface ITransactionManager
{
    Task Buy(ApplicationUser user, BaseDataType type, decimal amount, decimal price, string symbol);
    Task Sell(ApplicationUser user, BaseDataType type, decimal amount, decimal price, string symbol);

    StockTransaction[] GetStockTransactions(ApplicationUser user);
    CryptoTransaction[] GetCryptoTransactions(ApplicationUser user);
}