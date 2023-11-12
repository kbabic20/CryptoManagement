using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InvestmentManagement
{
  public class BaseTradeInfo
  {
    public string Network { get; set; }
    public string NetworkCurrency { get; set; }
    public List<PropertyInfo> IterateMembersInOrder()
    {
      var properties = GetType().GetProperties()
                               .Where(prop => prop.IsDefined(typeof(CsvOrderAttribute), false))
                               .OrderBy(prop => ((CsvOrderAttribute)prop.GetCustomAttribute(typeof(CsvOrderAttribute), false)).Order);
      return properties.ToList();
    }
  }
  public interface IGetProperty
  {
    public List<PropertyInfo> IterateMembersInOrder();
  }

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


  public class NetworkTxnInfo:BaseTradeInfo,IGetProperty
  {
    [CsvOrder(1)]
    public string Txhash { get; set; }
    [CsvOrder(2)]
    public string Blockno { get; set; }
    [CsvOrder(3)]
    public string UnixTimestamp { get; set; }
    [CsvOrder(4)]
    public string DateTime { get; set; }
    [CsvOrder(5)]
    public string From { get; set; }
    [CsvOrder(6)]
    public string To { get; set; }
    [CsvOrder(7)]
    public string ContractAddress { get; set; }
    [CsvOrder(8)]
    public string ValueIn { get; set; }
    [CsvOrder(9)]
    public string ValueOut { get; set; }
    [CsvOrder(10)]
    public string CurrentValue { get; set; }
    [CsvOrder(11)]
    public string TxnFeeNative { get; set; }
    [CsvOrder(12)]
    public string TxnFeeUsd { get; set; }
    [CsvOrder(13)]
    public string HistoricalPrice { get; set; }
    [CsvOrder(14)]
    public string Status { get; set; }
    [CsvOrder(15)]
    public string ErrCode { get; set; }
    [CsvOrder(16)]
    public string Method { get; set; }

  }
  public class NetworkTokenTxnInfo : BaseTradeInfo, IGetProperty
  {
    [CsvOrder(1)]
    public string Txhash { get; set; }
    [CsvOrder(2)]
    public string Blockno { get; set; }
    [CsvOrder(3)]
    public string UnixTimestamp { get; set; }
    [CsvOrder(4)]
    public string DateTime { get; set; }
    [CsvOrder(5)]
    public string From { get; set; }
    [CsvOrder(6)]
    public string To { get; set; }
    [CsvOrder(7)]
    public string TokenAmount { get; set; }
    [CsvOrder(8)]
    public string UsdValueDayOfTx { get; set; }
    [CsvOrder(9)]
    public string ContractAddress { get; set; }
    [CsvOrder(10)]
    public string TokenName { get; set; }
    [CsvOrder(11)]
    public string TokenSymbol { get; set; }

  }

  public class Portfolio
  {
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string ContractAddress { get; set; }
    public string CoinGeckoApiID { get; set; }
    public string Network { get; set; }
    public decimal AmountHolding { get; set; }
    public decimal AvgBuyPrice { get; set; }
    public decimal AvgGain { get; set; }
    public decimal CurrPrice { get; set; }
  }
  public class DepotInventory
  {
    public string DepotName { get; set; }
    public List<CoinInfo> CoinInfos { get; set; }
  }

  public class CoinInfo
  {
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string ContractAddress { get; set; }
    public string Network { get; set; }
    public decimal AmountHolding { get; set; }
  }

  public class MyWallet
  {
    public string Address { get; set; }
    public string Name { get; set; }
  }
  public class CryptoRegisterData
  {
    public string ContractAddress { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string CoinGeckoApiID { get; set; }
    public string Network { get; set; }
  }

  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class CsvOrderAttribute : Attribute
  {
    public int Order { get; }

    public CsvOrderAttribute(int order)
    {
      Order = order;
    }
  }
}
