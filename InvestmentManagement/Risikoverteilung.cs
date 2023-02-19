using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement
{
  public class Risikoverteilung
  {
    static string worksheet = "Risiko Verteilung";
    static int amountOfCryptos;
    static int amountOfBewertungsnummer = 6;
    static Dictionary<int, double> bewertungsnummerAufteilung = new Dictionary<int, double>();
    static Dictionary<int, double> amountOfCoinsBewertungsnummer = new Dictionary<int, double>();

    public static Dictionary<int, double> BewertungsnummerAufteilung { get => bewertungsnummerAufteilung; set => bewertungsnummerAufteilung = value; }
    public static Dictionary<int, double> AmountOfCoinsBewertungsnummer { get => amountOfCoinsBewertungsnummer; set => amountOfCoinsBewertungsnummer = value; }

    public static void GetAmountOfCryptos()
    {
      amountOfCryptos = (int)HandleExcel.GetValueFromCell(7, (int)HandleExcel.Spalten.A, worksheet);

    }

    public static void GetBewertungsnummerAufteilung()
    {
      for (int i = 0; i < amountOfBewertungsnummer; i++)
      {
        BewertungsnummerAufteilung.Add((int)HandleExcel.GetValueFromCell(3, ((int)HandleExcel.Spalten.L) + i, worksheet),
        HandleExcel.GetValueFromCell(4, ((int)HandleExcel.Spalten.L) + i, worksheet));

        amountOfCoinsBewertungsnummer.Add(i, 0);
      }
      

    }

    public static void GetBewertungsnummer(ref List<CryptoBuyInfos> _cryptoBuyInfos)
    {
      int line;
      

      for (int i = 0; i < _cryptoBuyInfos.Count; i++)
      {
        line = FindLine(_cryptoBuyInfos[i].CryptoName, _cryptoBuyInfos[i].CryptoTicker);
        if (line > 0)
        {


          if (HandleExcel.GetTextFromCell(line, (int)HandleExcel.Spalten.E, worksheet) == "Ja")
          {
            _cryptoBuyInfos[i].ShouldBeBought = true;

            _cryptoBuyInfos[i].Bewertungsnummer = (int)HandleExcel.GetValueFromCell(line, (int)HandleExcel.Spalten.D, worksheet);

            amountOfCoinsBewertungsnummer[_cryptoBuyInfos[i].Bewertungsnummer]++;
          }
          else
          {
            _cryptoBuyInfos[i].ShouldBeBought = false;
          }
        }
        else
        {
          Console.WriteLine("Coin {0} {1} not found in Risiko Verteilung!!!", _cryptoBuyInfos[i].CryptoName, _cryptoBuyInfos[i].CryptoTicker);
        }
      }

      Console.WriteLine("AmountOfBewertungsnummer:");
      for (int i = 0; i < amountOfCoinsBewertungsnummer.Count; i++)
      {
        Console.WriteLine("Bewertungsnummer {0}: {1}", amountOfCoinsBewertungsnummer.ElementAt(i).Key, amountOfCoinsBewertungsnummer.ElementAt(i).Value);
      }

    }

    static int FindLine(string _cryptoName, string _cryptoTicker)
    {
      int lineBegin = 7;
      
      for (int i = 0; i <= amountOfCryptos; i++)
      {
        if (_cryptoName == HandleExcel.GetTextFromCell(lineBegin + i, (int)HandleExcel.Spalten.B, worksheet)
          && _cryptoTicker == HandleExcel.GetTextFromCell(lineBegin + i, (int)HandleExcel.Spalten.C, worksheet))
        {
          return i + lineBegin;
        }
      }

      return -1; 
    }

  }
}
