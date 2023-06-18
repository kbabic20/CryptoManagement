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
      GoThroughNetworkTokenTxnData();
      Console.WriteLine("CreateCryptoRegister Done");
    }

    void GoThroughNetworkTxnData()
    {

      Console.WriteLine("-------------GoThroughNetworkTxnData-------------");

      string worksheet = "NetzwerkTxnDaten";

      var cellOfDate = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfNetwork = HandleExcel.GetCellByName("Network", worksheet);
      var cellOfNetworkCurrency = HandleExcel.GetCellByName("Network Currency", worksheet);
      var cellOfValueIn = HandleExcel.GetCellByName("Value In", worksheet);
      var cellOfValueOut = HandleExcel.GetCellByName("Value Out", worksheet);
      var cellOfMethod = HandleExcel.GetCellByName("Method", worksheet);
      var cellOfTxnFeeNative = HandleExcel.GetCellByName("TxnFeeNative", worksheet);
      var cellOfStatus = HandleExcel.GetCellByName("Status", worksheet);

      int i = 1;

      while(HandleExcel.GetDateFromCell(cellOfDate.cellLine + i , cellOfDate.cellColum, worksheet).Ticks > 0 )
      {

        string network = HandleExcel.GetTextFromCell(cellOfNetwork.cellLine + i, cellOfNetwork.cellColum, worksheet);
        string networkCurrency = HandleExcel.GetTextFromCell(cellOfNetworkCurrency.cellLine + i, cellOfNetworkCurrency.cellColum, worksheet);
        string status = HandleExcel.GetTextFromCell(cellOfStatus.cellLine + i, cellOfStatus.cellColum, worksheet);
        string method = HandleExcel.GetTextFromCell(cellOfMethod.cellLine + i, cellOfMethod.cellColum, worksheet);
        decimal valueIn = (decimal) HandleExcel.GetDecimalFromCell(cellOfValueIn.cellLine + i, cellOfValueIn.cellColum, worksheet);
        decimal valueOut = (decimal) HandleExcel.GetDecimalFromCell(cellOfValueOut.cellLine + i, cellOfValueOut.cellColum, worksheet);
        decimal txnFeeNative = (decimal)HandleExcel.GetDecimalFromCell(cellOfTxnFeeNative.cellLine + i, cellOfTxnFeeNative.cellColum, worksheet);

        decimal amount = 0;
        if (!IsCoinInList(network, networkCurrency, "", true))
        {
          if(!IsCoinInExcelRegister(network, networkCurrency, "", true))
          {
            //TODo Fehlermeldung Pop Up
            Console.WriteLine("This network '"+ network+ "' with this network currency '" + networkCurrency + "' is not in the Excel Register!! Pls add it manuelly");
          }
          else
          {
            int coinIndexPortfolio = GetIndexOfCoinInPortfolio(network, networkCurrency, "", true);

            if (coinIndexPortfolio != -1)
            {
              

              if (valueIn > 0)
              {
                amount = valueIn;

              }
              else if (valueOut > 0 )
              {
                if (status.Contains("Error"))
                {
                  amount = txnFeeNative * (-1);
                }
                else
                {
                  amount = valueOut * (-1) - txnFeeNative;
                }
                
              }


              portfolioList[coinIndexPortfolio].AmountHolding += amount;
            }
          }
        }
        else
        {
          int coinIndexPortfolio = GetIndexOfCoinInPortfolio(network, networkCurrency, "", true);

          if (coinIndexPortfolio != -1)
          {

            if (valueIn > 0)
            {
              amount = valueIn;

            }
            else if (valueOut > 0)
            {
              if (!(status is null))
              {
                if (status.Contains("Error"))
                {
                  amount = txnFeeNative * (-1);
                }
                else
                {
                  amount = valueOut * (-1) - txnFeeNative;
                }
              }
              else
              {
                amount = valueOut * (-1) - txnFeeNative;
              }

            }

            portfolioList[coinIndexPortfolio].AmountHolding += amount;
          }
        }

        i++;
      }
    }

    bool IsCoinInList(string _name, string _symbol, string _contractAddress, bool _isNetworkCoin)
    {
      bool isCoinInList = false;

      if (_contractAddress.Length > 0)
      {
        for (int i = 0; i < portfolioList.Count; i++)
        {
          if (portfolioList[i].ContractAddress.Equals(_contractAddress))
          {
            isCoinInList = true;
            break;
          }
        }
      }
      else
      {
        for (int i = 0; i < portfolioList.Count; i++)
        {
          if (_isNetworkCoin)
          {
            if (portfolioList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
            {
              if (portfolioList[i].Network.ToLower().Equals(_name.ToLower()))
              {
                isCoinInList = true;
                break;
              }
            }
          }
          else
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
          

        }
      }
      
      return isCoinInList;

    }

    bool IsCoinInExcelRegister(string _name, string _symbol, string _contractAddress, bool _isNetworkCoin)
    {
      bool isCoinInRegister = false;

      string worksheet = "CryptoRegister";

      var cellOfContractAddress = HandleExcel.GetCellByName("Contract Address", worksheet);
      var cellOfName = HandleExcel.GetCellByName("Name", worksheet);
      var cellOfSymbol = HandleExcel.GetCellByName("Symbol", worksheet);
      var cellOfNetwork = HandleExcel.GetCellByName("Network", worksheet);
      var cellOfCoinGeckoApiId = HandleExcel.GetCellByName("CoinGecko API ID", worksheet);

      string contractAddress = "N/A";
      string name = "";
      string symbol = "";
      string network = "";
      string coinGeckoApiId = "";
      int i = 1;

      while ((HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet) != null) && !isCoinInRegister)
      {
        if (HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet).Length == 0)
        {
          break;
        }

        contractAddress = HandleExcel.GetTextFromCell(cellOfContractAddress.cellLine + i, cellOfContractAddress.cellColum, worksheet);
        name = HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet);
        symbol = HandleExcel.GetTextFromCell(cellOfSymbol.cellLine + i, cellOfSymbol.cellColum, worksheet);
        network = HandleExcel.GetTextFromCell(cellOfNetwork.cellLine + i, cellOfNetwork.cellColum, worksheet);
        coinGeckoApiId = HandleExcel.GetTextFromCell(cellOfCoinGeckoApiId.cellLine + i, cellOfCoinGeckoApiId.cellColum, worksheet);

        if (!(contractAddress is null))
        {
          if (_contractAddress.Length > 0)
          {
            if (contractAddress.Equals(_contractAddress))
            {
              isCoinInRegister = true;
              break;
            }
          }
        }

        if (_isNetworkCoin)
        {
        if(network.ToLower().Equals(_name.ToLower()) && symbol.ToLower().Equals(_symbol.ToLower()))
        {
            isCoinInRegister = true;
            break;
          }
        }
        else
        {
          if (name.ToLower().Equals(_name.ToLower()) && symbol.ToLower().Equals(_symbol.ToLower()))
          {
            if (!(contractAddress is null))
            {
              if (contractAddress.Length.Equals(0) && _contractAddress.Length > 0)
              {
                HandleExcel.SetTextInCell(_contractAddress, cellOfContractAddress.cellLine + i, cellOfContractAddress.cellColum, worksheet);
              }
            }

            isCoinInRegister = true;
            break;
          }
        }

        i++;
      }

      if (isCoinInRegister)
      {
        if (_isNetworkCoin)
        {
          Portfolio portfolio = new Portfolio
          {
            Symbol = symbol,
            Network = network,
            Name = name,
            ContractAddress = contractAddress,
            CoinGeckoApiID = coinGeckoApiId
          };

          portfolioList.Add(portfolio);
        }
        else
        {
          Portfolio portfolio = new Portfolio
          {
            Symbol = symbol,
            Name = name,
            ContractAddress = contractAddress,
            CoinGeckoApiID = coinGeckoApiId
          };

          portfolioList.Add(portfolio);
        }
      }

      return isCoinInRegister;
    }

    int GetIndexOfCoinInPortfolio(string _name, string _symbol, string _contractAddress, bool _isNetworkCoin)
    {
      int index = -1;
      if (_contractAddress.Length > 0)
      {
        for (int i = 0; i < portfolioList.Count; i++)
        {
          if (portfolioList[i].ContractAddress.Equals(_contractAddress))
          {
            index = i;
            break;
          }
        }
      }
      else
      {
        for (int i = 0; i < portfolioList.Count; i++)
        {
          if (_isNetworkCoin)
          {
            if (portfolioList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
            {
              if (portfolioList[i].Network.ToLower().Equals(_name.ToLower()))
              {
                index = i;
                break;
              }
            }
          }
          else
          {
            if (portfolioList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
            {
              if (portfolioList[i].Name.ToLower().Equals(_name.ToLower()))
              {
                index = i;
                break;
              }
            }
          }
        }
      }
      
      return index;
    }
    void GoThroughNetworkTokenTxnData()
    {
      Console.WriteLine("-------------GoThroughNetworkTokenTxnData-------------");

      string worksheet = "NetzwerkTokenTxnDaten";

      var cellOfDate = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfNetwork = HandleExcel.GetCellByName("Network", worksheet);
      var cellOfNetworkCurrency = HandleExcel.GetCellByName("Network Currency", worksheet);
      var cellOfFrom = HandleExcel.GetCellByName("From", worksheet);
      var cellOfTo = HandleExcel.GetCellByName("To", worksheet);
      var cellOfTokenAmount = HandleExcel.GetCellByName("Token Amount", worksheet);
      var cellOfContractAddress = HandleExcel.GetCellByName("Contract Address", worksheet);
      var cellOfTokenName = HandleExcel.GetCellByName("Token Name", worksheet);
      var cellOfTokenSymbol = HandleExcel.GetCellByName("Token Symbol", worksheet);

      int i = 1;

      while (HandleExcel.GetDateFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet).Ticks > 0)
      {

        string network = HandleExcel.GetTextFromCell(cellOfNetwork.cellLine + i, cellOfNetwork.cellColum, worksheet);
        string networkCurrency = HandleExcel.GetTextFromCell(cellOfNetworkCurrency.cellLine + i, cellOfNetworkCurrency.cellColum, worksheet);
        string walletFrom = HandleExcel.GetTextFromCell(cellOfFrom.cellLine + i, cellOfFrom.cellColum, worksheet);
        string walletTo = HandleExcel.GetTextFromCell(cellOfTo.cellLine + i, cellOfTo.cellColum, worksheet);
        decimal tokenAmount = (decimal) HandleExcel.GetDecimalFromCell(cellOfTokenAmount.cellLine + i, cellOfTokenAmount.cellColum, worksheet);
        string contractAddress = HandleExcel.GetTextFromCell(cellOfContractAddress.cellLine + i, cellOfContractAddress.cellColum, worksheet);
        string tokenName = HandleExcel.GetTextFromCell(cellOfTokenName.cellLine + i, cellOfTokenName.cellColum, worksheet);
        string tokenSymbol = HandleExcel.GetTextFromCell(cellOfTokenSymbol.cellLine + i, cellOfTokenSymbol.cellColum, worksheet);

        decimal amount = 0;

        if (!IsMyWallet(walletFrom) && IsMyWallet(walletTo))
        {
          // Token was added to my wallets

          amount = tokenAmount;
        }
        else if (IsMyWallet(walletFrom) && !IsMyWallet(walletTo))
        {
          // Token was removed to my wallets

          amount = tokenAmount * (-1);
        }


        if (!IsCoinInList(tokenName, tokenSymbol, contractAddress, false))
        {
          if (!IsCoinInExcelRegister(tokenName, tokenSymbol, contractAddress, false))
          {
            //TODo Fehlermeldung Pop Up
            Console.WriteLine("This Token '" + tokenName + "' with this token symbol '" + tokenSymbol + "' is not in the Excel Register!! Pls add it manuelly");
          }
          else
          {
            int coinIndexPortfolio = GetIndexOfCoinInPortfolio(network, networkCurrency, contractAddress, false);

            if (coinIndexPortfolio != -1)
            {
              portfolioList[coinIndexPortfolio].AmountHolding += amount;
            }
          }
        }
        else
        {
          int coinIndexPortfolio = GetIndexOfCoinInPortfolio(network, networkCurrency, contractAddress, false);

          if (coinIndexPortfolio != -1)
          {

            portfolioList[coinIndexPortfolio].AmountHolding += amount;
          }
        }

        i++;
      }
    }

    bool IsMyWallet(string _walletAddress)
    {
      string worksheet = "MyWallets";

      var cellOfWalletAddress = HandleExcel.GetCellByName("Wallet Address", worksheet);
      var cellOfWalletName = HandleExcel.GetCellByName("Wallet Name", worksheet);

      string walletAddress = "";
      string walletName = "";
      int i = 1;

      while (HandleExcel.GetTextFromCell(cellOfWalletAddress.cellLine + i, cellOfWalletAddress.cellColum, worksheet) != null)
      {
        if (HandleExcel.GetTextFromCell(cellOfWalletAddress.cellLine + i, cellOfWalletAddress.cellColum, worksheet).Length == 0 )
        {
          break;
        }
        walletAddress = HandleExcel.GetTextFromCell(cellOfWalletAddress.cellLine + i, cellOfWalletAddress.cellColum, worksheet);
        walletName = HandleExcel.GetTextFromCell(cellOfWalletName.cellLine + i, cellOfWalletName.cellColum, worksheet);

        if (walletAddress.Equals(_walletAddress))
        {
          return true;
        }

        i++;
      }

      return false;
    }
  }
}
