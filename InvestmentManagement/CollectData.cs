using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Collections;

namespace InvestmentManagement
{
  public class CollectData
  {

    // Define the regular expression patterns to match numbers and non-numbers
    static string numberPattern = @"-?\d+(\.\d+)?";
    static string nonNumberPattern = @"[^\d\.]+";
    // Create regular expression objects for both patterns
    Regex numberRegex = new Regex(numberPattern);
    Regex nonNumberRegex = new Regex(nonNumberPattern);
    ExtractDataFromCSV extractDataFromCSV = new ExtractDataFromCSV();

    public void MergeFiles(string _documentFolderPath)
    {
      Console.WriteLine("-------------MergeFiles-------------");

      GoThroughEachFolderMerge(GetFoldersUnderCryptoDocumentPath(_documentFolderPath));

    }

    public void ExtractDataAndInsertInExcel(string _documentFolderPath, string _excelFile)
    {
      Console.WriteLine("-------------ExtractDataAndInsertInExcel-------------");
      extractDataFromCSV.ExtractNetworkData(@"C:\Projekte\Unterlagen\Cryptos\Dokumente\BNB Network\export-0x2c8ac232c76498fe46811879d20ce34b92983a9e.csv", ExtractDataFromCSV.Network.Bsc);
      InsertNetworkData(extractDataFromCSV.networkInfoList);
      GoThroughEachFolderExtract(GetFoldersUnderCryptoDocumentPath(_documentFolderPath));
      //extractDataFromCSV.ExtractData(@"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Kucoin\HISTORY_634c1b3bed20b6000741be35.csv", ExtractDataFromCSV.Cex.Kucoin);
      FormatData(ref extractDataFromCSV.cexBuySellInfoList); 
      InsertCexBuySellData(extractDataFromCSV.cexBuySellInfoList);
    }


    string[] GetFoldersUnderCryptoDocumentPath(string _documentPath)
    {
      string[] dirs = Directory.GetDirectories(_documentPath, "*", SearchOption.TopDirectoryOnly);

      foreach (string dir in dirs)
      {
        Console.WriteLine(dir);
      }

      return dirs;
    }

    void GoThroughEachFolderMerge(string[] _dirs)
    {
      string nameOfCex = "";

      for (int i = 0; i < _dirs.Length; i++)
      {
        string[] split = _dirs[i].Split('\\');

        string dirBuySell = _dirs[i] + "\\Käufe-Verkäufe";
        string csvFiles = dirBuySell + "\\Einzelne CSV Files";
        string[] fileNames = Directory.GetFiles(csvFiles);
        string outputfile = dirBuySell + "\\Alle Transaktionen.csv";
        char csvSeperator = ',';
        nameOfCex = split[split.Length - 1];

        switch (Enum.Parse(typeof(ExtractDataFromCSV.Cex), nameOfCex))
        {
          case ExtractDataFromCSV.Cex.Mexc:
            break;
          case ExtractDataFromCSV.Cex.Kucoin:
            break;
          case ExtractDataFromCSV.Cex.Binance:
            csvSeperator = ';';
            MergeFilesToOne(fileNames, outputfile);
            break;
          case ExtractDataFromCSV.Cex.Okx:
            break;
          default:
            break;
        }
      }
    }

