using TradingAlpha.App.Models;

namespace TradingAlpha.App.Services.Interfaces;

public interface ICryptoDataService
{
    public Task<HistBars> GetHistBarData(
        string symbols,
        string timeframe,
        string start,
        string end,
        int limit,
        string sort);
    
    public Task<decimal> GetLatestBar(string symbols);
}