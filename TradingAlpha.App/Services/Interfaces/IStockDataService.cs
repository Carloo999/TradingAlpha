using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services.Interfaces;

public interface IStockDataService
{
    public Task<HistBars> GetHistBarData(
        string symbols,
        string timeframe,
        string start,
        string end,
        int limit,
        string adjustment,
        string feed,
        string sort);
}