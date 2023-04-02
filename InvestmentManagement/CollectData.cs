using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace InvestmentManagement
{
  public class CollectData
  {

    public void ExtractDataAndInsertInExcel(string _fileToExtract, string _excelFile)
    {
      Console.WriteLine("-------------ExtractDataAndInsertInExcel-------------");

      ExtractDataFromCSV extractDataFromCSV = new ExtractDataFromCSV();
      extractDataFromCSV.ExtractData(_fileToExtract, ExtractDataFromCSV.Cex.Mexc);
      extractDataFromCSV.ExtractData(@"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Kucoin\HISTORY_634c1b3bed20b6000741be35.csv", ExtractDataFromCSV.Cex.Kucoin);
      InsertBuySellData(extractDataFromCSV.buySellInfoList);
    }

    void InsertBuySellData(List<BuySellInfo> _buySellInfoList)
    {
      Console.WriteLine("-------------InsertBuySellData-------------");

      string worksheet = "Kauf&Verkauf";

      HandleExcel.ClearRange("A4", "I1000", worksheet);

      var cellOfDate = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfPair = HandleExcel.GetCellByName("Pair", worksheet);
      var cellOfBuy_Sell = HandleExcel.GetCellByName("Kauf/Verkauf", worksheet);
      var cellOfDepot = HandleExcel.GetCellByName("Depot", worksheet);
      var cellOfRecievedAmount = HandleExcel.GetCellByName("Stückzahl", worksheet);
      var cellOfPrice = HandleExcel.GetCellByName("Preis pro Stück beim Kauf/Verkauf (bezogen auf das Pair)", worksheet);
      var cellOfPriceCurrency = HandleExcel.GetCellByName("Stückzahl", worksheet);
      var cellOfFee = HandleExcel.GetCellByName("Gebühren (bezogen auf das Pair)", worksheet);
      var cellOfAmountInvested = HandleExcel.GetCellByName("Gezahlt/Bekommen insgesamt (bezogen auf das Pair)", worksheet);

      for (int i = 0; i < _buySellInfoList.Count; i++)
      {
        HandleExcel.SetTextInCell(_buySellInfoList[i].Date, cellOfDate.cellLine + 1 +i, cellOfDate.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].Price, cellOfPrice.cellLine + 1 + i, cellOfPrice.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].Plattfrom, cellOfDepot.cellLine + 1 + i, cellOfDepot.cellColum, worksheet);
        HandleExcel.SetTextInCell(_buySellInfoList[i].Pair, cellOfPair.cellLine + 1 + i, cellOfPair.cellColum, worksheet);
      }

    }
    void FormatData(ref List<BuySellInfo> _buySellInfoList)
    {
      for (int i = 0; i < _buySellInfoList.Count; i++)
      {
        switch (Enum.Parse( typeof( ExtractDataFromCSV.Cex), _buySellInfoList[i].Plattfrom))  
        {
          case ExtractDataFromCSV.Cex.Mexc:
            break;
          case ExtractDataFromCSV.Cex.Kucoin:
            break;
          case ExtractDataFromCSV.Cex.Binance:
            break;
          case ExtractDataFromCSV.Cex.Okx:
            break;
          default:
            break;
        }

      }
    }

    void FormatPrice(ref BuySellInfo _buySellInfo)
    {
      // Define the regular expression patterns to match numbers and non-numbers
      string numberPattern = @"-?\d+(\.\d+)?";
      string nonNumberPattern = @"[^\d\.]+";
      // Create regular expression objects for both patterns
      Regex numberRegex = new Regex(numberPattern);
      Regex nonNumberRegex = new Regex(nonNumberPattern);

      switch (Enum.Parse(typeof(ExtractDataFromCSV.Cex), _buySellInfo.Plattfrom))
      {
        case ExtractDataFromCSV.Cex.Mexc:
          int separateNumberCount = 4;
          string[] stringsToSeparte = { _buySellInfo.Price, _buySellInfo.RecievedAmount, _buySellInfo.AmountInvestedAfterFee  , _buySellInfo.Fee };
          string[] input = new string[separateNumberCount];
          string[] numberString = new string[separateNumberCount];
          string[] nonNumberString = new string[separateNumberCount];
          decimal[] number = new decimal[separateNumberCount];
          for (int i = 0; i < stringsToSeparte.Length; i++)
          {
            input[i] = stringsToSeparte[i];

            // Extract the number and non-number substrings using regular expressions
            numberString[i] = numberRegex.Match(input[i]).Value;
            nonNumberString[i] = nonNumberRegex.Match(input[i]).Value;

            // Parse the number substring to a decimal number
            number[i] = decimal.Parse(numberString[i]);
          }
          _buySellInfo.Price = numberString[0];
          _buySellInfo.PriceCurrency = nonNumberString[0];
          _buySellInfo.RecievedAmount = numberString[1];
          _buySellInfo.AmountInvestedAfterFee = numberString[2];
          _buySellInfo.Fee = numberString[3];


          break;
        case ExtractDataFromCSV.Cex.Kucoin:
          break;
        case ExtractDataFromCSV.Cex.Binance:
          break;
        case ExtractDataFromCSV.Cex.Okx:
          break;
        default:
          break;
      }
    }
  }
}
