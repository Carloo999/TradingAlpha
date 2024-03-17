using System.ComponentModel.DataAnnotations;

namespace TradingAlpha.App.Enums;

public enum TransactionBaseType
{
    [Display(Name = "Buy")]
    BuyAction,
    [Display(Name = "Sell")]
    SellAction
}