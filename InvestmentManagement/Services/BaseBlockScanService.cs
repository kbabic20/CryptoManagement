using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace InvestmentManagement.Services
{
  public abstract class BaseBlockScanService
  {
    public class EtherScanApiResponse<T>
    {
      public List<T> result { get; set; }
    }
     string baseUrl;
     string apiKey;
    string walletAddress;

    public virtual string BaseUrl { get => baseUrl; set => baseUrl = value; }
    public virtual string ApiKey { get => apiKey; set => apiKey = value; }
    public virtual string WalletAddress { get => walletAddress; set => walletAddress = value; }

    public async Task GetDataAndSaveToCsv(string filePathRoot)
    {
      //string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Ethereum Network";

      var filePathRootSplited = filePathRoot.Split('\\');
      string date = DateTime.Today.ToString("dd_MM_yyyy");

      string networkName = filePathRootSplited.Last().Replace(" Network","");

      string filePathNetworkTxn = filePathRoot + $@"\Transaktionen\Einzelne CSV Files\{networkName}_Transactions_{date}.csv";
      string filePathNetworkTokenTxn = filePathRoot + $@"\Token Transaktionen\Einzelne CSV Files\{networkName}_TokenTransactions_{date}.csv";

      List<NetworkTxnInfo> networkTxnInfos = await GetNetworkTxnInfoAsync();
      List<NetworkTokenTxnInfo> networkTokenTxnInfos = await GetNetworkTokenTxnInfoAsync();

      SaveNetworkTxnInfoToCSV(networkTxnInfos, filePathNetworkTxn);
      SaveNetworkTxnInfoToCSV(networkTokenTxnInfos, filePathNetworkTokenTxn);
    }
    public async Task<List<NetworkTxnInfo>> GetNetworkTxnInfoAsync()
    {
      List<NetworkTxnInfo> networkTxnInfos = new List<NetworkTxnInfo>();
      List<BlockScanModel.Txlist> transactionList = await GetTxlistsAsync();

      foreach (var transaction in transactionList)
      {
        networkTxnInfos.Add(ConvertTxlistToNetworkTxnInfo(transaction));
      }

      return networkTxnInfos;
    }

    public async Task<List<BlockScanModel.Txlist>> GetTxlistsAsync()
    {
      List<BlockScanModel.Txlist> transactionList = new List<BlockScanModel.Txlist>();
      string url = $"{BaseUrl}?module=account&action=txlist&address={WalletAddress}&startblock=0&endblock=99999999&page=1&offset=10000&sort=asc&apikey={ApiKey}";


      transactionList = await GetApiResultsAsync<BlockScanModel.Txlist>(url);

      return transactionList;
    }

    async Task<List<T>> GetApiResultsAsync<T>(string url)
    {
      List<T> transactionList = new List<T>();

      using (HttpClient client = new HttpClient())
      {

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
          string responseContent = response.Content.ReadAsStringAsync().Result;

          // Deserialisierung der Antwort in die Liste von Txlist-Objekten
          var result = JsonConvert.DeserializeObject<EtherScanApiResponse<T>>(responseContent);
          if (result != null && result.result != null)
          {
            transactionList = result.result;
          }
        }
        else
        {
          Console.WriteLine("Fehler beim Abrufen der Daten. Statuscode: " + response.StatusCode);
        }
      }

      return transactionList;
    }

    NetworkTxnInfo ConvertTxlistToNetworkTxnInfo(BlockScanModel.Txlist transaction)
    {

      string valueIn;
      string valueOut;
      string status = "";

      if (transaction.Input.Equals("0x"))
      {
        valueIn = WeiConveter(transaction.Value);
        valueOut = "0";
      }
      else
      {
        valueIn = "0";
        valueOut = WeiConveter(transaction.Value);
      }

      if (transaction.Txreceipt_status != "1")
      {
        status = "Error";
      }
      else
      {
        status = transaction.Txreceipt_status;
      }

      NetworkTxnInfo networkTxnInfo = new NetworkTxnInfo
      {
        Txhash = transaction.Hash,
        Blockno = transaction.BlockNumber,
        UnixTimestamp = transaction.TimeStamp,
        DateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(transaction.TimeStamp)).LocalDateTime.ToString(),
        From = transaction.From,
        To = transaction.To,
        ContractAddress = transaction.ContractAddress,
        ValueIn = valueIn,
        ValueOut = valueOut,
        CurrentValue = "",
        TxnFeeNative = CalcualteGasToEth(transaction.GasUsed, transaction.GasPrice),
        TxnFeeUsd = "",
        HistoricalPrice = "",
        Status = status,
        ErrCode = transaction.IsError,
        Method = transaction.FunctionName

      };

      return networkTxnInfo;
    }

    decimal WeiConveter(decimal weiValue)
    {
      return weiValue / (decimal)Math.Pow(10, 18);
    }

    string WeiConveter(string weiValue, int decimalPlace = 18)
    {
      return (double.Parse(weiValue) / (double)Math.Pow(10, decimalPlace)).ToString(CultureInfo.InvariantCulture);
    }

    decimal CalcualteGasToEth(decimal gasUsed, decimal gasPrice)
    {

      return WeiConveter(gasUsed * gasPrice);
    }
    string CalcualteGasToEth(string gasUsed, string gasPrice)
    {
      decimal gasValue = decimal.Parse(gasUsed) * decimal.Parse(gasPrice);
      return WeiConveter(gasValue).ToString(CultureInfo.InvariantCulture);
    }



    public async Task<List<NetworkTokenTxnInfo>> GetNetworkTokenTxnInfoAsync()
    {
      List<NetworkTokenTxnInfo> networkTokenTxnInfos = new List<NetworkTokenTxnInfo>();
      List<BlockScanModel.Tokentx> transactionList = await GetTokentxAsync();

      foreach (var transaction in transactionList)
      {
        networkTokenTxnInfos.Add(ConvertTokentxToNetworkTokenTxnInfo(transaction));
      }

      return networkTokenTxnInfos;
    }
    async Task<List<BlockScanModel.Tokentx>> GetTokentxAsync()
    {
      List<BlockScanModel.Tokentx> transactionList = new List<BlockScanModel.Tokentx>();
      string url = $"{BaseUrl}?module=account&action=tokentx&address={WalletAddress}&startblock=0&endblock=99999999&page=1&offset=10000&sort=asc&apikey={ApiKey}";

      transactionList = await GetApiResultsAsync<BlockScanModel.Tokentx>(url);

      return transactionList;
    }


    NetworkTokenTxnInfo ConvertTokentxToNetworkTokenTxnInfo(BlockScanModel.Tokentx transaction)
    {

      NetworkTokenTxnInfo networkTokenTxnInfo = new NetworkTokenTxnInfo
      {
        Txhash = transaction.Hash,
        Blockno = transaction.BlockNumber,
        UnixTimestamp = transaction.TimeStamp,
        DateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(transaction.TimeStamp)).LocalDateTime.ToString(),
        From = transaction.From,
        To = transaction.To,
        TokenAmount = WeiConveter(transaction.Value,int.Parse(transaction.TokenDecimal)),
        UsdValueDayOfTx = "",
        ContractAddress = transaction.ContractAddress,
        TokenName = transaction.TokenName,
        TokenSymbol = transaction.TokenSymbol
      };

      return networkTokenTxnInfo;
    }

    public void SaveNetworkTxnInfoToCSV<T>(List<T> transactions, string filePath) where T : IGetProperty
    {
      using (StreamWriter file = new StreamWriter(filePath))
      {
        // Schreibe die CSV-Header-Zeile
        file.WriteLine(GetCsvHeader(transactions[0]));

        // Schreibe Daten jeder Transaktion in die CSV-Datei
        foreach (var transaction in transactions)
        {
          file.WriteLine(GetCsvLine(transaction));
        }

      }
    }
    public string GetCsvHeader<T>(T networkTxnInfo) where T : IGetProperty
    {
      string header = "";

      foreach (var property in networkTxnInfo.IterateMembersInOrder())
      {
        if (header.Length == 0)
        {
          if (property.Name.Contains("\""))
          {
            header = property.Name;
          }
          else
          {
            header = "\"" + property.Name + "\"";
          }
        }
        else
        {
          if (property.Name.Contains("\""))
          {
            header += "," + property.Name;
          }
          else
          {
            header += ",\"" + property.Name + "\"";
          }
        }
      }

      return header;

    }

    public string GetCsvLine<T>(T networkTxnInfo) where T : IGetProperty
    {
      string line = "";

      foreach (var property in networkTxnInfo.IterateMembersInOrder())
      {
        if (line.Length == 0)
        {
          if (property.GetValue(networkTxnInfo, null).ToString().Contains("\""))
          {
            line = property.GetValue(networkTxnInfo, null).ToString();
          }
          else
          {
            line = "\"" + property.GetValue(networkTxnInfo, null).ToString() + "\"";
          }
        }
        else
        {
          if (property.GetValue(networkTxnInfo, null).ToString().Contains("\""))
          {
            line += "," + property.GetValue(networkTxnInfo, null).ToString();
          }
          else
          {
            line += ",\"" + property.GetValue(networkTxnInfo, null).ToString() + "\"";
          }
        }
      }

      return line;

    }
  }
}

