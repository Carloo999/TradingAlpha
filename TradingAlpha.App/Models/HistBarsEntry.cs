using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Models;

public class HistBarsEntry
{
    public BaseDataType Type;
    public string Symbol;
    public DateTime Time;
    public decimal Close;
    public decimal High;
    public decimal Low;
    public decimal NOfTrades;
    public decimal Open;
    public decimal Volume;
    public decimal VolumeWeighted;

}