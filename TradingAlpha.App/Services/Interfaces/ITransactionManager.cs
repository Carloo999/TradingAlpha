using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services.Interfaces;

public interface ITransactionManager
{
    Task Buy(ApplicationUser user, BaseDataType type, decimal amount, string symbol);
    Task Sell(ApplicationUser user, BaseDataType type, decimal amount, string symbol);
}