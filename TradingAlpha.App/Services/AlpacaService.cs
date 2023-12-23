using System.Text.Json;
using RestSharp;

namespace TradingAlpha.App.Services;

public class AlpacaService : IAlpacaService
{
   private string _key = string.Empty;
   private string _secret = string.Empty;

   public async Task<string> RequestDataAsync(string additionToBaseUrl, bool authNeeded = true)
   {
      if (_key.Equals(string.Empty) || _secret.Equals(string.Empty)) 
         await Init("ApiSettings.json");
      
      var options = new RestClientOptions("https://data.alpaca.markets/" + additionToBaseUrl);
      var client = new RestClient(options);
      var request = new RestRequest("");
      request.AddHeader("accept", "application/json");
      
      if (authNeeded)
      {
         request.AddHeader("APCA-API-KEY-ID", _key);
         request.AddHeader("APCA-API-SECRET-KEY", _secret);
      }
      RestResponse response = await client.GetAsync(request);
      
      return (response.IsSuccessful ? response.Content : throw new InvalidOperationException())!;
   }

   private async Task temp()
   {
      var options = new RestClientOptions("https://data.alpaca.markets/v1beta3/crypto/us/bars?symbols=BTC%2FUSD&timeframe=1D&start=2023-12-01T00%3A00%3A00Z&end=2023-12-23T00%3A00%3A00Z&limit=1000&sort=asc");
      var client = new RestClient(options);
      var request = new RestRequest("");
      request.AddHeader("accept", "application/json");
      var response = await client.GetAsync(request);

      Console.WriteLine("{0}", response.Content);

   }
   
   private async Task Init(string jsonFilePath)
   {
      using var streamReader = new StreamReader(jsonFilePath);
      var jsonString = await streamReader.ReadToEndAsync();
            
      using JsonDocument document = JsonDocument.Parse(jsonString);
      var apiSettings = document.RootElement.GetProperty("ApiSettings");

      _key = apiSettings.GetProperty("AlpacaKey").ToString();
      _secret = apiSettings.GetProperty("AlpacaSecret").ToString();
   }
}
