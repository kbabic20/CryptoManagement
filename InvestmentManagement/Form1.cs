using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;


namespace InvestmentManagement
{
  public partial class Form1 : Form
  {



    WebCrawler webCrawler;
    HandleExcel handleExcel;
    CoinGeckoAPI coinGeckoAPI;
    string _excelFilePath = "D:\\OneDrive\\OneDrive - rfh-campus.de\\Finanzen\\Investment\\Cryptos\\Crypto Investment_aktuell.xlsx";// "C:\\Users\\Kasim\\OneDrive - rfh-campus.de\\Finanzen\\Investment\\Cryptos\\Crypto Investment.xlsx";
    string csvPath = @"D:\OneDrive\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Binance\Export Order History-2022-01-29 18_09_36.csv";
    string testExcelPath = @"D:\OneDrive\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Binance\test.xlsx";
    public Form1()
    {
      InitializeComponent();
      //webCrawler = new WebCrawler();
      handleExcel = new HandleExcel(_excelFilePath);
      ExtractDataFromCSV.SaveDataToExcel( ExtractDataFromCSV.GetDataFromCSV(csvPath), testExcelPath);


    }

    private void Click_RefreshStockPrice(object sender, System.EventArgs e)
    {


      txt_StatusLeiste.Text = "";
      handleExcel.RefreshStockPrices();


      txt_StatusLeiste.Text = "Ticker webcrawler is finished.";


    }// Click_RefreshStockPrice

    private void Click_RefreshCryptoPrice(object sender, System.EventArgs e)
    {


      txt_StatusLeiste.Text = "";
      handleExcel.RefreshCryptoPrices(); // coinGeckoAPI.GetCurrentPrice("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=eur");


      txt_StatusLeiste.Text = "Refresh is finished.";


    }// Click_RefreshStockPrice

    private void ReleaseObject(object obj)
    {
      try
      {
        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        obj = null;
      }
      catch (Exception ex)
      {
        obj = null;
        MessageBox.Show("Unable to release the Object " + ex.ToString());
      }
      finally
      {
        GC.Collect();
      }
    } //ReleaseObject

  }// Class


} //namespace
