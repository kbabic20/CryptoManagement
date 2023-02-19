using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  public class CryptoBuyInfos
  {
    string cryptoName;
    string cryptoTicker;
    int bewertungsnummer;
    bool shouldBeBought;
    string whereToBuy;
    string buyWith;
    double amountToBuyEUR;
    double amountToBuyUSD;

    public string CryptoName { get => cryptoName; set => cryptoName = value; }
    public string CryptoTicker { get => cryptoTicker; set => cryptoTicker = value; }
    public int Bewertungsnummer { get => bewertungsnummer; set => bewertungsnummer = value; }
    public bool ShouldBeBought { get => shouldBeBought; set => shouldBeBought = value; }
    public string WhereToBuy { get => whereToBuy; set => whereToBuy = value; }
    public string BuyWith { get => buyWith; set => buyWith = value; }
    public double AmountToBuyEUR { get => amountToBuyEUR; set => amountToBuyEUR = value; }
    public double AmountToBuyUSD { get => amountToBuyUSD; set => amountToBuyUSD = value; }
  }
}
