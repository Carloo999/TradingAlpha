using Newtonsoft.Json;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class StockDataService(IAlpacaService alpacaService) : IStockDataService
{
    public async Task<HistBars> GetHistBarData(
        string symbols,
        string timeframe,
        string start,
        string end,
        int limit,
        string adjustment,
        string feed,
        string sort)
    {
        var endpointPath = "v2/stocks/bars";
        var queryParams = new List<Tuple<string, string>>()
        {
            new("symbols", symbols),
            new("timeframe", timeframe),
            new("start", start),
            new("end", end),
            new("limit", limit.ToString()),
            new("adjustment", adjustment),
            new("feed", feed),
            new("sort", sort)
        };

        var result = await alpacaService.RequestDataAsync(endpointPath, queryParams);
        return ParseApiResponse(result);
    }
    
    private HistBars ParseApiResponse(string result)
    {
        var bars = JsonConvert.DeserializeObject<HistBars>(result);
        if (bars != null)
        {
            bars.Type = BaseDataType.Stock;
            return bars;
        }

        throw new InvalidOperationException("Invalid API response format");
    }
}