using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  public class CollectData
  {

    public void ExtractDataAndInsertInExcel(string _fileToExtract, string _excelFile)
    {
      Console.WriteLine("-------------ExtractDataAndInsertInExcel-------------");

      ExtractDataFromCSV extractDataFromCSV = new ExtractDataFromCSV();
      extractDataFromCSV.ExtractData(_fileToExtract, ExtractDataFromCSV.Cex.Mxc);
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
  }
}
