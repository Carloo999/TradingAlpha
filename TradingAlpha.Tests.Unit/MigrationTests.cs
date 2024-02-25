using Microsoft.EntityFrameworkCore;
using TradingAlpha.App.Data;

namespace TradingAlpha.Tests.Unit;

public class MigrationTests
{
    [Fact]
    public void ModelSnapshot_IsUpToDate_With_DbContext()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(@"DataSource=Data\\app.db;Cache=Shared")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            // Act
            var pendingMigrations = context.Database.GetPendingMigrations();
            
            // Assert
            Assert.Empty(pendingMigrations);
        }
    }
}