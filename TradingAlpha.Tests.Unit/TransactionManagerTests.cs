using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TradingAlpha.App.Data;
using TradingAlpha.App.Enums;
using TradingAlpha.App.Models;
using TradingAlpha.App.Models.TransactionTypes;
using TradingAlpha.App.Services;
using TradingAlpha.App.Services.Interfaces;
using TradingAlpha.Tests.Unit.TestUtils;

namespace TradingAlpha.Tests.Unit;

public class TransactionManagerTests
{
    private TransactionManager _sut;
    private IPortfolioManager _portfolioManager = Substitute.For<IPortfolioManager>();
    private IStockDataService _stockData = Substitute.For<IStockDataService>();
    private ICryptoDataService _cryptoData = Substitute.For<ICryptoDataService>();
    private IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    [Theory]
    [InlineData(1,2, "NVDA", "2023-11-12T12:12:12")]
    [InlineData(1,100, "AAPL", "2023-12-12T12:12:12")]
    [InlineData(1.244,2.2, "SPDR", "2024-01-01T00:00:00")]
    public async Task Buy_ShouldMakeTransaction_WhenBalanceIsSufficient(
        decimal amount, decimal price, string symbol, string dateString)
    {
        //Arrange 
        var options = await InMemoryDbCreator.OpenConnection();
        await using var dbContext = new ApplicationDbContext(options);

        var currentTime = DateTime.Parse(dateString);
        var user = new ApplicationUser
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "Test@mail.com",
            EmailConfirmed = true,
            Balance = 1000m,
            PortfolioId = 1
        };
        var port = new Portfolio
        {
            Id = 1,
            PortfolioEntries = new List<PortfolioEntry>(),
            User = user
        };
        var transaction = new StockTransaction
        {
            Amount = amount, AtPrice = price, Id = 1, Symbol = symbol,
            Timestamp = currentTime, User = user, TransactionBaseType = TransactionBaseType.BuyAction
        };
        _dateTimeProvider.Now.Returns(currentTime);
        _stockData.GetLatestBar(symbol, "EUR").Returns(price);
        _sut = new TransactionManager(dbContext, _portfolioManager, _stockData, _cryptoData, _dateTimeProvider);
        
        //Act
        if (await dbContext.Database.EnsureCreatedAsync())
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.Portfolios.AddAsync(port);
            await dbContext.SaveChangesAsync();
            await _sut.Buy(user, BaseDataType.Stock, amount, symbol);
        }
        
        //Assert
        user.Balance.Should().Be(1000 - amount * price);
        dbContext.StockTransactions.First().Should().BeEquivalentTo(transaction);
        //End connections
        InMemoryDbCreator.CloseConnection();
    }
}