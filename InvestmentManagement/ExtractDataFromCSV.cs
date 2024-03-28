using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Globalization;

namespace InvestmentManagement
{
  public class ExtractDataFromCSV
  {
    public enum Cex
    {
      Mexc = 1,
      Kucoin,
      Binance,
      Okx,
      CryptoCom,
      Xeggex
    }
    public enum Network
    {
      Ethereum = 1,
      Bsc,
      Polygon,
      Fantom,
      Arbitrum,
      Optimism,
      Tron,
      Avalanche,
      Solana,
      Bitcoin,
      Moonbeam,
      Aurora
    }
    public enum ExtractFormat
    {
      Ethereum = 1,
      Bsc = 1,
      Polygon = 1,
      Fantom = 1,
      Arbitrum = 1,
      Optimism = 1,
      Avalanche = 1,
      Moonbeam = 1,
      Tron,
      Solana,
      Bitcoin,
      Aurora
    }

    enum Spalte
    {
      A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R
    }

    public List<CexBuySellInfo> cexBuySellInfoList = new List<CexBuySellInfo>();
    public List<NetworkTxnInfo> networkTxnInfoList = new List<NetworkTxnInfo>();
    public List<NetworkTokenTxnInfo> networkTokenTxnInfoList = new List<NetworkTokenTxnInfo>();

    // Okx Convert variables
    bool isFirstEntry = true;
    string saveDate = "";

    string pair = "";
    string amount = "";
    string buyOrSell = "";
    decimal price = 0;

    public static DataTable GetDataFromCSV(string filePath)
    {
      DataTable data = new DataTable();

      using (TextFieldParser parser = new TextFieldParser(filePath))
      {
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(";", ",");

        string[] headers = parser.ReadFields();
        foreach (string header in headers)
        {
          data.Columns.Add(header);
        }

        while (!parser.EndOfData)
        {
          string[] fields = parser.ReadFields();
          data.Rows.Add(fields);
        }
      }

      return data;
    }

    public static void SaveDataToExcel(DataTable data, string filePath)
    {
      Excel.Application excelApp = new Excel.Application();
      Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
      Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;

      for (int i = 0; i < data.Columns.Count; i++)
      {
        excelWorksheet.Cells[1, i + 1] = data.Columns[i].ColumnName;
      }

      for (int i = 0; i < data.Rows.Count; i++)
      {
        for (int j = 0; j < data.Columns.Count; j++)
        {
          excelWorksheet.Cells[i + 2, j + 1] = data.Rows[i][j];
        }
      }

      excelWorkbook.SaveAs(filePath);
      excelWorkbook.Close();
      excelApp.Quit();
    }

