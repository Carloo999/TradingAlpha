namespace TradingAlpha.App.Services;

public class StockDataService(IAlpacaService alpacaService) : IStockDataService
{
    private readonly IAlpacaService _alpacaService = alpacaService;

    public async Task Test()
    {
        var s =await _alpacaService.RequestDataAsync("v2/stocks/bars?symbols=AAPL%2CTSLA&timeframe=1Min&start=2022-01-03T00%3A00%3A00Z&end=2022-01-04T00%3A00%3A00Z&limit=1000&adjustment=raw&feed=sip&sort=asc");
        Console.Write(s);
    } 
}