    void GoThroughEachFolderExtract(string[] _dirs)
    {
      string nameOfCex = "";
      string csvFile = "Käufe-Verkäufe\\Alle Transaktionen.csv";

      for (int i = 0; i < _dirs.Length; i++)
      {
        string[] split = _dirs[i].Split('\\');

        nameOfCex = split[split.Length - 1];

        switch (Enum.Parse(typeof(ExtractDataFromCSV.Cex), nameOfCex))
        {
          case ExtractDataFromCSV.Cex.Mexc:
            extractDataFromCSV.ExtractCexData(_dirs[i] + csvFile, ExtractDataFromCSV.Cex.Mexc);
            break;
          case ExtractDataFromCSV.Cex.Kucoin:
            extractDataFromCSV.ExtractCexData(_dirs[i] + csvFile, ExtractDataFromCSV.Cex.Kucoin);
            break;
          case ExtractDataFromCSV.Cex.Binance:
            extractDataFromCSV.ExtractCexData(_dirs[i] + csvFile, ExtractDataFromCSV.Cex.Binance);
            break;
          case ExtractDataFromCSV.Cex.Okx:
            extractDataFromCSV.ExtractCexData(_dirs[i] + csvFile, ExtractDataFromCSV.Cex.Okx);
            break;
        }
        
      }
    }
    static void MergeFilesToOne(string[] _fileNames, string _outputFileName)
    {
      // Initialize a HashSet to hold the unique lines of all CSV files
      HashSet<string> uniqueLines = new HashSet<string>();

      // Iterate over the file names and read their lines into the HashSet
      foreach (string fileName in _fileNames)
      {
        // Read all the lines of the current file and add them to the HashSet
        string[] lines = File.ReadAllLines(fileName);
        foreach (string line in lines)
        {
          uniqueLines.Add(line);
        }
      }

      // Write the unique lines of the merged CSV file to the output file
      File.WriteAllLines(_outputFileName, uniqueLines);

    }
    void InsertCexBuySellData(List<CexBuySellInfo> _buySellInfoList)
    {
      Console.WriteLine("-------------InsertBuySellData-------------");

      string worksheet = "Kauf&Verkauf";

      HandleExcel.ClearRange("A4", "Z1000", worksheet);

      var cellOfDate = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfPair = HandleExcel.GetCellByName("Pair", worksheet);
      var cellOfBuy_Sell = HandleExcel.GetCellByName("Kauf/Verkauf?", worksheet);
      var cellOfDepot = HandleExcel.GetCellByName("Depot", worksheet);
      var cellOfRecievedAmount = HandleExcel.GetCellByName("Stückzahl", worksheet);
      var cellOfPrice = HandleExcel.GetCellByName("Preis pro Stück beim Kauf/Verkauf (bezogen auf das Pair)", worksheet);
      //var cellOfPriceCurrency = HandleExcel.GetCellByName("Stückzahl", worksheet);
      var cellOfFee = HandleExcel.GetCellByName("Gebühren (bezogen auf das Pair)", worksheet);
      var cellOfAmountInvested = HandleExcel.GetCellByName("Gezahlt/Bekommen insgesamt (bezogen auf das Pair)", worksheet);
      var cellOfAmountInvestedAfterFee = HandleExcel.GetCellByName("Gezahlt/Bekommen ohne Gebühr (bezogen auf das Pair)", worksheet);

      for (int i = 0; i < _buySellInfoList.Count; i++)
      {
        HandleExcel.SetTextInCell(_buySellInfoList[i].Date, cellOfDate.cellLine + 1 +i, cellOfDate.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].Price, cellOfPrice.cellLine + 1 + i, cellOfPrice.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].Plattfrom, cellOfDepot.cellLine + 1 + i, cellOfDepot.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].Pair, cellOfPair.cellLine + 1 + i, cellOfPair.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].Fee, cellOfFee.cellLine + 1 + i, cellOfFee.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].BuyOrSell, cellOfBuy_Sell.cellLine + 1 + i, cellOfBuy_Sell.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].RecievedAmount, cellOfRecievedAmount.cellLine + 1 + i, cellOfRecievedAmount.cellColum, worksheet);
        //HandleExcel.SetTextInCell(_buySellInfoList[i].PriceCurrency, cellOfPriceCurrency.cellLine + 1 + i, cellOfPriceCurrency.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].AmountInvestedAfterFee, cellOfAmountInvestedAfterFee.cellLine + 1 + i, cellOfAmountInvestedAfterFee.cellColum, worksheet);
      }

    }
    void InsertNetworkData(List<NetworkInfo> _networkInfoList)
    {
      Console.WriteLine("-------------InsertNetworkData-------------");

      string worksheet = "NetzwerkDaten";

      HandleExcel.ClearRange("A4", "Z1000", worksheet);

      var cellOfDatum = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfNetwork = HandleExcel.GetCellByName("Network", worksheet);
      var cellOfNetwork_Currency = HandleExcel.GetCellByName("Network Currency", worksheet);
      var cellOfTxhash = HandleExcel.GetCellByName("Txhash", worksheet);
      var cellOfFrom = HandleExcel.GetCellByName("From", worksheet);
      var cellOfTo = HandleExcel.GetCellByName("To", worksheet);
      var cellOfValue_In = HandleExcel.GetCellByName("Value In", worksheet);
      var cellOfValue_Out = HandleExcel.GetCellByName("Value Out", worksheet);
      var cellOfTxnFeeNative = HandleExcel.GetCellByName("TxnFeeNative", worksheet);
      var cellOfTxnFee_Usd = HandleExcel.GetCellByName("TxnFee Usd", worksheet);
      var cellOfHistorical_Price = HandleExcel.GetCellByName("Historical Price", worksheet);
      var cellOfMethod = HandleExcel.GetCellByName("Method", worksheet);

      for (int i = 0; i < _networkInfoList.Count; i++)
      {
        HandleExcel.SetTextInCell(_networkInfoList[i].DateTime, cellOfDatum.cellLine + 1 + i, cellOfDatum.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].Network, cellOfNetwork.cellLine + 1 + i, cellOfNetwork.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].NetworkCurrency, cellOfNetwork_Currency.cellLine + 1 + i, cellOfNetwork_Currency.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].Txhash, cellOfTxhash.cellLine + 1 + i, cellOfTxhash.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].From, cellOfFrom.cellLine + 1 + i, cellOfFrom.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].To, cellOfTo.cellLine + 1 + i, cellOfTo.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].ValueIn, cellOfValue_In.cellLine + 1 + i, cellOfValue_In.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].ValueOut, cellOfValue_Out.cellLine + 1 + i, cellOfValue_Out.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].TxnFeeNative, cellOfTxnFeeNative.cellLine + 1 + i, cellOfTxnFeeNative.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].TxnFeeUsd, cellOfTxnFee_Usd.cellLine + 1 + i, cellOfTxnFee_Usd.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].HistoricalPrice, cellOfHistorical_Price.cellLine + 1 + i, cellOfHistorical_Price.cellColum, worksheet);
        HandleExcel.SetTextInCell(_networkInfoList[i].Method, cellOfMethod.cellLine + 1 + i, cellOfMethod.cellColum, worksheet);
      }

    }
    void FormatData(ref List<CexBuySellInfo> _buySellInfoList)
    {
      for (int i = 0; i < _buySellInfoList.Count; i++)
      {
        FormatNumbers( _buySellInfoList[i]);
        FormatPair(_buySellInfoList[i]);

      }
    }

    void FormatNumbers( CexBuySellInfo _buySellInfo)
    {
      // Remove Tausender-Trennzeichen, falls vorhanden 
      _buySellInfo.Price = _buySellInfo.Price.Replace(",", "");
      _buySellInfo.RecievedAmount = _buySellInfo.RecievedAmount.Replace(",", "");
      _buySellInfo.AmountInvestedAfterFee = _buySellInfo.AmountInvestedAfterFee.Replace(",", "");
      _buySellInfo.Fee = _buySellInfo.Fee.Replace(",", "");

      switch (Enum.Parse(typeof(ExtractDataFromCSV.Cex), _buySellInfo.Plattfrom))
      {
        case ExtractDataFromCSV.Cex.Mexc:
          {

            string[] stringsToSeparte = { _buySellInfo.Price, _buySellInfo.RecievedAmount, _buySellInfo.AmountInvestedAfterFee, _buySellInfo.Fee };
            string[] numberString = new string[stringsToSeparte.Length];
            string[] nonNumberString = new string[stringsToSeparte.Length];
            decimal[] number = new decimal[stringsToSeparte.Length];
            for (int i = 0; i < stringsToSeparte.Length; i++)
            {

              // Extract the number and non-number substrings using regular expressions
              numberString[i] = numberRegex.Match(stringsToSeparte[i]).Value;
              nonNumberString[i] = nonNumberRegex.Match(stringsToSeparte[i]).Value;

              // Parse the number substring to a decimal number
              number[i] = decimal.Parse(numberString[i]);
            }
            _buySellInfo.Price = numberString[0];
            _buySellInfo.PriceCurrency = nonNumberString[0];
            _buySellInfo.RecievedAmount = numberString[1];
            _buySellInfo.AmountInvestedAfterFee = numberString[2];
            _buySellInfo.Fee = numberString[3];
          }
          
          break;
        case ExtractDataFromCSV.Cex.Kucoin:
          break;
        case ExtractDataFromCSV.Cex.Binance:
          {
            if (_buySellInfo.Pair.Equals("SHIBEUR"))
            {
              Console.WriteLine("");
            }
            string[] values = _buySellInfo.Fee.Split('+');

            string[] numberString = new string[values.Length];
            string[] nonNumberString = new string[values.Length];
            decimal fee = 0.0m;

            for (int i = 0; i < values.Length; i++)
            {
              // Extract the number and non-number substrings using regular expressions
              numberString[i] = numberRegex.Match(values[i]).Value;
              nonNumberString[i] = nonNumberRegex.Match(values[i]).Value;

              // Parse the number substring to a decimal number

              var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
              fee += decimal.Parse(numberString[i], numberFormatInfo);
            }
            _buySellInfo.Fee = fee.ToString(new CultureInfo("en-US"));

            _buySellInfo.FeeCurrency = nonNumberString[0];
          }
          
          break;
        case ExtractDataFromCSV.Cex.Okx:
          break;
        default:
          break;
      }
    }

    void FormatPair( CexBuySellInfo _buySellInfo)
    {
      string pairSeperator = "/";
      switch (Enum.Parse(typeof(ExtractDataFromCSV.Cex), _buySellInfo.Plattfrom))
      {
        case ExtractDataFromCSV.Cex.Mexc:
          _buySellInfo.Pair = _buySellInfo.Pair.Replace("_", pairSeperator);
          break;
        case ExtractDataFromCSV.Cex.Kucoin:
          _buySellInfo.Pair = _buySellInfo.Pair.Replace("-", pairSeperator);
          break;
        case ExtractDataFromCSV.Cex.Binance:
          _buySellInfo.Pair = SplitBinancePair(_buySellInfo.Pair);
          break;
        case ExtractDataFromCSV.Cex.Okx:
          break;
        default:
          break;
      }
    }

    string  SplitBinancePair( string _input)
    {
      // Define the suffixes to check
      string[] suffixes = { "EUR", "USDT", "BTC", "BNB", "ETC" };
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
  }//class CollectData
}
