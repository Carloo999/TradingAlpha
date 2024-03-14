using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TradingAlpha.App.Data;

namespace TradingAlpha.Tests.Unit.TestUtils;

public static class InMemoryDbCreator
{
    private static SqliteConnection? _connection;
    
    public static async Task<DbContextOptions<ApplicationDbContext>> OpenConnection()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        await _connection.OpenAsync();
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;
        return options;
    }

    public static void CloseConnection()
    {
        _connection.Close();
    }
}