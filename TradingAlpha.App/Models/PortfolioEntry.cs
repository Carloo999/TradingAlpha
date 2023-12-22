using System.ComponentModel.DataAnnotations.Schema;
using TradingAlpha.App.Enums;

namespace TradingAlpha.App.Models;

public class PortfolioEntry
{
    public int Id { get; set; }
    public DateTime AddedToPortfolio { get; set; }
    
    [ForeignKey("PortfolioId")]
    public int PortfolioId { get; set; } 
    public Portfolio Portfolio { get; set; }
}