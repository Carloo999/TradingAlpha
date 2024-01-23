using System.Text.Json;
using RestSharp;
using TradingAlpha.App.Services.Interfaces;

namespace TradingAlpha.App.Services;

public class AlpacaService : IAlpacaService
{
   private string _key = string.Empty;
   private string _secret = string.Empty;

   public async Task<string> RequestDataAsync(string endpointPath, List<Tuple<string,string>> queryParams, bool authNeeded = true)
   {
      if (_key.Equals(string.Empty) || _secret.Equals(string.Empty)) 
         await Init("ApiSettings.json");
      
      var options = new RestClientOptions("https://data.alpaca.markets/" + endpointPath);
      var client = new RestClient(options);
      var request = new RestRequest("");
      request.AddHeader("accept", "application/json");
      
      if (authNeeded)
      {
         request.AddHeader("APCA-API-KEY-ID", _key);
         request.AddHeader("APCA-API-SECRET-KEY", _secret);
      }

      foreach (var (name, value) in queryParams)
      {
         request.AddQueryParameter(name, value);
      }
      
      RestResponse response = await client.GetAsync(request);
      
      return (response.IsSuccessful ? response.Content : throw new InvalidOperationException())!;
   }
   
   private async Task Init(string jsonFilePath)
   {
      using var streamReader = new StreamReader(jsonFilePath);
      var jsonString = await streamReader.ReadToEndAsync();
            
      using JsonDocument document = JsonDocument.Parse(jsonString);
      var apiSettings = document.RootElement.GetProperty("apisettings");

      _key = apiSettings.GetProperty("AlpacaKey").ToString();
      _secret = apiSettings.GetProperty("AlpacaSecret").ToString();
   }
}
