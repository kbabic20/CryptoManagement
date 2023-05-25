using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  class PortfolioManagement
  {

    List<Portfolio> portfolioList = new List<Portfolio>();
    

    public void CreateCryptoRegister()
    {
      GoThroughNetworkTxnData();
    }

    void GoThroughNetworkTxnData()
    {
      string worksheet = "NetzwerkTxnDaten";

      var cellOfDate = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfNetwork = HandleExcel.GetCellByName("Network", worksheet);
      var cellOfNetworkCurrency = HandleExcel.GetCellByName("Network Currency", worksheet);

      int i = 1;

      while(HandleExcel.GetTextFromCell(cellOfDate.cellLine + i , cellOfDate.cellColum, worksheet).Length > 0 )
      {

        string network = HandleExcel.GetTextFromCell(cellOfNetwork.cellLine + i, cellOfNetwork.cellColum, worksheet);
        string networkCurrency = HandleExcel.GetTextFromCell(cellOfNetworkCurrency.cellLine + i, cellOfNetworkCurrency.cellColum, worksheet);

        if (!IsCoinInList(network, networkCurrency))
        {
          if(!IsCoinInExcelRegister(network, networkCurrency))
          {
            //TODo Fehlermeldung Pop Up
            Console.WriteLine("This network '"+ network+ "' with this network currency '" + networkCurrency + "' is not in the Excel Register!! Pls add it manuelly");
          }
        }

        i++;
      }
    }

    bool IsCoinInList(string _name, string _symbol)
    {
      bool isCoinInList = false;
      for (int i = 0; i < portfolioList.Count; i++)
      {
        if (portfolioList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
        {
          if (portfolioList[i].Name.ToLower().Equals(_name.ToLower()))
          {
            isCoinInList = true;
            break;
          }
        }
        
      }
      return isCoinInList;

    }

    bool IsCoinInExcelRegister(string _name, string _symbol)
    {
      bool isCoinInRegister = false;

      string worksheet = "CryptoNetworkData";

      var cellOfName = HandleExcel.GetCellByName("Name", worksheet);
      var cellOfSymbol = HandleExcel.GetCellByName("Symbol", worksheet);
      var cellOfCoinGeckoApiId = HandleExcel.GetCellByName("CoinGecko API ID", worksheet);
      string name = "";
      string symbol = "";
      string coinGeckoApiId = "";
      int i = 1;

      while ((HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet).Length > 0) && !isCoinInRegister)
      {
        name = HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet);
        symbol = HandleExcel.GetTextFromCell(cellOfSymbol.cellLine + i, cellOfSymbol.cellColum, worksheet);
        coinGeckoApiId = HandleExcel.GetTextFromCell(cellOfCoinGeckoApiId.cellLine + i, cellOfCoinGeckoApiId.cellColum, worksheet);

        if (name.ToLower().Equals(_name.ToLower()) && symbol.ToLower().Equals(_symbol.ToLower()))
        {
          isCoinInRegister = true;
        }

        i++;
      }

      if (isCoinInRegister)
      {
        Portfolio portfolio = new Portfolio
        {
          Symbol = symbol,
          Name = name,
          ContractAddress = "N/A",
          CoinGeckoApiID = coinGeckoApiId
        };

        portfolioList.Add(portfolio);
      }

      return isCoinInRegister;
    }
  }
}
