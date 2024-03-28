using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestmentManagement.Models;

namespace InvestmentManagement.Services
{
  public class DexscreenerApiService : BaseRestApiService
  {

    const string baseUrl = "https://api.dexscreener.com/latest/";

    DexscreenerRestResponseModel.PairsList cryptoMaps = new DexscreenerRestResponseModel.PairsList();

    public async Task<Dictionary<string,decimal>> GetUsdPricePerPair(string chainName, List<string> pairs)
    {
      Dictionary<string, decimal> usdPricePerPair = new Dictionary<string, decimal>();

      cryptoMaps = await GetCoinmarketcapRestResponseModelCryptoMap(chainName, pairs);

      foreach (var pair in pairs)
      {
        usdPricePerPair.Add(pair,ExtractUsdPrice(pair));
      }

      return usdPricePerPair;
    }

    decimal ExtractUsdPrice(string pair)
    {
      for (int i = 0; i < cryptoMaps.pairs.Length; i++)
      {
        if (cryptoMaps.pairs[i].pairAddress.Equals(pair))
        {
          return decimal.Parse(cryptoMaps.pairs[i].priceUsd, CultureInfo.InvariantCulture);
        }
      }

      return 0;
    }

    async Task<DexscreenerRestResponseModel.PairsList> GetCoinmarketcapRestResponseModelCryptoMap(string chainName, List<string> pairs )
    {
      string getPairsUrl = baseUrl + $"dex/pairs/{chainName}/";
      string pairsAsString = String.Join(",", pairs);
      getPairsUrl += pairsAsString;

      Console.WriteLine(getPairsUrl);

      return await GetApiResultsAsync<DexscreenerRestResponseModel.PairsList>(getPairsUrl);
    }
  }
}
