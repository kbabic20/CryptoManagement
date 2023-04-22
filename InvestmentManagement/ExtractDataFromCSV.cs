using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;

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
      CryptoCom
    }
    public enum Network
    {
      Ethereum = 1,
      Bsc,
      Ploygon,
      Fantom,
      Arbitrum,
      Optimism,
      Tron,
      Avalanche,
      Solana,
      Bitcoin
    }

    enum Spalte
    {
      A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R
    }

    public List<CexBuySellInfo> cexBuySellInfoList = new List<CexBuySellInfo>();
    public List<NetworkInfo> networkInfoList = new List<NetworkInfo>();

    public static DataTable GetDataFromCSV(string filePath)
    {
      DataTable data = new DataTable();
      
      using (TextFieldParser parser = new TextFieldParser(filePath))
      {
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(";",",");

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

     public void ExtractCexData(string _path, Cex _cex )
    {
      Console.WriteLine("-------------ExtractData-------------");

      // Define the regular expression patterns to match numbers and non-numbers
      string numberPattern = @"-?\d+(\.\d+)?";
      string nonNumberPattern = @"[^\d\.]+";
      // Create regular expression objects for both patterns
      Regex numberRegex = new Regex(numberPattern);
      Regex nonNumberRegex = new Regex(nonNumberPattern);

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


                CexBuySellInfo buySellInfo = new CexBuySellInfo
                {
                  Plattfrom = Cex.Mexc.ToString(),//"Mexc",
                  Pair = values[(int)Spalte.A], //.Replace("_", "/"),
                  Date = values[(int)Spalte.B],
                  BuyOrSell = values[(int)Spalte.C],
                  Price = values[(int)Spalte.D], //numberString[0],
                  PriceCurrency = values[(int)Spalte.D],
                  RecievedAmount = values[(int)Spalte.E],
                  AmountInvestedAfterFee = values[(int)Spalte.F],
                  Fee = values[(int)Spalte.G]
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
                  Plattfrom = Cex.Kucoin.ToString(),//"Kucoin",
                  Pair = values[(int)Spalte.D],//.Replace("-", "/"),
                  Date = values[(int)Spalte.A],
                  BuyOrSell = values[(int)Spalte.E],
                  Price = values[(int)Spalte.L],
                  PriceCurrency = values[(int)Spalte.N],
                  RecievedAmount = values[(int)Spalte.J],
                  AmountInvestedAfterFee = values[(int)Spalte.K],
                  Fee = values[(int)Spalte.M]
                };

                cexBuySellInfoList.Add(buySellInfo);
              }
              break;
            case Cex.Binance:
              {
                bool isSubOrder = false;
                // Skip first line
                if (line.Equals("Date(UTC);OrderNo;Pair;Type;Order Price;Order Amount;AvgTrading Price;Filled;Total;status"))
                {
                  continue;
                }

                // Skip first line
                if (line.Equals(";Date(UTC);Trading Price;Filled;Total;Fee;;;;"))
                {
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

                if (!isSubOrder)
                {
                  CexBuySellInfo buySellInfo = new CexBuySellInfo
                  {
                    Plattfrom = Cex.Binance.ToString(),
                    Pair = values[(int)Spalte.C],
                    Date = values[(int)Spalte.A],
                    BuyOrSell = values[(int)Spalte.D],
                    Price = values[(int)Spalte.G],
                    PriceCurrency = values[(int)Spalte.C],
                    RecievedAmount = values[(int)Spalte.F],
                    AmountInvestedAfterFee = values[(int)Spalte.I],
                    Fee = ""
                  };

                  cexBuySellInfoList.Add(buySellInfo);

                }
                else
                {
                  if (cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee.Length > 0)
                  {

                    cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee += "+" + values[(int)Spalte.F];
                  }
                  else
                  {
                    cexBuySellInfoList[cexBuySellInfoList.Count - 1].Fee = values[(int)Spalte.F];
                  }
                }

              }

              break;
            case Cex.Okx:
              break;
            default:
              break;
          }
        }
      }
    }

    public void ExtractNetworkData(string _path, Network _network)
    {
      Console.WriteLine("-------------ExtractData-------------");

      // Define the regular expression patterns to match numbers and non-numbers
      string numberPattern = @"-?\d+(\.\d+)?";
      string nonNumberPattern = @"[^\d\.]+";
      // Create regular expression objects for both patterns
      Regex numberRegex = new Regex(numberPattern);
      Regex nonNumberRegex = new Regex(nonNumberPattern);

      using (StreamReader reader = new StreamReader(_path))
      {
        while (!reader.EndOfStream)
        {
          string line = reader.ReadLine();

          switch (_network)
          {
            case Network.Ethereum:
              break;
            case Network.Bsc:
              { 
              // Skip first line
              if (line.Contains("Txhash,\"Blockno\",\"UnixTimestamp\",\"DateTime\",\"From\",\"To\",\"ContractAddress\","))
              {
                continue;
              }

              string[] values = line.Split(',');

              NetworkInfo networkInfo = new NetworkInfo
              {
                Network = Network.Bsc.ToString(),
                NetworkCurrency = "BNB",
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
                Method = values[(int)Spalte.P],
              };

              networkInfoList.Add(networkInfo);
          }
          break;
            case Network.Ploygon:
              break;
            case Network.Fantom:
              break;
            case Network.Arbitrum:
              break;
            case Network.Optimism:
              break;
            case Network.Tron:
              break;
            case Network.Avalanche:
              break;
            case Network.Solana:
              break;
            case Network.Bitcoin:
              break;
            default:
              break;
          }
        }
      }
    }
  }
}
