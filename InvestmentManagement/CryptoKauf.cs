using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  class CryptoKauf
  {

    int amountToInvest;
    int lineNewBuy;
    List<CryptoBuyInfos> cryptoBuyInfos = new List<CryptoBuyInfos>();

    public int LineNewBuy { get => lineNewBuy; set => lineNewBuy = value; }

    public void GetAmountToInvest()
    {
      string worksheet = "Coins zu kaufen";
      //amountToInvest = (int) HandleExcel.GetValueFromCell(4,2,"Kauf Anweisung");
      amountToInvest = (int)HandleExcel.GetValueFromCell(lineNewBuy, (int)HandleExcel.Spalten.C, worksheet);
      Console.WriteLine("amountToInvest: " + amountToInvest);
    }

    public void CalculteBuyForCoins()
    {
      GetCryptoBuyInfos();
      InsertInfosInWorksheet();
    }

    void GetCryptoBuyInfos()
    {
      string worksheet = "Coins zu kaufen";
      int amountCryptosToBuyLine = lineNewBuy;
      int amountCryptosToBuy;


      amountCryptosToBuy = (int)HandleExcel.GetValueFromCell(amountCryptosToBuyLine, (int)HandleExcel.Spalten.D, worksheet);

      for (int i = 1; i <= amountCryptosToBuy; i++)
      {
        CryptoBuyInfos buyInfos = new CryptoBuyInfos();

        buyInfos.CryptoName = HandleExcel.GetTextFromCell(amountCryptosToBuyLine+ i, (int)HandleExcel.Spalten.E, worksheet);
        buyInfos.CryptoTicker = HandleExcel.GetTextFromCell(amountCryptosToBuyLine + i, (int)HandleExcel.Spalten.F, worksheet);
        buyInfos.BuyWith = HandleExcel.GetTextFromCell(amountCryptosToBuyLine + i, (int)HandleExcel.Spalten.I, worksheet);
        buyInfos.WhereToBuy = HandleExcel.GetTextFromCell(amountCryptosToBuyLine + i, (int)HandleExcel.Spalten.J, worksheet);

        cryptoBuyInfos.Add(buyInfos);
      }

      Risikoverteilung.GetAmountOfCryptos();
      Risikoverteilung.GetBewertungsnummerAufteilung();
      Risikoverteilung.GetBewertungsnummer(ref cryptoBuyInfos);
      CalculateAmountToBuy();
    }

    void CalculateAmountToBuy()
    {
      double conversionEURTOUSD = HandleExcel.GetValueFromCell(5, (int)HandleExcel.Spalten.E, "Data");
      for (int i = 0; i < cryptoBuyInfos.Count; i++)
      {
        cryptoBuyInfos[i].AmountToBuyEUR = (amountToInvest * (Risikoverteilung.BewertungsnummerAufteilung[cryptoBuyInfos[i].Bewertungsnummer] / 100))/ Risikoverteilung.AmountOfCoinsBewertungsnummer[cryptoBuyInfos[i].Bewertungsnummer];
        cryptoBuyInfos[i].AmountToBuyUSD = cryptoBuyInfos[i].AmountToBuyEUR * conversionEURTOUSD;
      }
    }

    void InsertInfosInWorksheet()
    {
      string worksheet = "Kauf Anweisung";
      int counter = 0;

      HandleExcel.SetValueInCell(amountToInvest, 4 , (int)HandleExcel.Spalten.B, worksheet);

      for (int i = 0; i < cryptoBuyInfos.Count; i++)
      {
        if (cryptoBuyInfos[i].ShouldBeBought)
        {
          HandleExcel.SetTextInCell(cryptoBuyInfos[i].CryptoName, 4 + counter, (int)HandleExcel.Spalten.C, worksheet);
          HandleExcel.SetTextInCell(cryptoBuyInfos[i].CryptoTicker, 4 + counter, (int)HandleExcel.Spalten.D, worksheet);
          HandleExcel.SetValueInCell(cryptoBuyInfos[i].Bewertungsnummer, 4 + counter, (int)HandleExcel.Spalten.E, worksheet);
          HandleExcel.SetTextInCell(cryptoBuyInfos[i].WhereToBuy, 4 + counter, (int)HandleExcel.Spalten.F, worksheet);
          HandleExcel.SetTextInCell(cryptoBuyInfos[i].BuyWith, 4 + counter, (int)HandleExcel.Spalten.G, worksheet);
          HandleExcel.SetValueInCell(cryptoBuyInfos[i].AmountToBuyEUR, 4 + counter, (int)HandleExcel.Spalten.H, worksheet);
          HandleExcel.SetValueInCell(cryptoBuyInfos[i].AmountToBuyUSD, 4 + counter, (int)HandleExcel.Spalten.I, worksheet);

          counter++;
        }
        
      }
    }

    void GetBewertungsnummer()
    {

      

    }

  }
}
