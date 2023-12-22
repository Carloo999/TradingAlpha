namespace TradingAlpha.App.Models.EntryTypes;

public class CryptoEntry : PortfolioEntry
{
    public string Name { get; set; }
    public decimal PriceBoughtAt { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime LastUpdate { get; set; }
    public decimal Amount { get; set; }
}