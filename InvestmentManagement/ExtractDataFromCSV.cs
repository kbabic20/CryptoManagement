using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;

namespace InvestmentManagement
{
 public class ExtractDataFromCSV
  {
    enum Cex
    {
      Mxc = 1,
      Kucoin,
      Binance,
      Okx
    }

    enum Spalte
    {
      A,B,C,D,E,F,G,H,I,J,K
    }
    public static DataTable GetDataFromCSV(string filePath)
    {
      DataTable data = new DataTable();

      using (TextFieldParser parser = new TextFieldParser(filePath))
      {
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(";",",");

        string[] headers = parser.ReadFields();
        foreach (string header in headers)
        {
          data.Columns.Add(header);
        }

        while (!parser.EndOfData)
        {
          string[] fields = parser.ReadFields();
          data.Rows.Add(fields);
        }
      }

      return data;
    }

    public static void SaveDataToExcel(DataTable data, string filePath)
    {
      Excel.Application excelApp = new Excel.Application();
      Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
      Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;

      for (int i = 0; i < data.Columns.Count; i++)
      {
        excelWorksheet.Cells[1, i + 1] = data.Columns[i].ColumnName;
      }

      for (int i = 0; i < data.Rows.Count; i++)
      {
        for (int j = 0; j < data.Columns.Count; j++)
        {
          excelWorksheet.Cells[i + 2, j + 1] = data.Rows[i][j];
        }
      }

      excelWorkbook.SaveAs(filePath);
      excelWorkbook.Close();
      excelApp.Quit();
    }

    static void ExtractData(string _path, Cex _cex )
    {
      // Define the regular expression patterns to match numbers and non-numbers
      string numberPattern = @"-?\d+(\.\d+)?";
      string nonNumberPattern = @"[^\d\.]+";
      // Create regular expression objects for both patterns
      Regex numberRegex = new Regex(numberPattern);
      Regex nonNumberRegex = new Regex(nonNumberPattern);

      using (StreamReader reader = new StreamReader(_path))
      {
        while (!reader.EndOfStream)
        {
          string line = reader.ReadLine();


          switch (_cex)
          {
            case Cex.Mxc:

              string[] values = line.Split(',');
              int separateNumberCount = 4;
              string[] input = new string [separateNumberCount];
              string[] numberString = new string[separateNumberCount];
              string[] nonNumberString = new string[separateNumberCount];
              decimal[] number = new decimal[separateNumberCount];
              for (int i = 0; i < separateNumberCount; i++)
              {
                input[i] = values[(int)Spalte.D+i];

                // Extract the number and non-number substrings using regular expressions
                numberString[i] = numberRegex.Match(input[i]).Value;
                nonNumberString[i] = nonNumberRegex.Match(input[i]).Value;

                // Parse the number substring to a decimal number
                number[i] = decimal.Parse(numberString[i]);
              }
              

              BuySellInfo buySellInfo = new BuySellInfo
              {
                Plattfrom = "Mexc",
                Pair = values[(int)Spalte.A].Replace("_", "/"),
                Date = values[(int)Spalte.B],
                BuyOrSell = values[(int)Spalte.C],
                Price = numberString[0],
                PriceCurrency = nonNumberString[0],
                RecievedAmount = numberString[1],
                AmountInvested = numberString[2],
                Fee = numberString[3]
              };
          
          break;
            case Cex.Kucoin:
              break;
            case Cex.Binance:
              break;
            case Cex.Okx:
              break;
            default:
              break;
          }
        }
      }
    }
  }
}
