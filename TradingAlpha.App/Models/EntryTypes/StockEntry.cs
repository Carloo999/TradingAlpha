namespace TradingAlpha.App.Models.EntryTypes;

public class StockEntry : PortfolioEntry
{
    //Not able to retrieve actual stock name so remove and update db
    public string Name { get; set; }
    public string Symbol { get; set; }
    public decimal PriceBoughtAt { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime LastUpdate { get; set; }
    public decimal Amount { get; set; }
}