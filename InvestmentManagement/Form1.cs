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
    CryptoKauf CryptoKauf;
    //string _excelFilePath = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Crypto Investment_aktuell.xlsx"; //@"C:\Projekte\Unterlagen\Cryptos\Dokumente\Crypto Investment_aktuell.xlsx"; //
    string _excelFilePath = @"C:\Projekte\Unterlagen\Cryptos\Dokumente\Crypto Investment_aktuell.xlsx"; //
    string csvPath = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Binance\Export Order History-2022-10-16 16_49_26.csv";// @"C:\Projekte\Unterlagen\Cryptos\Dokumente\Binance\Export Order History-2022-10-16 16_49_26.csv";//
    string testExcelPath = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Binance\test.xlsx";
    string documentFolder = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\";
    public Form1()
    {
      InitializeComponent();
      //webCrawler = new WebCrawler();
      handleExcel = new HandleExcel(_excelFilePath);
     // ExtractDataFromCSV.SaveDataToExcel( ExtractDataFromCSV.GetDataFromCSV(csvPath), testExcelPath);
      CryptoKauf = new CryptoKauf();

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

    private void Click_CalculateBuyOfCryptos(object sender, System.EventArgs e)
    {
      txt_StatusLeiste.Text = "";

      CryptoKauf.LineNewBuy = int.Parse(txtb_LineNewBuy.Text);
      CryptoKauf.GetAmountToInvest();
      CryptoKauf.CalculteBuyForCoins();

      txt_StatusLeiste.Text = "CalculateBuyOfCryptos is finished.";

    }// Click_CalculateBuyOfCryptos
    private void Click_btn_CollBuySellCryptos(object sender, System.EventArgs e)
    {
      txt_StatusLeiste.Text = "";
      CollectData collectData = new CollectData();
      collectData.ExtractDataAndInsertInExcel(csvPath, "");
      txt_StatusLeiste.Text = "CollBuySellCryptos is finished.";

    }// Click_btn_CollBuySellCryptos
    private void Click_btn_MergeFlies(object sender, System.EventArgs e)
    {
      txt_StatusLeiste.Text = "";
      CollectData collectData = new CollectData();
      collectData.MergeFiles(documentFolder);
      txt_StatusLeiste.Text = "MergeFlies is finished.";

    }// Click_btn_CollBuySellCryptos

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
