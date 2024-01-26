using System.ComponentModel.DataAnnotations.Schema;
using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Models;

public class PortfolioEntry
{
    public decimal CurrentPrice { get; set; }
    public DateTime LastUpdate { get; set; }
    public decimal Amount { get; set; }
    
    public int Id { get; set; }
    [ForeignKey("PortfolioId")]
    public int PortfolioId { get; set; } 
    public Portfolio Portfolio { get; set; }
}