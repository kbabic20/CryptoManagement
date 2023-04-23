using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  public class CexBuySellInfo
  {
    public string Plattfrom { get; set; }
    public string Pair { get; set; }
    public string Date { get; set; }
    public string BuyOrSell { get; set; }
    public string Price { get; set; }
    public string PriceCurrency { get; set; }
    public string RecievedAmount { get; set; }
    public string AmountInvestedAfterFee { get; set; }
    public string AmountInvested { get; set; }
    public string Fee { get; set; }
    public string FeeCurrency { get; set; }
  }


  public class NetworkTxnInfo
  {
    public string Network { get; set; }
    public string NetworkCurrency { get; set; }
    public string Txhash { get; set; }
    public string Blockno { get; set; }
    public string UnixTimestamp { get; set; }
    public string DateTime { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string ContractAddress { get; set; }
    public string ValueIn { get; set; }
    public string ValueOut { get; set; }
    public string TxnFeeNative { get; set; }
    public string TxnFeeUsd { get; set; }
    public string HistoricalPrice { get; set; }
    public string Method { get; set; }
  }
  public class NetworkTokenTxnInfo
  {
    public string Network { get; set; }
    public string NetworkCurrency { get; set; }
    public string Txhash { get; set; }
    public string Blockno { get; set; }
    public string UnixTimestamp { get; set; }
    public string DateTime { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string TokenAmount { get; set; }
    public string UsdValueDayOfTx { get; set; }
    public string ContractAddress { get; set; }
    public string TokenName { get; set; }
    public string TokenSymbol { get; set; }
  }
}
