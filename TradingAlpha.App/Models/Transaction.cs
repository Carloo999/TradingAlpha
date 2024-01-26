using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Models;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public BaseDataType Type { get; set; }
    public decimal Amount { get; set; }
    public ApplicationUser User { get; set; }
}