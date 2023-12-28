using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Models;

public class HistBars
{
    public BaseDataType Type { get; set; }
    public Dictionary<string, List<HistBarsEntry>> Bars { get; set; }
}