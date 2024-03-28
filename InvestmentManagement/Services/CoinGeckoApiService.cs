using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Net;
using InvestmentManagement.Models;

namespace InvestmentManagement.Services
{
  class CoinGeckoApiService : BaseRestApiService
  {

    const string baseUrl = "https://api.coingecko.com/api/v3/";

    List<CoinGeckoRestResponseModel.CoinsList> coinsList = new List<CoinGeckoRestResponseModel.CoinsList>();
    List<CoinGeckoRestResponseModel.CoinsMarkets> coinsMarkets = new List<CoinGeckoRestResponseModel.CoinsMarkets>();

    public class CoinsListPrice
    {
      public string Id { get; set; }
      public string Symbol { get; set; }
      public string Name { get; set; }
      public decimal CurrentPrice { get; set; }
      public string Contractaddress { get; set; }
    }

    private string GetRequest(string _url)
    {
      
      HttpWebRequest req = WebRequest.Create(_url) as HttpWebRequest;
      string result = null;
      try
      {
        using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
        {
          StreamReader reader = new StreamReader(resp.GetResponseStream());
          result = reader.ReadToEnd();
        }
      }
      catch (WebException e)
      {

        return null;
      }



      return result;
    }


    public string GetCurrentPrice(string _url)
    {
      string result = null;
      result = GetRequest(_url);

      if (!(result is null))
      {
        result = result.Substring(result.IndexOf("eur\":") + 5);
        result = result.Substring(0, result.Length - 2);
        result = result.Replace(",", "");
        result = result.Replace(".", ",");
        Console.WriteLine("Price: " + result);
      }
      else
      {
        // If the result is null wait 2 min and then try it again
        Console.WriteLine("Wait 2 min to continue Timestamp: " + DateTime.Now.ToString());
        System.Threading.Thread.Sleep(120000); //120000 ms
        result = GetCurrentPrice(_url);
      }

      return result;
    }


    public async Task<List<CoinsListPrice>> GetCurrentPriceAsync(List<CoinsListPrice> coinsListPrice,  string currency)
    {
      coinsList = await GetCoinsListAsync();
      //coinsMarkets = await GetCoinsMarketsAsync(currency);

      //Task[] tasks = { GetCoinsListAsync(),
      //                 GetCoinsMarketsAsync(currency)};

      //await Task.WhenAll(tasks);

      coinsListPrice = await InsertCurrPriceToList(coinsListPrice, currency);


      return coinsListPrice;
    }

    async Task<List<CoinGeckoRestResponseModel.CoinsList>> GetCoinsListAsync()
    {
      List<CoinGeckoRestResponseModel.CoinsList> coinsList = new List<CoinGeckoRestResponseModel.CoinsList>();
      string url = baseUrl + "coins/list?include_platform=true";


      coinsList = await GetApiResultsAsListAsync<CoinGeckoRestResponseModel.CoinsList>(url);
      return coinsList;
    }
    async Task<List<CoinGeckoRestResponseModel.CoinsMarkets>> GetCoinsMarketsAsync(string currency)
    {
      List<CoinGeckoRestResponseModel.CoinsMarkets> coinsMarkets = new List<CoinGeckoRestResponseModel.CoinsMarkets>();
      int maxCoinPerPage = 250;
      int amountPages = coinsList.Count / maxCoinPerPage;
     
      for (int i = 1; i <= amountPages; i++)
      {
        Console.WriteLine("Current Coin Markets page:"+i +" of " + amountPages);
        string url = baseUrl + $"coins/markets?vs_currency={currency}&order=market_cap_desc&per_page=250&page={i}&sparkline=false&locale=de";
        coinsMarkets.AddRange(await GetApiResultsAsListAsync<CoinGeckoRestResponseModel.CoinsMarkets>(url));
      }
      //return await GetApiResultsAsync<CoinGeckoRestResponseModel.CoinsMarkets>(url);
      return coinsMarkets;
      //return await GetApiResultsRestSharperAsync<CoinGeckoRestResponseModel.CoinsMarkets>(url);
    }
    async Task<Dictionary<string, Dictionary<string, decimal>>> GetCoinsSimplePriceAsync(string coinIds, string currency)
    {
      string coinIdsAsOneString = coinIds.ToString();// "";
      
      foreach (var coinId in coinIds)
      {
        coinIdsAsOneString += coinId;
      }

      string url = baseUrl + $"simple/price?ids={coinIds}&vs_currencies={currency}";

      return await GetApiResultsDicAsync< decimal>(url);
    }