    public void ExtractCexData(string _path, Cex _cex)
    {
      Console.WriteLine("-------------ExtractCexData-------------");

      // Define the regular expression patterns to match numbers and non-numbers
      string numberPattern = @"-?\d+(\.\d+)?";
      string nonNumberPattern = @"[^\d\.]+";
      // Create regular expression objects for both patterns
      Regex numberRegex = new Regex(numberPattern);
      Regex nonNumberRegex = new Regex(nonNumberPattern);
      int dataFormat = 0;

      using (StreamReader reader = new StreamReader(_path))
      {
        while (!reader.EndOfStream)
        {
          string line = reader.ReadLine();


          switch (_cex)
          {
            case Cex.Mexc:
              {
                // Skip first line
                if (line.Equals("Pairs,Time,Side,Filled Price,Executed Amount,Total,Fee,Role"))
                {
                  continue;
                }

                if (line.Equals("Paare,Zeit,Seite,Realisierter Preis,Ausgeführter Betrag,Gesamt,Gebühr,Rolle"))
                {
                  continue;
                }

                string[] values = line.Split(',');
                int separateNumberCount = 4;
                string[] input = new string[separateNumberCount];
                string[] numberString = new string[separateNumberCount];
                string[] nonNumberString = new string[separateNumberCount];
                decimal[] number = new decimal[separateNumberCount];
                for (int i = 0; i < separateNumberCount; i++)
                {
                  input[i] = values[(int)Spalte.D + i];

                  // Extract the number and non-number substrings using regular expressions
                  numberString[i] = numberRegex.Match(input[i]).Value;
                  nonNumberString[i] = nonNumberRegex.Match(input[i]).Value;

                  // Parse the number substring to a decimal number
                  number[i] = decimal.Parse(numberString[i]);
                }

                string feeCurr = "";

                if (nonNumberString[3].Length > 0)
                {
                  feeCurr = nonNumberString[3];
                }
                else
                {
                  feeCurr = values[(int)Spalte.A].Split('_')[1];
                }

                CexBuySellInfo buySellInfo = new CexBuySellInfo
                {
                  Plattfrom = _cex.ToString(),//"Mexc",
                  Pair = values[(int)Spalte.A].Replace("_", "/"),
                  Date = values[(int)Spalte.B],
                  BuyOrSell = values[(int)Spalte.C],
                  Price = numberString[0],
                  PriceCurrency = values[(int)Spalte.D],
                  RecievedAmount = numberString[1],
                  AmountInvestedAfterFee = numberString[2],
                  Fee = numberString[3],
                  FeeCurrency = feeCurr
                };

                cexBuySellInfoList.Add(buySellInfo);
              }

              break;
            case Cex.Kucoin:
              {
                // Skip first line
                if (line.Equals("orderCreatedAt,id,clientOid,symbol,side,type,stopPrice,price,size,dealSize,dealFunds,averagePrice,fee,feeCurrency,remark,tags,orderStatus,"))
                {
                  continue;
                }

                string[] values = line.Split(',');

                CexBuySellInfo buySellInfo = new CexBuySellInfo
                {
                  Plattfrom = _cex.ToString(),//"Kucoin",
                  Pair = values[(int)Spalte.D].Replace("-", "/"),
                  Date = values[(int)Spalte.A],
                  BuyOrSell = values[(int)Spalte.E],
                  Price = values[(int)Spalte.L],
                  PriceCurrency = values[(int)Spalte.N],
                  RecievedAmount = values[(int)Spalte.J],
                  AmountInvestedAfterFee = values[(int)Spalte.K],
                  Fee = values[(int)Spalte.M],
                  FeeCurrency = values[(int)Spalte.N]
                };

                cexBuySellInfoList.Add(buySellInfo);
              }
              break;
            case Cex.Binance:
              {
                bool isSubOrder = false;
                
                // Skip first line
                if (line.Equals("Date(UTC);Pair;Type;Order Price;Order Amount;AvgTrading Price;Filled;Total;status"))
                {
                  dataFormat = 1;
                  continue;
                }

                // Skip first line
                if (line.Equals(";Date(UTC);Trading Price;Filled;Total;Fee;;;"))
                {
                  dataFormat = 1;
                  continue;
                }

                // Skip first line
                if (line.Equals("Date(UTC);OrderNo;Pair;Type;Order Price;Order Amount;AvgTrading Price;Filled;Total;status"))
                {
                  dataFormat = 2;
                  continue;
                }

                // Skip first line
                if (line.Equals(";Date(UTC);Trading Price;Filled;Total;Fee;;;;"))
                {
                  dataFormat = 2;
                  continue;
                }

                // Skip first line
                if (line.Equals("Date(UTC);Market;Type;Price;Amount;Total;Fee;Fee Coin"))
                {
                  dataFormat = 3;
                  continue;
                }

                string[] values = line.Split(';');

                // Get sub orders
                if (!(values[0].Length > 0) && values[1].Length > 0)
                {
                  isSubOrder = true;
                }
                else
                {
                  isSubOrder = false;
                }

                switch (dataFormat)
                {
                  case 1:
                    if (!isSubOrder)
                    {
                      CexBuySellInfo buySellInfo = new CexBuySellInfo
                      {
                        Plattfrom = _cex.ToString(),
                        Date = values[(int)Spalte.A],
                        Pair = SplitBinancePair(values[(int)Spalte.B]),
                        PriceCurrency = values[(int)Spalte.B],
                        BuyOrSell = values[(int)Spalte.C],
                        Price = values[(int)Spalte.F],
                        RecievedAmount = values[(int)Spalte.G],
                        AmountInvestedAfterFee = values[(int)Spalte.H],
                        Fee = ""
                      };

                      cexBuySellInfoList.Add(buySellInfo);

                    }
                    else
                    {
                      if (cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee.Length > 0)
                      {
                        // Extract the number and non-number substrings using regular expressions
                        string numberString = numberRegex.Match(values[(int)Spalte.F]).Value;
                        string nonNumberString = nonNumberRegex.Match(values[(int)Spalte.F]).Value;

                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee = (decimal.Parse(cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee) + decimal.Parse(numberString)).ToString(CultureInfo.InvariantCulture); 
                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].FeeCurrency = nonNumberString;
                      }
                      else
                      {
                        // Extract the number and non-number substrings using regular expressions
                        string numberString = numberRegex.Match(values[(int)Spalte.F]).Value;
                        string nonNumberString = nonNumberRegex.Match(values[(int)Spalte.F]).Value;

                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee = numberString;
                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].FeeCurrency = nonNumberString;
                      }
                    }
                    break;
                  case 2:
                    if (!isSubOrder)
                    {
                      CexBuySellInfo buySellInfo = new CexBuySellInfo
                      {
                        Plattfrom = _cex.ToString(),
                        Date = values[(int)Spalte.A],
                        Pair = SplitBinancePair(values[(int)Spalte.C]),
                        PriceCurrency = values[(int)Spalte.C],
                        BuyOrSell = values[(int)Spalte.D],
                        Price = values[(int)Spalte.G],
                        RecievedAmount = values[(int)Spalte.H],
                        AmountInvestedAfterFee = values[(int)Spalte.I],
                        Fee = ""
                      };

                      cexBuySellInfoList.Add(buySellInfo);

                    }
                    else
                    {
                      if (cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee.Length > 0)
                      {
                        // Extract the number and non-number substrings using regular expressions
                        string numberString = numberRegex.Match(values[(int)Spalte.F]).Value;
                        string nonNumberString = nonNumberRegex.Match(values[(int)Spalte.F]).Value;

                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee = (decimal.Parse(cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee) + decimal.Parse(numberString)).ToString(CultureInfo.InvariantCulture);
                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].FeeCurrency = nonNumberString;
                      }
                      else
                      {
                        // Extract the number and non-number substrings using regular expressions
                        string numberString = numberRegex.Match(values[(int)Spalte.F]).Value;
                        string nonNumberString = nonNumberRegex.Match(values[(int)Spalte.F]).Value;

                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee = numberString;
                        cexBuySellInfoList[cexBuySellInfoList.Count - 1].FeeCurrency = nonNumberString;
                      }
                    }
                    break;
                  case 3:
                    {
                      CexBuySellInfo buySellInfo = new CexBuySellInfo
                      {
                        Plattfrom = _cex.ToString(),
                        Date = values[(int)Spalte.A],
                        Pair = SplitBinancePair(values[(int)Spalte.B]),
                        PriceCurrency = values[(int)Spalte.B],
                        BuyOrSell = values[(int)Spalte.C],
                        Price = values[(int)Spalte.D],
                        RecievedAmount = values[(int)Spalte.E],
                        AmountInvestedAfterFee = values[(int)Spalte.F],
                        Fee = values[(int)Spalte.G],
                        FeeCurrency = values[(int)Spalte.H],
                      };

                      cexBuySellInfoList.Add(buySellInfo);
                    }
                    
                    break;
                  default:
                    break;
                }
              }

              break;
            case Cex.CryptoCom:
              {
                // Skip first line
                if (line.Equals("Timestamp (UTC),Transaction Description,Currency,Amount,To Currency,To Amount,Native Currency,Native Amount,Native Amount (in USD),Transaction Kind,Transaction Hash"))
                {
                  continue;
                }

                string[] values = line.Split(',');


                string buyOrSell = "";
                decimal price = 0;

                if (values[(int)Spalte.J].Contains("crypto_purchase"))
                {
                  buyOrSell = "Buy";
                  price = decimal.Parse(values[(int)Spalte.H]) / decimal.Parse(values[(int)Spalte.D]);

                  CexBuySellInfo buySellInfo = new CexBuySellInfo
                  {
                    Date = values[(int)Spalte.A],
                    Plattfrom = _cex.ToString(),
                    Pair = values[(int)Spalte.C] + "/" + values[(int)Spalte.G],
                    BuyOrSell = buyOrSell,
                    Price = price.ToString(CultureInfo.InvariantCulture),
                    PriceCurrency = values[(int)Spalte.G],
                    RecievedAmount = values[(int)Spalte.D],
                    AmountInvestedAfterFee = values[(int)Spalte.H],
                    Fee = "0",
                    FeeCurrency = "N/A"
                  };

                  cexBuySellInfoList.Add(buySellInfo);
                }
                else if (values[(int)Spalte.J].Contains("rewards_platform_deposit_credited") || values[(int)Spalte.J].Contains("admin_wallet_credited"))
                {
                  buyOrSell = "Reward";

                  CexBuySellInfo buySellInfo = new CexBuySellInfo
                  {
                    Date = values[(int)Spalte.A],
                    Plattfrom = _cex.ToString(),
                    Pair = values[(int)Spalte.C] + "/" + values[(int)Spalte.G],
                    BuyOrSell = buyOrSell,
                    Price = "0",
                    PriceCurrency = values[(int)Spalte.G],
                    RecievedAmount = values[(int)Spalte.D],
                    AmountInvestedAfterFee = "0",
                    Fee = "0",
                    FeeCurrency = "N/A"
                  };

                  cexBuySellInfoList.Add(buySellInfo);
                }
                else if (values[(int)Spalte.J].Contains("crypto_exchange"))
                {
                  buyOrSell = "Buy";
                  price = decimal.Parse(values[(int)Spalte.H]) / decimal.Parse(values[(int)Spalte.F]);

                  CexBuySellInfo buySellInfo = new CexBuySellInfo
                  {
                    Date = values[(int)Spalte.A],
                    Plattfrom = _cex.ToString(),
                    Pair = values[(int)Spalte.E] + "/" + values[(int)Spalte.G],
                    BuyOrSell = buyOrSell,
                    Price = "0",
                    PriceCurrency = values[(int)Spalte.G],
                    RecievedAmount = values[(int)Spalte.F],
                    AmountInvestedAfterFee = values[(int)Spalte.H],
                    Fee = "0",
                    FeeCurrency = "N/A"
                  };

                  cexBuySellInfoList.Add(buySellInfo);
                }
              }
              
              break;
            case Cex.Xeggex:
              {
                // Skip first line
                if (line.Equals("Type,Time,Market,Side,Price,Quantity,TotalWithFee,AlternateFee"))
                {
                  continue;
                }

                string[] values = SplitString(line);

                CexBuySellInfo buySellInfo = new CexBuySellInfo
                {
                  Plattfrom = _cex.ToString(),
                  Pair = values[(int)Spalte.C],
                  Date = values[(int)Spalte.B],
                  BuyOrSell = values[(int)Spalte.D],
                  Price = values[(int)Spalte.E],
                  PriceCurrency = values[(int)Spalte.C],
                  RecievedAmount = values[(int)Spalte.F],
                  AmountInvestedAfterFee = values[(int)Spalte.G]
                };

                cexBuySellInfoList.Add(buySellInfo);
              }
              break;
            case Cex.Okx:
              {
                // Skip first line
                if (line.Equals("UID:425832338606700738,﻿Name:,﻿Verification:"))
                {
                  continue;
                }

                // Skip second line
                if (line.Equals("﻿id,Time,Type,Amount,Before Balance,After Balance,Fee,Symbol"))
                {
                  continue;
                }

                OkxCoinConvert(line, _cex.ToString());
              }
              break;
            default:
              break;
          }
        }
      }
    }

    string SplitBinancePair(string _input)
    {
      // Define the suffixes to check
      string[] suffixes = { "EUR", "USDT", "BTC", "BNB", "ETC" , "BUSD", "ETH"};
      string results = "";
      // Iterate over the suffixes and check if the input string ends with any of them
      foreach (string suffix in suffixes)
      {
        if (_input.EndsWith(suffix))
        {
          // If the input string ends with a suffix, split it into two parts
          int suffixLength = suffix.Length;
          results = _input.Substring(0, _input.Length - suffixLength);
          results += "/" + _input.Substring(_input.Length - suffixLength);
        }
      }
      return results;
    }

    static string[] SplitString(string _input)
    {
      string[] parts = new string[8];
      int startIndex = 0;
      int endIndex = 0;
      int partIndex = 0;
      bool insideQuotes = false;

      for (int i = 0; i < _input.Length; i++)
      {
        if (_input[i] == '"')
        {
          insideQuotes = !insideQuotes;
        }

        if (_input[i] == ',' && !insideQuotes)
        {
          endIndex = i;
          parts[partIndex] = _input.Substring(startIndex , endIndex - startIndex ).Replace("\"","");
          startIndex = endIndex + 1;
          partIndex++;
        }
      }

      // Last part
      parts[partIndex] = _input.Substring(startIndex , _input.Length - startIndex );

      return parts;
    }

    void OkxCoinConvert(string _input, string _cexName)
    {
      string[] values = _input.Split(',');

      
      

      if (values[(int)Spalte.C].Equals("Convert"))
      {
        if (isFirstEntry)
        {
          saveDate = values[(int)Spalte.B];
          pair = values[(int)Spalte.H];
          amount = values[(int)Spalte.D];
          buyOrSell = "Buy";
          isFirstEntry = false;
        }
        else
        {
          if (values[(int)Spalte.B].Equals(saveDate))
          {
            pair += "/" + values[(int)Spalte.H];
            decimal paid = decimal.Parse(values[(int)Spalte.D].Replace("-", ""), CultureInfo.InvariantCulture);
            price = paid / decimal.Parse(amount, CultureInfo.InvariantCulture);

            CexBuySellInfo buySellInfo = new CexBuySellInfo
            {
              Plattfrom = _cexName,
              Pair = pair,
              Date = saveDate,
              BuyOrSell = buyOrSell,
              Price = price.ToString(CultureInfo.InvariantCulture),
              PriceCurrency = values[(int)Spalte.H],
              RecievedAmount = amount,
              AmountInvestedAfterFee = paid.ToString(CultureInfo.InvariantCulture)
            };

            cexBuySellInfoList.Add(buySellInfo);
          }

        }
      }
    }


    public void ExtractNetworkTxnData(string _path, Network _network)
    {
      Console.WriteLine("-------------ExtractNetworkTxnData-------------");
      string networkCurrency = "";
      using (StreamReader reader = new StreamReader(_path))
      {
        while (!reader.EndOfStream)
        {
          string line = reader.ReadLine();

          {
            string[] values = line.Split(new string[] { ",\"" }, StringSplitOptions.None);
            if (values.Length < 2)
            {
              values = line.Split(new string[] { "," }, StringSplitOptions.None);
            }
            for (int i = 0; i < values.Length; i++)
            {
              values[i] = values[i].Replace(",", "");

              values[i] = values[i].Replace("\"", "");
            }

            // Skip first line
            if (values[0].ToLower().Contains("txhash"))
            {
              //networkCurrency = values[7].Substring(9, values[7].Length - 2 - 8);
              continue;
            }
            networkCurrency = GetNetworkCurrencey(_network);

            switch (((int)Enum.Parse(typeof(ExtractDataFromCSV.ExtractFormat), _network.ToString())))
            {
              case (int)ExtractFormat.Aurora:
                {

                  NetworkTxnInfo networkInfo = new NetworkTxnInfo
                  {
                    Network = _network.ToString(),
                    NetworkCurrency = networkCurrency,
                    Txhash = values[(int)Spalte.A],
                    Blockno = values[(int)Spalte.B],
                    DateTime = values[(int)Spalte.C].Replace(".000000Z", ""),
                    From = values[(int)Spalte.D],
                    To = values[(int)Spalte.E],
                    ContractAddress = values[(int)Spalte.F],
                    Status = values[(int)Spalte.J],
                    ErrCode = values[(int)Spalte.K],
                  };

                  string type = values[(int)Spalte.G];
                  double value = double.Parse(values[(int)Spalte.H]) / Math.Pow(10, 18);
                  if (type.Equals("OUT"))
                  {
                    networkInfo.ValueOut = value.ToString();
                  }
                  else
                  {
                    networkInfo.ValueIn = value.ToString();
                  }
                    
                  networkTxnInfoList.Add(networkInfo);


                  decimal fee = decimal.Parse(values[(int)Spalte.I]) /(decimal) Math.Pow(10, 18);
                  networkInfo.TxnFeeNative = fee.ToString();
                }
                
                break;
              default:
                {
                  NetworkTxnInfo networkInfo = new NetworkTxnInfo
                  {
                    Network = _network.ToString(),
                    NetworkCurrency = networkCurrency,
                    Txhash = values[(int)Spalte.A],
                    Blockno = values[(int)Spalte.B],
                    UnixTimestamp = values[(int)Spalte.C],
                    DateTime = values[(int)Spalte.D],
                    From = values[(int)Spalte.E],
                    To = values[(int)Spalte.F],
                    ContractAddress = values[(int)Spalte.G],
                    ValueIn = values[(int)Spalte.H],
                    ValueOut = values[(int)Spalte.I],
                    TxnFeeNative = values[(int)Spalte.K],
                    TxnFeeUsd = values[(int)Spalte.L],
                    HistoricalPrice = values[(int)Spalte.M],
                    Status = values[(int)Spalte.N],
                    ErrCode = values[(int)Spalte.O],
                    Method = values[(int)Spalte.P],
                  };

                  networkTxnInfoList.Add(networkInfo);
                }
                
                break;
            }


          }
        }
      }
    }

    string GetNetworkCurrencey(Network _network)
    {
      string networkCurrency = "N/A";
      switch (_network)
      {
        case Network.Ethereum:

          networkCurrency = "ETH";
          break;
        case Network.Bsc:

          networkCurrency = "BNB";
          break;
        case Network.Polygon:

          networkCurrency = "MATIC";
          break;
        case Network.Fantom:

          networkCurrency = "FTM";
          break;
        case Network.Arbitrum:

          networkCurrency = "ETH";
          break;
        case Network.Optimism:

          networkCurrency = "ETH";
          break;
        case Network.Tron:

          break;
        case Network.Avalanche:
          networkCurrency = "AVAX";
          break;
        case Network.Solana:
          break;
        case Network.Bitcoin:

          networkCurrency = "BTC";
          break;
        case Network.Moonbeam:
          networkCurrency = "GLMR";
          break;
        case Network.Aurora:

          networkCurrency = "ETH";
          break;
        default:
          break;
      }

      return networkCurrency;
    }
    public void ExtractNetworkTokenTxnData(string _path, Network _network)
    {
      Console.WriteLine("-------------ExtractNetworkTokenTxnkData-------------");

      // Define the regular expression patterns to match numbers and non-numbers
      string numberPattern = @"-?\d+(\.\d+)?";
      string nonNumberPattern = @"[^\d\.]+";
      // Create regular expression objects for both patterns
      Regex numberRegex = new Regex(numberPattern);
      Regex nonNumberRegex = new Regex(nonNumberPattern);
      int extractFormat;

      using (StreamReader reader = new StreamReader(_path))
      {
        while (!reader.EndOfStream)
        {
          string line = reader.ReadLine();
          string networkCurrency = "N/A";
          //line = line.Replace("\"", "");
          string[] values = line.Split(new string[] { ",\"" }, StringSplitOptions.None);
          if (values.Length <=1)
          {
            values = line.Split(new string[] { "," }, StringSplitOptions.None);
          }
          for (int i = 0; i < values.Length; i++)
          {
            values[i] = values[i].Replace(",", "");

            values[i] = values[i].Replace("\"", "");
          }

          // Skip first line
          if (values[0].ToLower().Contains("txhash"))
          {
            continue;
          }

          networkCurrency = GetNetworkCurrencey(_network);
          switch ((int)Enum.Parse(typeof(ExtractDataFromCSV.ExtractFormat), _network.ToString()))
          {
            case 1:
              {
                NetworkTokenTxnInfo networkTokentxnInfo = new NetworkTokenTxnInfo
                {
                  Network = _network.ToString(),
                  NetworkCurrency = networkCurrency,
                  Txhash = values[(int)Spalte.A],
                  Blockno = values[(int)Spalte.B],
                  UnixTimestamp = values[(int)Spalte.C],
                  DateTime = values[(int)Spalte.D],
                  From = values[(int)Spalte.E],
                  To = values[(int)Spalte.F],
                  TokenAmount = values[(int)Spalte.G],
                  UsdValueDayOfTx = values[(int)Spalte.H],
                  ContractAddress = values[(int)Spalte.I],
                  TokenName = values[(int)Spalte.J],
                  TokenSymbol = values[(int)Spalte.K]
                };

                networkTokenTxnInfoList.Add(networkTokentxnInfo);
              }
              

              break;


            case 2:
              {
                NetworkTokenTxnInfo networkTokentxnInfo = new NetworkTokenTxnInfo
                {
                  Network = _network.ToString(),
                  NetworkCurrency = networkCurrency,
                  Txhash = values[(int)Spalte.A],
                  Blockno = "N/A",
                  UnixTimestamp = values[(int)Spalte.B],
                  DateTime = values[(int)Spalte.C],
                  From = values[(int)Spalte.D],
                  To = values[(int)Spalte.E],
                  TokenAmount = values[(int)Spalte.F],
                  UsdValueDayOfTx = values[(int)Spalte.G],
                  ContractAddress = values[(int)Spalte.H],
                  TokenName = values[(int)Spalte.I],
                  TokenSymbol = values[(int)Spalte.J]
                };

                networkTokenTxnInfoList.Add(networkTokentxnInfo);
              }

              break;

            case (int)ExtractFormat.Aurora:
              {
                NetworkTokenTxnInfo networkTokentxnInfo = new NetworkTokenTxnInfo
                {
                  Network = _network.ToString(),
                  NetworkCurrency = networkCurrency,
                  Txhash = values[(int)Spalte.A],
                  Blockno = values[(int)Spalte.B],
                  DateTime = values[(int)Spalte.C].Replace(".000000Z", ""),
                  From = values[(int)Spalte.D],
                  To = values[(int)Spalte.E],
                  ContractAddress = values[(int)Spalte.F],
                  TokenSymbol = values[(int)Spalte.H]
                };
                double amount = double.Parse(values[(int)Spalte.J]) / Math.Pow(10, int.Parse(values[(int)Spalte.I]));
                networkTokentxnInfo.TokenAmount = amount.ToString();
                networkTokenTxnInfoList.Add(networkTokentxnInfo);
              }


              break;

            default:
              break;
          }
        

        
        }
      }
    }
  }
}
