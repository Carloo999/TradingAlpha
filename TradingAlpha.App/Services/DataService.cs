using Newtonsoft.Json;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class DataService(IAlpacaService alpacaService) : IStockDataService, ICryptoDataService
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
        
        var queryParams = CreateQueryParams(symbols, timeframe, start, end, limit, adjustment, feed, sort);
        var result = await alpacaService.RequestDataAsync(endpointPath, queryParams);
        return ParseApiResponse(result, BaseDataType.Stock);
    }

    public async Task<HistBars> GetHistBarData(
        string symbols,
        string timeframe,
        string start,
        string end,
        int limit,
        string sort)
    {
        var endpointPath = "v1beta3/crypto/us/bars";

        var queryParams = CreateQueryParams(symbols, timeframe, start, end, limit, sort);
        var result = await alpacaService.RequestDataAsync(endpointPath, queryParams, false);
        return ParseApiResponse(result, BaseDataType.Crypto);
    }

    private HistBars ParseApiResponse(string result, BaseDataType type)
    {
        var bars = JsonConvert.DeserializeObject<HistBars>(result);
        if (bars != null)
        {
            bars.Type = type;
            return bars;
        }

        throw new InvalidOperationException("Invalid API response format");
    }

    private List<Tuple<string, string>> CreateQueryParams(
        string symbols,
        string timeframe,
        string start,
        string end,
        int limit,
        string sort)
    {
        return new List<Tuple<string, string>>
        {
            new("symbols", symbols),
            new("timeframe", timeframe),
            new("start", start),
            new("end", end),
            new("limit", limit.ToString()),
            new("sort", sort)
        };
    }
    
    private List<Tuple<string, string>> CreateQueryParams(
        string symbols,
        string timeframe,
        string start,
        string end,
        int limit,
        string adjustment,
        string feed,
        string sort)
    {
        var queryParams = CreateQueryParams(symbols, timeframe, start, end, limit, sort);
        queryParams.AddRange(new[]
        {
            new Tuple<string, string>("adjustment", adjustment),
            new Tuple<string, string>("feed", feed)
        });
        return queryParams;
    }
}