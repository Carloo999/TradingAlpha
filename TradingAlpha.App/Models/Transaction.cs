using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Models;

public abstract class Transaction
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Amount { get; set; }
    public ApplicationUser User { get; set; }
    public decimal AtPrice { get; set; }
    public TransactionBaseType TransactionBaseType { get; set; }
}