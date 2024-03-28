﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement.Models
{
  public class CoinmarketcapRestResponseModel
  {
    public class CryptoMap
    {
      public Status Status { get; set; }
      public Data[] Data { get; set; }//List<Data> Data { get; set; }
  }

    public class Status
    {
      public DateTime timestamp { get; set; }
      public int error_code { get; set; }
      public object error_message { get; set; }
      public int elapsed { get; set; }
      public int credit_count { get; set; }
      public object notice { get; set; }
    }

    public class Data
    {
      public int id { get; set; }
      public int rank { get; set; }
      public string name { get; set; }
      public string symbol { get; set; }
      public string slug { get; set; }
      public int is_active { get; set; }
      public DateTime first_historical_data { get; set; }
      public DateTime last_historical_data { get; set; }
      public Platform platform { get; set; }
    }

    public class Platform
    {
      public int id { get; set; }
      public string name { get; set; }
      public string symbol { get; set; }
      public string slug { get; set; }
      public string token_address { get; set; }
    }

  }
}
