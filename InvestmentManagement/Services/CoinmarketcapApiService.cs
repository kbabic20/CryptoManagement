using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestmentManagement.Models;

namespace InvestmentManagement.Services
{
  public class CoinmarketcapApiService : BaseRestApiService
  {

    const string baseUrl = "https://pro-api.coinmarketcap.com/v1/";
    Dictionary<string, string> header = new Dictionary<string, string> { { "X-CMC_PRO_API_KEY", "7b9c151e-fd1c-4420-8496-a03ddd44fd69" } };

    CoinmarketcapRestResponseModel.CryptoMap cryptoMaps = new CoinmarketcapRestResponseModel.CryptoMap();
    List<CoinmarketcapRestResponseModel.Data> cryptoMapsData = new List<CoinmarketcapRestResponseModel.Data>();
    public async Task Test()
    {
      string url = " https://pro-api.coinmarketcap.com/v1/cryptocurrency/map?start=1&limit=5000";
      CoinmarketcapRestResponseModel.CryptoMap cryptoMaps = await GetApiResultsAsync<CoinmarketcapRestResponseModel.CryptoMap>(url,header);
      List<CoinmarketcapRestResponseModel.Data> cryptoMapsData = new List<CoinmarketcapRestResponseModel.Data>();
      CoinmarketcapRestResponseModel.CryptoMap cryptoMapsOnlyEth = new CoinmarketcapRestResponseModel.CryptoMap();

      foreach (var cryptoMap in cryptoMaps.Data)
      {
        if (cryptoMap.symbol.ToLower().Equals("avax"))
        {
          cryptoMapsData.Add(cryptoMap);
        }
      }

      cryptoMapsOnlyEth.Data = cryptoMapsData.ToArray();

    }

    public async Task<string> GetNetworkNameBaseOnSymbol(string symbol)
    {
      if (cryptoMaps.Data is null )
      {
        cryptoMaps = await GetCoinmarketcapRestResponseModelCryptoMap();

      }

      cryptoMapsData.Clear();

      foreach (var cryptoMap in cryptoMaps.Data)
      {
        if (cryptoMap.symbol.ToLower().Equals(symbol.ToLower()))
        {
          cryptoMapsData.Add(cryptoMap);
        }
      }

      CoinmarketcapRestResponseModel.CryptoMap cryptoMapsOnlyEth = new CoinmarketcapRestResponseModel.CryptoMap();

      
      cryptoMapsOnlyEth.Data = cryptoMapsData.ToArray();

      if (cryptoMapsOnlyEth.Data.Length == 1)
      {
        return cryptoMapsOnlyEth.Data[0].name;
      }
      else if (cryptoMapsOnlyEth.Data.Length == 0)
      {
        return "not found";
      }
      else 
      {
        return "more then one result";
      }

    }

    async Task<CoinmarketcapRestResponseModel.CryptoMap> GetCoinmarketcapRestResponseModelCryptoMap()
    {
      string url = baseUrl + "cryptocurrency/map?start=1&limit=5000";

      return await GetApiResultsAsync<CoinmarketcapRestResponseModel.CryptoMap>(url, header);
    }

  }
}
