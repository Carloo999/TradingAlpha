namespace TradingAlpha.App.Services;

public interface IAlpacaService
{
    Task<string> RequestDataAsync(string additionToBaseUrl, bool authNeeded = true);
}