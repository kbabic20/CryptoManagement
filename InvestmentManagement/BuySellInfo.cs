using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  public class BuySellInfo
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
}
