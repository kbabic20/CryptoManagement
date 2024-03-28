using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement.Models
{
  public class DexscreenerRestResponseModel
  {

    public class PairsList
    {
      public string schemaVersion { get; set; }
      public Pair[] pairs { get; set; }
      public object pair { get; set; }
    }

    public class Pair
    {
      public string chainId { get; set; }
      public string dexId { get; set; }
      public string url { get; set; }
      public string pairAddress { get; set; }
      public Basetoken baseToken { get; set; }
      public Quotetoken quoteToken { get; set; }
      public string priceNative { get; set; }
      public string priceUsd { get; set; }
      public Txns txns { get; set; }
      public Volume volume { get; set; }
      public Pricechange priceChange { get; set; }
      public Liquidity liquidity { get; set; }
      public int fdv { get; set; }
      public long pairCreatedAt { get; set; }
    }

    public class Basetoken
    {
      public string address { get; set; }
      public string name { get; set; }
      public string symbol { get; set; }
    }

    public class Quotetoken
    {
      public string address { get; set; }
      public string name { get; set; }
      public string symbol { get; set; }
    }

    public class Txns
    {
      public M5 m5 { get; set; }
      public H1 h1 { get; set; }
      public H6 h6 { get; set; }
      public H24 h24 { get; set; }
    }

    public class M5
    {
      public int buys { get; set; }
      public int sells { get; set; }
    }

    public class H1
    {
      public int buys { get; set; }
      public int sells { get; set; }
    }

    public class H6
    {
      public int buys { get; set; }
      public int sells { get; set; }
    }

    public class H24
    {
      public int buys { get; set; }
      public int sells { get; set; }
    }

    public class Volume
    {
      public float h24 { get; set; }
      public float h6 { get; set; }
      public float h1 { get; set; }
      public float m5 { get; set; }
    }

    public class Pricechange
    {
      public float m5 { get; set; }
      public int h1 { get; set; }
      public int h6 { get; set; }
      public int h24 { get; set; }
    }

    public class Liquidity
    {
      public float usd { get; set; }
      public int _base { get; set; }
      public float quote { get; set; }
    }

  }
}
