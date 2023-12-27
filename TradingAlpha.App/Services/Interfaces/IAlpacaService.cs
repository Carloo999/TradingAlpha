namespace TradingAlpha.App.Services.Interfaces;

public interface IAlpacaService
{
    Task<string> RequestDataAsync(string endpointPath, List<Tuple<string,string>> queryParams, bool authNeeded = true);
}