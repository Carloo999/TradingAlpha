using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Models;

public class LatestBar
{
    public BaseDataType Type { get; set; }
    public Dictionary<string, HistBarsEntry> Bars { get; set; }
}