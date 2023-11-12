using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  class PortfolioManagement
  {

    class DataInfo
    {
      public string DateOfTxn { get; set; }
      public string CoinName { get; set; }
      public string CoinSymbol { get; set; }
      public string ContractAddress { get; set; }
      public string CoinGeckoApiID { get; set; }
      public string Network { get; set; }
      public decimal AmountOfCoin { get; set; }
      public decimal AvgBuyPrice { get; set; }
      public decimal AvgGain { get; set; }
      public decimal CurrPrice { get; set; }
      public string DepotName { get; set; }
    }


    List<Portfolio> portfolioList = new List<Portfolio>();
    List<DepotInventory> depotInventoryList = new List<DepotInventory>();
    List<MyWallet> myWalletsList = new List<MyWallet>();
    List<CryptoRegisterData> cryptoRegisterDataList = new List<CryptoRegisterData>();
    public List<string> txhashToIgnoreList = new List<string>();


    public void CreateCryptoRegister()
    {
      CollectExcelData();

      GoThroughNetworkTxnData();
      GoThroughNetworkTokenTxnData();
     // GoThroughCexBuySellData();
      WriteNewCryptoRegisterDataIntoExcel();
      WritePortfolioIntoExcel();
      Console.WriteLine("CreateCryptoRegister Done");
    }

    void CollectExcelData()
    {
      SaveMyWalletsData();
      SaveCryptoRegisterData();
    }

    void SaveMyWalletsData()
    {
      Console.WriteLine("-------------SaveMyWalletsData-------------");

      string worksheet = "MyWallets";

      var cellOfWalletAddress = HandleExcel.GetCellByName("Wallet Address", worksheet);
      var cellOfWalletName = HandleExcel.GetCellByName("Wallet Name", worksheet);

      string walletAddress = "";
      string walletName = "";
      int i = 1;

      while (HandleExcel.GetTextFromCell(cellOfWalletAddress.cellLine + i, cellOfWalletAddress.cellColum, worksheet) != null)
      {
        walletAddress = HandleExcel.GetTextFromCell(cellOfWalletAddress.cellLine + i, cellOfWalletAddress.cellColum, worksheet);

        if (HandleExcel.GetTextFromCell(cellOfWalletAddress.cellLine + i, cellOfWalletAddress.cellColum, worksheet).Length == 0)
        {
          break;
        }

        walletName = HandleExcel.GetTextFromCell(cellOfWalletName.cellLine + i, cellOfWalletName.cellColum, worksheet);

        MyWallet myWallet = new MyWallet
        {
          Address = walletAddress,
          Name = walletName
        };

        myWalletsList.Add(myWallet);

        i++;
      }

    }


    void SaveCryptoRegisterData()
    {

      Console.WriteLine("-------------SaveCryptoRegisterData-------------");

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

      while ((HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet) != null) )
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

        CryptoRegisterData cryptoRegisterData = new CryptoRegisterData
        {
          ContractAddress = contractAddress,
          Name = name,
          Symbol = symbol,
          Network = network,
          CoinGeckoApiID = coinGeckoApiId
        };

        cryptoRegisterDataList.Add(cryptoRegisterData);

        i++;
      }

    }

    void GoThroughNetworkTxnData()
    {

      Console.WriteLine("-------------GoThroughNetworkTxnData-------------");

      string worksheet = "NetzwerkTxnDaten";

      var cellOfDate = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfNetwork = HandleExcel.GetCellByName("Network", worksheet);
      var cellOfTxhash = HandleExcel.GetCellByName("Txhash", worksheet);
      var cellOfNetworkCurrency = HandleExcel.GetCellByName("Network Currency", worksheet);
      var cellOfFrom = HandleExcel.GetCellByName("From", worksheet);
      var cellOfTo = HandleExcel.GetCellByName("To", worksheet);
      var cellOfValueIn = HandleExcel.GetCellByName("Value In", worksheet);
      var cellOfValueOut = HandleExcel.GetCellByName("Value Out", worksheet);
      var cellOfMethod = HandleExcel.GetCellByName("Method", worksheet);
      var cellOfTxnFeeNative = HandleExcel.GetCellByName("TxnFeeNative", worksheet);
      var cellOfStatus = HandleExcel.GetCellByName("Status", worksheet);

      int i = 1;

      string walletName = "";
      int depotListIndex;
      int coinDepotListIndex;

      DateTime date = new DateTime();

      while (HandleExcel.GetTextFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet).Length > 0) //HandleExcel.GetDateFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet).Ticks > 0 )
      {
        date = DateTime.Parse(HandleExcel.GetTextFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet));//HandleExcel.GetDateFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet);

        string network = HandleExcel.GetTextFromCell(cellOfNetwork.cellLine + i, cellOfNetwork.cellColum, worksheet);
        string networkCurrency = HandleExcel.GetTextFromCell(cellOfNetworkCurrency.cellLine + i, cellOfNetworkCurrency.cellColum, worksheet);
        string txhash = HandleExcel.GetTextFromCell(cellOfTxhash.cellLine + i, cellOfTxhash.cellColum, worksheet);
        string status = HandleExcel.GetTextFromCell(cellOfStatus.cellLine + i, cellOfStatus.cellColum, worksheet);
        string method = HandleExcel.GetTextFromCell(cellOfMethod.cellLine + i, cellOfMethod.cellColum, worksheet);
        string walletFrom = HandleExcel.GetTextFromCell(cellOfFrom.cellLine + i, cellOfFrom.cellColum, worksheet);
        string walletTo = HandleExcel.GetTextFromCell(cellOfTo.cellLine + i, cellOfTo.cellColum, worksheet);
        decimal valueIn = (decimal) HandleExcel.GetDecimalFromCell(cellOfValueIn.cellLine + i, cellOfValueIn.cellColum, worksheet);
        decimal valueOut = (decimal) HandleExcel.GetDecimalFromCell(cellOfValueOut.cellLine + i, cellOfValueOut.cellColum, worksheet);
        decimal txnFeeNative = (decimal)HandleExcel.GetDecimalFromCell(cellOfTxnFeeNative.cellLine + i, cellOfTxnFeeNative.cellColum, worksheet);

        DataInfo dataInfo = new DataInfo
        {
          DateOfTxn = date.ToString(),
          CoinSymbol = networkCurrency,
          Network = network
        };

        //if (method.Equals("Transfer"))
        //{
        //  txhashToIgnoreList.Add(txhash);
        //}

        decimal amount = 0;

        if (networkCurrency == "ETH")
        {
          Console.WriteLine("test");
        }
      

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

              if (valueIn > 0)// && !method.Equals("Transfer"))
              {
                amount = valueIn;
                dataInfo.DepotName = GetMyWalletName(walletTo);

              }
              else if (valueOut > 0)//  && !method.Equals("Transfer"))
              {
                if (status.Contains("Error"))
                {
                  amount = txnFeeNative * (-1);
                }
                else
                {
                  amount = valueOut * (-1) - txnFeeNative;
                }

                dataInfo.DepotName = GetMyWalletName(walletFrom);
              }
              else
              {
                amount = txnFeeNative * (-1);
                dataInfo.DepotName = GetMyWalletName(walletFrom);
              }

              dataInfo.AmountOfCoin = amount;

              portfolioList[coinIndexPortfolio].AmountHolding += amount;

              AddCoinToDepotList(dataInfo);

            }
          }
        }
        else
        {
          int coinIndexPortfolio = GetIndexOfCoinInPortfolio(network, networkCurrency, "", true);

          if (coinIndexPortfolio != -1)
          {


            if (valueIn == 0.2988m)
            {
              Console.WriteLine("test");
            }
            if (valueIn > 0)//  && !method.Equals("Transfer"))
            {
              amount = valueIn; 
              dataInfo.DepotName = GetMyWalletName(walletTo);

            }
            else if (valueOut > 0)//  && !method.Equals("Transfer"))
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

              dataInfo.DepotName = GetMyWalletName(walletFrom);
            }
            else
            {
              amount = txnFeeNative * (-1);
              dataInfo.DepotName = GetMyWalletName(walletFrom);
            }

            dataInfo.AmountOfCoin = amount;

            portfolioList[coinIndexPortfolio].AmountHolding += amount;


            AddCoinToDepotList(dataInfo);
          }
        }

        i++;
      }
    }

    void AddCoinToDepotList(DataInfo _dataInfo)
    {

      int depotListIndex;
      int coinDepotListIndex;

      if (IsDepotInList(_dataInfo.DepotName, out depotListIndex))
      {
        if (IsCoinInDepot(_dataInfo, depotListIndex, out coinDepotListIndex))
        {
          depotInventoryList[depotListIndex].CoinInfos[coinDepotListIndex].AmountHolding += _dataInfo.AmountOfCoin;
        }
        else
        {
          CoinInfo coinInfo = new CoinInfo
          {
            Symbol = _dataInfo.CoinSymbol,
            Network = _dataInfo.Network,
            AmountHolding = _dataInfo.AmountOfCoin,
            ContractAddress = _dataInfo.ContractAddress,
            Name = _dataInfo.CoinName
          };

          depotInventoryList[depotListIndex].CoinInfos.Add(coinInfo);
        }

      }
      else
      {
        DepotInventory depotInventory = new DepotInventory
        {

          DepotName = _dataInfo.DepotName,
          CoinInfos = new List<CoinInfo>()

        };

        depotInventoryList.Add(depotInventory);

        CoinInfo coinInfo = new CoinInfo
        {
          Symbol = _dataInfo.CoinSymbol,
          Network = _dataInfo.Network,
          AmountHolding = _dataInfo.AmountOfCoin,
          ContractAddress = _dataInfo.ContractAddress,
          Name = _dataInfo.CoinName
        };

        depotInventoryList[depotInventoryList.Count -1].CoinInfos.Add(coinInfo);
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
              //if (portfolioList[i].Network.ToLower().Equals(_name.ToLower()))
              //{
              //  isCoinInList = true;
              //  break;
              //}
              isCoinInList = true;
              break;
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
    bool IsCoinInList(string _symbol)
    {
      bool isCoinInList = false;

      for (int i = 0; i < portfolioList.Count; i++)
      {
        if (portfolioList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
        {
          isCoinInList = true;
          break;
        }

      }

      return isCoinInList;

    }

    bool IsCoinInExcelRegister(string _name, string _symbol, string _contractAddress, bool _isNetworkCoin)
    {
      bool isCoinInRegister = false;
      int index = -1;

      for (int  i = 0;  i < cryptoRegisterDataList.Count;  i++)
      {
        if (_contractAddress.Length > 0) 
        {
          if (!(cryptoRegisterDataList[i].ContractAddress is null))
          {
            if (cryptoRegisterDataList[i].ContractAddress.Equals(_contractAddress))
            {
              isCoinInRegister = true;
              index = i;
              break;
            }
          }
        }
        else
        {
          if (_isNetworkCoin)
          {
            if (cryptoRegisterDataList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
                //&& cryptoRegisterDataList[i].Network.ToLower().Equals(_name.ToLower()))
            {
              isCoinInRegister = true;
              index = i;
              break;
            }
          }
          else
          {
            if (cryptoRegisterDataList[i].Name.ToLower().Equals(_name.ToLower()) && cryptoRegisterDataList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
            {
              if (!(cryptoRegisterDataList[i].ContractAddress is null))
              {
                if (cryptoRegisterDataList[i].ContractAddress.Length.Equals(0) && _contractAddress.Length > 0)
                {
                  cryptoRegisterDataList[i].ContractAddress = _contractAddress;
                }
              }

              isCoinInRegister = true;
              index = i;
              break;
            }
          }
        }

        

      }

      if (isCoinInRegister)
      {
        //if (_isNetworkCoin)
        //{
        //  Portfolio portfolio = new Portfolio
        //  {
        //    Symbol = cryptoRegisterDataList[index].Symbol,
        //    Network = cryptoRegisterDataList[index].Network,
        //    Name = cryptoRegisterDataList[index].Name,
        //    ContractAddress = cryptoRegisterDataList[index].ContractAddress,
        //    CoinGeckoApiID = cryptoRegisterDataList[index].CoinGeckoApiID
        //  };

        //  portfolioList.Add(portfolio);
        //}
        //else
        //{
        //  Portfolio portfolio = new Portfolio
        //  {
        //    Symbol = cryptoRegisterDataList[index].Symbol,
        //    Name = cryptoRegisterDataList[index].Name,
        //    ContractAddress = cryptoRegisterDataList[index].ContractAddress,
        //    CoinGeckoApiID = cryptoRegisterDataList[index].CoinGeckoApiID
        //  };

        //  portfolioList.Add(portfolio);
        //}

        Portfolio portfolio = new Portfolio
        {
          Symbol = cryptoRegisterDataList[index].Symbol,
          Network = cryptoRegisterDataList[index].Network,
          Name = cryptoRegisterDataList[index].Name,
          ContractAddress = cryptoRegisterDataList[index].ContractAddress,
          CoinGeckoApiID = cryptoRegisterDataList[index].CoinGeckoApiID
        };

        portfolioList.Add(portfolio);
      }

      return isCoinInRegister;

    }
    bool IsCoinInExcelRegister(string _symbol)
    {
      bool isCoinInRegister = false;
      int index = -1;

      for (int i = 0; i < cryptoRegisterDataList.Count; i++)
      {

        if (cryptoRegisterDataList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
        {
          isCoinInRegister = true;
          index = i;
          break;
        }

      }

      if (isCoinInRegister)
      {

        Portfolio portfolio = new Portfolio
        {
          Symbol = cryptoRegisterDataList[index].Symbol,
          Network = cryptoRegisterDataList[index].Network,
          Name = cryptoRegisterDataList[index].Name,
          ContractAddress = cryptoRegisterDataList[index].ContractAddress,
          CoinGeckoApiID = cryptoRegisterDataList[index].CoinGeckoApiID
        };

        portfolioList.Add(portfolio);
      }

      return isCoinInRegister;

    }

    bool IsDepotInList(string _depotName, out int _index)
    {
      for (int i = 0; i < depotInventoryList.Count; i++)
      {
        if (depotInventoryList[i].DepotName.Equals(_depotName))
        {
          _index = i;
          return true;
        }
      }
      _index = -1;
      return false;
    }

    bool IsCoinInDepot(DataInfo _dataInfo, int _depotIndex, out int _coinIndex)
    {
      _coinIndex = -1;

      if (_dataInfo.CoinName is null && _dataInfo.ContractAddress is null &&!(_dataInfo.CoinSymbol is null) && !(_dataInfo.Network is null))
      {
        // NetworkTxn Data
        return IsCoinInDepot(_dataInfo.Network, _dataInfo.CoinSymbol, "", true, _depotIndex, out _coinIndex);
      }
      else if (_dataInfo.CoinName is null && _dataInfo.ContractAddress is null && _dataInfo.ContractAddress is null)
      {
        // CEX Data
        return IsCoinInDepot(_dataInfo.CoinSymbol, _depotIndex, out _coinIndex);
      }
      else
      {
        // NetworkTokenTxn Data
        return IsCoinInDepot(_dataInfo.CoinName, _dataInfo.CoinSymbol, _dataInfo.ContractAddress, false, _depotIndex, out _coinIndex);
      }

    }
    bool IsCoinInDepot(string _name, string _symbol, string _contractAddress, bool _isNetworkCoin, int depotIndex,out int _coinIndex)

    {
      bool isCoinInList = false;
      _coinIndex = -1;

      if (_contractAddress.Length > 0)
      {
        for (int i = 0; i < depotInventoryList[depotIndex].CoinInfos.Count; i++)
        {
          if (!(depotInventoryList[depotIndex].CoinInfos[i].ContractAddress is null))
          {
            if (depotInventoryList[depotIndex].CoinInfos[i].ContractAddress.Equals(_contractAddress))
            {
              isCoinInList = true;
              _coinIndex = i;
              break;
            }
          }
          
        }
      }
      else
      {
        for (int i = 0; i < depotInventoryList[depotIndex].CoinInfos.Count; i++)
        {
          if (_isNetworkCoin)
          {
            if (depotInventoryList[depotIndex].CoinInfos[i].Symbol.ToLower().Equals(_symbol.ToLower()))
            {
              //if (depotInventoryList[depotIndex].CoinInfos[i].Network.ToLower().Equals(_name.ToLower()))
              //{
              //  isCoinInList = true;
              //  _coinIndex = i;
              //  break;
              //}
              isCoinInList = true;
              _coinIndex = i;
              break;
            }
          }
          else
          {
            if (depotInventoryList[depotIndex].CoinInfos[i].Symbol.ToLower().Equals(_symbol.ToLower()))
            {
              if (depotInventoryList[depotIndex].CoinInfos[i].Name.ToLower().Equals(_name.ToLower()))
              {
                isCoinInList = true;
                _coinIndex = i;
                break;
              }
            }
          }


        }
      }

      return isCoinInList;

    }
    bool IsCoinInDepot( string _symbol, int depotIndex, out int _coinIndex)

    {
      bool isCoinInList = false;

      _coinIndex = -1;

      for (int i = 0; i < depotInventoryList[depotIndex].CoinInfos.Count; i++)
      {
        if (depotInventoryList[depotIndex].CoinInfos[i].Symbol.ToLower().Equals(_symbol.ToLower()))
        {
          isCoinInList = true;
          _coinIndex = i;
          break;
        }

      }

      return isCoinInList;

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
              //if (portfolioList[i].Network.ToLower().Equals(_name.ToLower()))
              //{
              //  index = i;
              //  break;
              //}
              index = i;
              break;
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
    int GetIndexOfCoinInPortfolio(string _symbol)
    {
      int index = -1;

      for (int i = 0; i < portfolioList.Count; i++)
      {
        if (portfolioList[i].Symbol.ToLower().Equals(_symbol.ToLower()))
        {
          index = i;
          break;
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
      var cellOfTxhash = HandleExcel.GetCellByName("Txhash", worksheet);
      var cellOfFrom = HandleExcel.GetCellByName("From", worksheet);
      var cellOfTo = HandleExcel.GetCellByName("To", worksheet);
      var cellOfTokenAmount = HandleExcel.GetCellByName("Token Amount", worksheet);
      var cellOfContractAddress = HandleExcel.GetCellByName("Contract Address", worksheet);
      var cellOfTokenName = HandleExcel.GetCellByName("Token Name", worksheet);
      var cellOfTokenSymbol = HandleExcel.GetCellByName("Token Symbol", worksheet);

      int i = 1;

      DateTime date = new DateTime();

      while (HandleExcel.GetTextFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet).Length > 0) //HandleExcel.GetDateFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet).Ticks > 0 )
      {

        date = DateTime.Parse(HandleExcel.GetTextFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet));//HandleExcel.GetDateFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet);

        string network = HandleExcel.GetTextFromCell(cellOfNetwork.cellLine + i, cellOfNetwork.cellColum, worksheet);
        string networkCurrency = HandleExcel.GetTextFromCell(cellOfNetworkCurrency.cellLine + i, cellOfNetworkCurrency.cellColum, worksheet);
        string txhash = HandleExcel.GetTextFromCell(cellOfTxhash.cellLine + i, cellOfTxhash.cellColum, worksheet);
        string walletFrom = HandleExcel.GetTextFromCell(cellOfFrom.cellLine + i, cellOfFrom.cellColum, worksheet);
        string walletTo = HandleExcel.GetTextFromCell(cellOfTo.cellLine + i, cellOfTo.cellColum, worksheet);
        decimal tokenAmount = (decimal) HandleExcel.GetDecimalFromCell(cellOfTokenAmount.cellLine + i, cellOfTokenAmount.cellColum, worksheet);
        string contractAddress = HandleExcel.GetTextFromCell(cellOfContractAddress.cellLine + i, cellOfContractAddress.cellColum, worksheet);
        string tokenName = HandleExcel.GetTextFromCell(cellOfTokenName.cellLine + i, cellOfTokenName.cellColum, worksheet);
        string tokenSymbol = HandleExcel.GetTextFromCell(cellOfTokenSymbol.cellLine + i, cellOfTokenSymbol.cellColum, worksheet);

        decimal amount = 0;


        //if (txhashToIgnoreList.Contains(txhash))
        //{
        //  i++;
        //  continue;
        //}

        DataInfo dataInfo = new DataInfo
        {
          DateOfTxn = date.ToString(),
          CoinSymbol = tokenSymbol,
          Network = network,
          CoinName = tokenName,
          ContractAddress = contractAddress
          
        };


        if (!IsMyWallet(walletFrom) && IsMyWallet(walletTo))
        {
          // Token was added to my wallets

          amount = tokenAmount;
          dataInfo.DepotName = GetMyWalletName(walletTo);
        }
        else if (IsMyWallet(walletFrom) && !IsMyWallet(walletTo))
        {
          // Token was removed to my wallets

          amount = tokenAmount * (-1);
          dataInfo.DepotName = GetMyWalletName(walletFrom);
        }


        if (true)
        {

        }

        if (!IsCoinInList(tokenName, tokenSymbol, contractAddress, false))
        {
          if (!IsCoinInExcelRegister(tokenName, tokenSymbol, contractAddress, false))
          {
            //TODo Fehlermeldung Pop Up
            Console.WriteLine("This Token '" + tokenName + "' with this token symbol '" + tokenSymbol + "' in this network '" + network + "' in this network '" + network + "' in this network this contrac adress'" + contractAddress + "' is not in the Excel Register!! Pls add it manuelly");
          }
          else
          {
            int coinIndexPortfolio = GetIndexOfCoinInPortfolio(network, networkCurrency, contractAddress, false);

            if (coinIndexPortfolio != -1)
            {
              portfolioList[coinIndexPortfolio].AmountHolding += amount;

              dataInfo.AmountOfCoin = amount;

              AddCoinToDepotList(dataInfo);
            }
          }
        }
        else
        {
          int coinIndexPortfolio = GetIndexOfCoinInPortfolio(network, networkCurrency, contractAddress, false);

          if (coinIndexPortfolio != -1)
          {

            portfolioList[coinIndexPortfolio].AmountHolding += amount;

            dataInfo.AmountOfCoin = amount;

            AddCoinToDepotList(dataInfo);
          }
        }

        i++;
      }
    }

    bool IsMyWallet(string _walletAddress)
    {

      //Console.WriteLine("-------------IsMyWallet-------------");

      for (int i = 0; i < myWalletsList.Count; i++)
      {
        if (myWalletsList[i].Address.Equals(_walletAddress))
        {
          return true;
        }
      }

      return false;
    }
    string GetMyWalletName(string _walletAddress)
    {

      //Console.WriteLine("-------------IsMyWallet-------------");

      for (int i = 0; i < myWalletsList.Count; i++)
      {
        if (myWalletsList[i].Address.Equals(_walletAddress))
        {
          return myWalletsList[i].Name;
        }
      }

      return "";
    }

    void WriteNewCryptoRegisterDataIntoExcel()
    {

      Console.WriteLine("-------------WriteNewCryptoRegisterDataIntoExcel-------------");

      string worksheet = "CryptoRegister";

      var cellOfContractAddress = HandleExcel.GetCellByName("Contract Address", worksheet);
      var cellOfName = HandleExcel.GetCellByName("Name", worksheet);
      var cellOfSymbol = HandleExcel.GetCellByName("Symbol", worksheet);
      var cellOfNetwork = HandleExcel.GetCellByName("Network", worksheet);

      string name = "";
      string symbol = "";
      string network = "";
      string contractAddress = "";

      int i = 1;

      while ((HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet) != null))
      {
        name = HandleExcel.GetTextFromCell(cellOfName.cellLine + i, cellOfName.cellColum, worksheet);

        if (name.Length == 0)
        {
          break;
        }

        symbol = HandleExcel.GetTextFromCell(cellOfSymbol.cellLine + i, cellOfSymbol.cellColum, worksheet);
        network = HandleExcel.GetTextFromCell(cellOfNetwork.cellLine + i, cellOfNetwork.cellColum, worksheet);
        contractAddress = HandleExcel.GetTextFromCell(cellOfContractAddress.cellLine + i, cellOfContractAddress.cellColum, worksheet);

        if (contractAddress.Length == 0)
        {
          for (int j = 0; j < cryptoRegisterDataList.Count; j++)
          {
            if (cryptoRegisterDataList[j].ContractAddress.Length > 0)
            {

              if ((network.ToLower().Equals(cryptoRegisterDataList[j].Network.ToLower())
              && symbol.ToLower().Equals(cryptoRegisterDataList[j].Symbol.ToLower()))
              || (name.ToLower().Equals(cryptoRegisterDataList[j].Name.ToLower())
              && symbol.ToLower().Equals(cryptoRegisterDataList[j].Symbol.ToLower())))
              {
                HandleExcel.SetTextInCell(cryptoRegisterDataList[j].ContractAddress, cellOfContractAddress.cellLine + i, cellOfContractAddress.cellColum, worksheet);
                break;
              }

            }
            else
            {
              continue;
            }
          }
        }

        i++;
      }



    }
    void GoThroughCexBuySellData()
    {

      Console.WriteLine("-------------GoThroughCexBuySellData-------------");

      string worksheet = "Cex Kauf&Verkauf";



      var cellOfDate = HandleExcel.GetCellByName("Datum", worksheet);
      var cellOfPair = HandleExcel.GetCellByName("Pair", worksheet);
      var cellOfBuy_Sell = HandleExcel.GetCellByName("Kauf/Verkauf?", worksheet);
      var cellOfDepot = HandleExcel.GetCellByName("Depot", worksheet);
      var cellOfRecievedAmount = HandleExcel.GetCellByName("Stückzahl", worksheet);
      var cellOfPrice = HandleExcel.GetCellByName("Preis pro Stück beim Kauf/Verkauf (bezogen auf das Pair)", worksheet);
      var cellOfFee = HandleExcel.GetCellByName("Gebühren Preis", worksheet);
      var cellOfFeeCurr = HandleExcel.GetCellByName("Gebühren Währung", worksheet);
      var cellOfAmountInvested = HandleExcel.GetCellByName("Gezahlt/Bekommen insgesamt (bezogen auf das Pair)", worksheet);
      var cellOfAmountInvestedAfterFee = HandleExcel.GetCellByName("Gezahlt/Bekommen ohne Gebühr (bezogen auf das Pair)", worksheet);

      int i = 1;

      DateTime date = new DateTime();

      while (HandleExcel.GetTextFromCell(cellOfDepot.cellLine + i, cellOfDepot.cellColum, worksheet).Length > 0)
      {

       // date = HandleExcel.GetDateFromCell(cellOfDate.cellLine + i, cellOfDate.cellColum, worksheet);

        string pair = HandleExcel.GetTextFromCell(cellOfPair.cellLine + i, cellOfPair.cellColum, worksheet);
        string buy_Sell = HandleExcel.GetTextFromCell(cellOfBuy_Sell.cellLine + i, cellOfBuy_Sell.cellColum, worksheet);
        string depot = HandleExcel.GetTextFromCell(cellOfDepot.cellLine + i, cellOfDepot.cellColum, worksheet);
        decimal recievedAmount = (decimal)HandleExcel.GetDecimalFromCell(cellOfRecievedAmount.cellLine + i, cellOfRecievedAmount.cellColum, worksheet);

        string coinSymbol = GetCoinOutOfPair(pair);

        DataInfo dataInfo = new DataInfo
        {
          //DateOfTxn = date.ToString(),
          CoinSymbol = coinSymbol,
          DepotName = depot

        };

        if (buy_Sell.ToLower().Equals("sell"))
        {
          recievedAmount *= (-1);
        }

        if (!IsCoinInList(coinSymbol))
        {
          if (!IsCoinInExcelRegister(coinSymbol))
          {
            //TODo Fehlermeldung Pop Up
            Console.WriteLine("This Token '" + coinSymbol + "' From this CEX '" + depot + "' is not in the Excel Register!! Pls add it manuelly");
          }
          else
          {
            int coinIndexPortfolio = GetIndexOfCoinInPortfolio(coinSymbol);

            if (coinIndexPortfolio != -1)
            {
              portfolioList[coinIndexPortfolio].AmountHolding += recievedAmount;

              dataInfo.AmountOfCoin = recievedAmount;

              AddCoinToDepotList(dataInfo);
            }
          }
        }
        else
        {
          int coinIndexPortfolio = GetIndexOfCoinInPortfolio(coinSymbol);

          if (coinIndexPortfolio != -1)
          {

            portfolioList[coinIndexPortfolio].AmountHolding += recievedAmount;

            dataInfo.AmountOfCoin = recievedAmount;

            AddCoinToDepotList(dataInfo);
          }
        }

        i++;
      }

    }

    string GetCoinOutOfPair(string _pair)
    {
      string[] values = _pair.Split('/');

      return values[0];

    }

    void WritePortfolioIntoExcel()
    {

      Console.WriteLine("-------------WritePortfolioIntoExcel-------------");

      string worksheet = "Mein Bestand (2)";

      var cellOfName = HandleExcel.GetCellByName("Crypto (Name)", worksheet);
      var cellOfSymbol = HandleExcel.GetCellByName("Crypto (Ticker)", worksheet);
      var cellOfAmountOverAll = HandleExcel.GetCellByName("Stückzahl Insgesamt", worksheet);
      var cellOfCoinGeckoApiId = HandleExcel.GetCellByName("CoinGecko API ID", worksheet);
      var cellOfAmountOfCoins = HandleExcel.GetCellByName("Anzahl der Coins", worksheet);
      var cellOfDepots = HandleExcel.GetCellByName("Depots", worksheet);


      HandleExcel.ClearRange("A" + (cellOfAmountOfCoins.cellLine + 1).ToString(), "Z1000", worksheet);

      HandleExcel.SetValueInCell(portfolioList.Count, cellOfAmountOfCoins.cellLine + 1, cellOfAmountOfCoins.cellColum, worksheet);

      for (int i = 0; i < portfolioList.Count; i++)
      {
        
        HandleExcel.SetTextInCell(portfolioList[i].CoinGeckoApiID, cellOfCoinGeckoApiId.cellLine + 1 + i, cellOfCoinGeckoApiId.cellColum, worksheet);
        HandleExcel.SetTextInCell(portfolioList[i].Name, cellOfName.cellLine + 1 + i, cellOfName.cellColum, worksheet);
        HandleExcel.SetTextInCell(portfolioList[i].Symbol, cellOfSymbol.cellLine + 1 + i, cellOfSymbol.cellColum, worksheet);
        HandleExcel.SetDecimalValueInCell(portfolioList[i].AmountHolding, cellOfAmountOverAll.cellLine + 1 + i, cellOfAmountOverAll.cellColum, worksheet);

        int indexDepotColum = 0;
        while (HandleExcel.GetTextFromCell(cellOfDepots.cellLine + 1, cellOfDepots.cellColum + indexDepotColum, worksheet).Length > 0 )
        {
          string depot = HandleExcel.GetTextFromCell(cellOfDepots.cellLine + 1, cellOfDepots.cellColum + indexDepotColum, worksheet);

          for (int j = 0; j < depotInventoryList.Count; j++)
          {
            if (depotInventoryList[j].DepotName.ToLower().Equals(depot.ToLower()))
            {
              for (int k = 0; k < depotInventoryList[j].CoinInfos.Count; k++)
              {
                if (depotInventoryList[j].CoinInfos[k].Symbol.Equals(portfolioList[i].Symbol))
                {

                  HandleExcel.SetDecimalValueInCell(depotInventoryList[j].CoinInfos[k].AmountHolding, cellOfSymbol.cellLine + 1 + i, cellOfDepots.cellColum + indexDepotColum, worksheet);
                }
              }
              break;
            }
          }
          indexDepotColum++;
        }
      }
    }
  }
}
