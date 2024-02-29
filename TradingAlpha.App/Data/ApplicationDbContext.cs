using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.EntryTypes;
using TradingAlpha.App.Models.TransactionTypes;

namespace TradingAlpha.App.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<PortfolioEntry> PortfolioEntries { get; set; }
    public DbSet<StockEntry> StockEntries { get; set; }
    public DbSet<CryptoEntry> CryptoEntries { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }
    public DbSet<CryptoTransaction> CryptoTransactions { get; set; }
}