    async Task<List<CoinsListPrice>> InsertCurrPriceToList(List<CoinsListPrice> coinsListPrice, string currency)
    {
      string coinIdsAsOneString = "";
      for (int i = 0; i < coinsListPrice.Count; i++)
      {
        if (coinsListPrice[i].Id is null)
        {
          coinsListPrice[i].Id = "";
        }
        if (coinsListPrice[i].Id.Length == 0)
        {
          if (i == 122)
          {
            Console.WriteLine("Test");
          }
          //coinsListPrice[i].Id = GetCoinIdFromCoinsList(coinsListPrice[i].Name, coinsListPrice[i].Symbol);
          coinsListPrice[i].Id = GetCoinIdFromCoinsList(coinsListPrice[i]);

          if (coinsListPrice[i].Id.Length >0)
          {
            if (coinIdsAsOneString.Length == 0)
            {
              coinIdsAsOneString = coinsListPrice[i].Id;
            }
            else
            {
              coinIdsAsOneString += "," + coinsListPrice[i].Id;
            }
          }
          
        }

        //if (coinsListPrice[i].Id.Length == 0)
        //{
        //  coinsListPrice[i].CurrentPrice = GetCoinCurrPriceFromCoinsMarkets(coinsListPrice[i].Id);
        //}
      }

      Dictionary<string, Dictionary<string, decimal>> coinsSimplePrices = await GetCoinsSimplePriceAsync(coinIdsAsOneString, currency);

      for (int i = 0; i < coinsListPrice.Count; i++)
      {
        
        if (coinsListPrice[i].Id.Length > 0)
        {
          coinsListPrice[i].CurrentPrice = coinsSimplePrices[coinsListPrice[i].Id][currency];
        }
      }

      return coinsListPrice;
    }
    public void Test()
    {
      if (CompareStrings("Amino", "Amino") && CompareStrings("$amo", "amo"))
      {
        Console.WriteLine("Test");
      }
    }
    string GetCoinIdFromCoinsList(CoinsListPrice coinsListPrice)
    {
      string id = "";

      if (coinsListPrice.Contractaddress.Length > 0)
      {
        foreach (var coin in coinsList)
        {
          if (!(coin.Platforms is null))
          {
            if (coin.Platforms.Count > 0)
            {
              foreach (var platform in coin.Platforms)
              {
                if (!(platform.Value is null))
                {
                  if (platform.Value.Equals(coinsListPrice.Contractaddress))
                  {
                    return coin.Id;
                  }
                }
                
              }
            }
          }

        }
      }
      else
      {
        foreach (var coin in coinsList)
        {
          if (CompareNameAndSymbol(coinsListPrice.Name, coin.Name, coinsListPrice.Symbol, coin.Symbol))
          {
            return coin.Id;
          }


        }
      }

      

      return id;
    }
    string GetCoinIdFromCoinsList(string name, string symbol)
    {
      string id = "";

      foreach (var coin in coinsList)
      {
        if (CompareNameAndSymbol(name, coin.Name, symbol, coin.Symbol))//CompareStrings(name, coin.Name) && CompareStrings(symbol, coin.Symbol))
        {
          id = coin.Id;
        }
      }

      return id;
    }

    static bool CompareStrings(string str1, string str2)
    {
      int maxLength = Math.Max(str1.Length, str2.Length);
      int distance = CalculateLevenshteinDistance(str1, str2);

      double similarityPercentage = ((double)(maxLength - distance) / maxLength) * 100;

      return similarityPercentage >= 75;
    }

    static bool CompareNameAndSymbol(string name, string nameFromCoingecko, string symbol, string symbolFromCoingecko)
    {
      bool isSimilarEnough = false;

      if (CompareSymbol(symbol,symbolFromCoingecko))
      {
        isSimilarEnough = true;

        if (!(CompareStrings(name.ToLower(), nameFromCoingecko.ToLower())))
        {
          isSimilarEnough = false;
        }
      }

      return isSimilarEnough;// (GetCompareStringsResult(name1.ToLower(), name2.ToLower()) + GetCompareStringsResult(symbol1.ToLower(), symbol2.ToLower())) >= (80*2);
    }
    static bool CompareSymbol(string symbol1, string symbolFromCoingecko)
    {
      if (symbol1.ToLower().Equals(symbolFromCoingecko.ToLower()))
      {
        return true;
      }
      else if (("$"+symbol1.ToLower()).Equals(symbolFromCoingecko.ToLower()))
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    static double GetCompareStringsResult(string str1, string str2)
    {
      int maxLength = Math.Max(str1.Length, str2.Length);
      int distance = CalculateLevenshteinDistance(str1, str2);

      double similarityPercentage = ((double)(maxLength - distance) / maxLength) * 100;

      return similarityPercentage ;
    }

    static int CalculateLevenshteinDistance(string str1, string str2)
    {
      int[,] matrix = new int[str1.Length + 1, str2.Length + 1];

      for (int i = 0; i <= str1.Length; i++)
      {
        matrix[i, 0] = i;
      }

      for (int j = 0; j <= str2.Length; j++)
      {
        matrix[0, j] = j;
      }

      for (int i = 1; i <= str1.Length; i++)
      {
        for (int j = 1; j <= str2.Length; j++)
        {
          int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;

          matrix[i, j] = Math.Min(
              Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
              matrix[i - 1, j - 1] + cost
          );
        }
      }

      return matrix[str1.Length, str2.Length];
    }


    decimal GetCoinCurrPriceFromCoinsMarkets(string id)
    {
      decimal currPrice = 0;

      foreach (var coin in coinsMarkets)
      {
        if (coin.Equals(id))
        {
          if (!(coin.Current_price is null))
          {
            currPrice = (decimal)coin.Current_price;
          }
          
        }
      }

      return currPrice;
    }

  }

}
