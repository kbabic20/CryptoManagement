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
    List<string> whereToBuy = new List<string>();
    Dictionary<string, double> whereToBuyDic = new Dictionary<string, double>();

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

        if (whereToBuy.Contains(buyInfos.WhereToBuy))
        {
          whereToBuy.Add(buyInfos.WhereToBuy);
        }

        if (!(buyInfos.BuyWith is null))
        {
          if(buyInfos.BuyWith.Contains("etwork"))
          {
            if (!whereToBuyDic.ContainsKey(buyInfos.BuyWith.ToLower()))
            {
              whereToBuyDic.Add(buyInfos.BuyWith.ToLower(), 0.0);
            }
          }
          else
          {
            if (!whereToBuyDic.ContainsKey(buyInfos.WhereToBuy.ToLower()))
            {
              whereToBuyDic.Add(buyInfos.WhereToBuy.ToLower(), 0.0);
            }

          }
        }
        else 
        {
          if (!whereToBuyDic.ContainsKey(buyInfos.WhereToBuy.ToLower()))
          {
            whereToBuyDic.Add(buyInfos.WhereToBuy.ToLower(), 0.0);
          }
            
        }
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
        if (cryptoBuyInfos[i].ShouldBeBought)
        {
          cryptoBuyInfos[i].AmountToBuyEUR = (amountToInvest * (Risikoverteilung.BewertungsnummerAufteilung[cryptoBuyInfos[i].Bewertungsnummer] / 100)) / Risikoverteilung.AmountOfCoinsBewertungsnummer[cryptoBuyInfos[i].Bewertungsnummer];
          cryptoBuyInfos[i].AmountToBuyUSD = cryptoBuyInfos[i].AmountToBuyEUR * conversionEURTOUSD;
          if (whereToBuyDic.ContainsKey(cryptoBuyInfos[i].WhereToBuy.ToLower()))
          {
            whereToBuyDic[cryptoBuyInfos[i].WhereToBuy.ToLower()] += cryptoBuyInfos[i].AmountToBuyEUR;
          }
          else if (whereToBuyDic.ContainsKey(cryptoBuyInfos[i].BuyWith.ToLower()))
          {
            whereToBuyDic[cryptoBuyInfos[i].BuyWith.ToLower()] += cryptoBuyInfos[i].AmountToBuyEUR;
          }
        }
        
        
      }
    }

    void InsertInfosInWorksheet()
    {
      string worksheet = "Kauf Anweisung";
      int counter = 0;
      HandleExcel.ClearRange("B4", "I1000", worksheet);
      HandleExcel.ClearRange("K3", "AZ100", worksheet);
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

      double conversionEURTOUSD = HandleExcel.GetValueFromCell(5, (int)HandleExcel.Spalten.E, "Data");

      for (int i = 0; i < whereToBuyDic.Count; i++)
      {
        HandleExcel.SetTextInCell(whereToBuyDic.ElementAt(i).Key + " EUR", 3 , (int)HandleExcel.Spalten.K + i *2, worksheet);
        HandleExcel.SetTextInCell(whereToBuyDic.ElementAt(i).Key + "USD", 3, (int)HandleExcel.Spalten.K + i * 2 + 1, worksheet);
        HandleExcel.SetValueInCell(whereToBuyDic.ElementAt(i).Value, 4, (int)HandleExcel.Spalten.K + i * 2, worksheet);
        HandleExcel.SetValueInCell(whereToBuyDic.ElementAt(i).Value * conversionEURTOUSD, 4, (int)HandleExcel.Spalten.K + i * 2 + 1, worksheet);
      }

    }

    void GetBewertungsnummer()
    {

      

    }

  }
}
