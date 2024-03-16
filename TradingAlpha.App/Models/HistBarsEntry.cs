using Newtonsoft.Json;

namespace TradingAlpha.App.Models;

public class HistBarsEntry
{
   [JsonProperty(PropertyName = "c")]
   public decimal Close { get; set; }

   [JsonProperty(PropertyName = "h")]
   public decimal High { get; set; }
   
   [JsonProperty(PropertyName = "l")]
   public decimal Low { get; set; }
   
   [JsonProperty(PropertyName = "n")]
   public decimal NumOfTrades { get; set; }
   
   [JsonProperty(PropertyName = "o")]
   public decimal Open { get; set; }
   
   
   [JsonProperty(PropertyName = "t")]
   public DateTime Time { get; set; }

   [JsonProperty(PropertyName = "v")]
   public decimal Volume { get; set; }
   
   [JsonProperty(PropertyName = "vw")]
   public decimal VolumeWeighted { get; set; }
   
   public decimal SMA { get; set; }
}