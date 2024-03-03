using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}