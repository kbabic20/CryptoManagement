using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace InvestmentManagement
{
  public class HandleExcel
  {
    class CellNames
    {
      public string worksheet;
      public string nameOfCell;
      public (int cellLine, int cellColum) cell;
    }

    Excel.Application xlApp;
    static Excel._Workbook oWB;
    static Excel._Worksheet oSheet;
    Excel.Range oRng;
    int itemNo_InvestmentAufteilung;
    int itemNo_Etf_Aktien_Infos;
    int itemNo_Mein_Bestand;
    int _amountOfstocks = 0;

    static string worksheetNameOld = "";

    List<string> _urls = new List<string>();
    List<string> _stockTypes = new List<string>();
    List<string> _stockISINs = new List<string>();
    List<string> _stockNames = new List<string>();
    List<string> _priceOfStocks = new List<string>();
    List<string> coinGeckoAPIIDS = new List<string>();
    List<string> priceOfCryptos = new List<string>();

   static List<CellNames> cellNames = new List<CellNames>();

    WebCrawler webCrawler;
    CoinGeckoAPI coinGeckoAPI;

    

    public enum Spalten
    {
      A = 1,
      B,
      C,
      D,
      E,
      F,
      G,
      H,
      I,
      J,
      K,
      L,
      M,
      N,
      O,
      P,
      Q

       
    }

    public HandleExcel(string _path)
    {
      itemNo_InvestmentAufteilung = 0;
      itemNo_Etf_Aktien_Infos = 0;
      this.itemNo_Mein_Bestand = 0;

      OpenExcelFile(_path);

      webCrawler = new WebCrawler();
      this.coinGeckoAPI = new CoinGeckoAPI();
    }

    void OpenExcelFile(string _path)
    {
      Console.WriteLine("-------------OpenExcelFile-------------");

      //Start Excel and get Application object.
      xlApp = new Excel.Application();
      xlApp.Visible = true;
      oWB = (Excel._Workbook)xlApp.Workbooks.Open(_path);

      // SaveStockInfos();

      //xlApp = GetExcelInstance3(_path);//Excel.Application existingExcelApp = GetExcelInstance(_path);

      //if (xlApp != null)//existingExcelApp != null)
      //{
      //  // Verwende vorhandene Excel-Instanz
      //  oWB = xlApp.Workbooks.Open(_path); // Excel.Workbook workbook =  existingExcelApp.Workbooks.Open(_path);
      //  // Weitere Aktionen mit der geöffneten Datei durchführen...
      //}
      //else
      //{
      //  // Erstelle eine neue Excel-Instanz und öffne die Datei
      //  xlApp = new Excel.Application(); // Excel.Application newExcelApp = new Excel.Application();
      //  oWB = xlApp.Workbooks.Open(_path); //Excel.Workbook workbook =  newExcelApp.Workbooks.Open(_path);
      //  // Weitere Aktionen mit der geöffneten Datei durchführen...
      //  xlApp.Visible = true;
      //}

      

      //GetIndexOfWorkSheets();
    }
    public Excel.Application GetExcelInstance(string filePath)
    {
      Excel.Application excelApp = null;

      try
      {
        excelApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");

        foreach (Excel.Workbook workbook in excelApp.Workbooks)
        {
          if (workbook.FullName.Equals(filePath))
          {
            return excelApp;
          }
        }
      }
      catch (System.Runtime.InteropServices.COMException)
      {
        // Excel is not currently running
      }

      return null;
    }

    public Excel.Application GetExcelInstance2(string filePath)
    {
      Excel.Application excelApp = null;

      try
      {
        var processes = System.Diagnostics.Process.GetProcessesByName("excel");

        foreach (var process in processes)
        {
          var wbCount = process.MainWindowHandle.ToInt32();
          if (wbCount > 0)
          {
            Excel.Application tempApp = System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application") as Excel.Application;
            foreach (Excel.Workbook workbook in tempApp.Workbooks)
            {
              if (workbook.FullName.Equals(filePath))
              {
                excelApp = tempApp;
                break;
              }
            }
            if (excelApp != null)
              break;
          }
        }
      }
      catch (System.Runtime.InteropServices.COMException)
      {
        // Excel is not currently running
      }

      return excelApp;
    }

    public Excel.Application GetExcelInstance3(string filePath)
    {
      Excel.Application excelApp = null;

      try
      {
        excelApp = new Excel.Application();
        excelApp.DisplayAlerts = false; // Verhindert das Anzeigen von Benachrichtigungen

        Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
        workbook.Close(SaveChanges: false); // Schließt die Datei ohne Speichern

        excelApp.Quit();

        excelApp.DisplayAlerts = true; // Stellt die ursprünglichen Anzeigeeinstellungen wieder her
      }
      catch (System.Runtime.InteropServices.COMException)
      {
        // Die Datei ist bereits geöffnet, daher wird die Excel-Instanz genutzt
        return excelApp;
      }
      catch (Exception)
      {
        // Fehler beim Öffnen der Datei oder allgemeiner Fehler
        // Hier kannst du entsprechend handeln
      }

      // Wenn die Datei nicht geöffnet ist oder ein Fehler auftritt, wird null zurückgegeben
      return null;
    }

    static bool IsWorkbookOpened(string wbook)
    {
      bool isOpened = true;
      Excel.Application exApp;
      exApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
      try
      {
        exApp.Workbooks.get_Item(wbook);
      }
      catch (Exception)
      {
        isOpened = false;
      }
      return isOpened;
    }


    void GetIndexOfWorkSheets()
    {
      Console.WriteLine("-------------GetIndexOfWorkSheets-------------");

      Console.WriteLine("Get worksheet numbers:");
      for (int _currWorkShettNo = 1; _currWorkShettNo < oWB.Worksheets.Count + 1; _currWorkShettNo++)
      {
        oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(_currWorkShettNo);

        Console.WriteLine("Sheet name: " + oSheet.Name);

        // Get worksheet "Investment Aufteilung"
        if (oSheet.Name == "Investment Aufteilung")
        {
          itemNo_InvestmentAufteilung = _currWorkShettNo;
        }
        // Get worksheet "Etf_Aktien_Infos"
        if (oSheet.Name == "Etf_Aktien_Infos")
        {
          itemNo_Etf_Aktien_Infos = _currWorkShettNo;
        }

        // Get worksheet "Mein Bestand"
        if (oSheet.Name == "Mein Bestand")
        {
          itemNo_Mein_Bestand = _currWorkShettNo;
        }
      }


    }

    void CopyStockInfosToEtf_Aktien_Infos()
    {

      string _ticker = "";
      int _startLine;


      Console.WriteLine("-------------CopyStockInfosToEtf_Aktien_Infos-------------");

      if (this._amountOfstocks > 0)
      {
        // Get worksheet "Etf_Aktien_Infos"
        oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(itemNo_Etf_Aktien_Infos);
        Console.WriteLine("Name of worksheet: " + oSheet.Name);

        var _cellOfISIN = GetCellByName("Aktie/ETF (ISIN)");
        var _cellOfStockType = GetCellByName("ETF oder Aktie");
        var _cellOfStockName = GetCellByName("Aktie/ETF (Name)");
        var _cellOfStockTicker = GetCellByName("Aktie/ETF (Ticker/Symbol)");

        if (_cellOfISIN.cellColum == 0 || _cellOfISIN.cellLine == 0)
        {
          Console.WriteLine("Cell of ISIN was not found");
        }
        else if (_cellOfStockType.cellColum == 0 || _cellOfStockType.cellLine == 0)
        {
          Console.WriteLine("Cell of stock type was not found");
        }
        else if (_cellOfStockName.cellColum == 0 || _cellOfStockName.cellLine == 0)
        {
          Console.WriteLine("Cell of stock name was not found");
        }
        else
        {

          _startLine = _cellOfISIN.cellLine + 1;
          for (int _currStock = _startLine; _currStock < (this._amountOfstocks + _startLine); _currStock++)
          {

            oSheet.Cells[_currStock, _cellOfISIN.cellColum].Value = this._stockISINs[_currStock - _startLine];
            oSheet.Cells[_currStock, _cellOfStockType.cellColum].Value = this._stockTypes[_currStock - _startLine];
            oSheet.Cells[_currStock, _cellOfStockName.cellColum].Value = this._stockNames[_currStock - _startLine];
            
            if (this._stockTypes[_currStock - _startLine] == "ETF")
            {
              // _ticker = webCrawler.GetTickerFromISIN("https://de.extraetf.com/etf-profile/" + _currentISIN);
            }
            else if (this._stockTypes[_currStock - _startLine] == "Aktie")
            {
              // _ticker = webCrawler.GetTickerFromISIN("https://de.extraetf.com/stock-profile/" + _currentISIN);
            }
            else
            {
              Console.WriteLine("No stock type was found!");
              oSheet.Cells[_currStock, _cellOfStockType.cellColum].Value = "Atkien Typ nicht gefunden!";
            }

            if (_ticker == null)
            {
              Console.WriteLine("No ticker was found!");
              _ticker = "Ticker nicht gefunden!";
            }


            oSheet.Cells[_currStock, _cellOfStockTicker.cellColum].Value = _ticker;


          }
        }

      }

    }

    int GetAmountOfStocks(int _indexOfWorksheet)
    {

      int _amountOfstocks = 0;
      int _currLine;

      Console.WriteLine("-------------GetAmountOfStocks-------------");

      // Get worksheet "Investment Aufteilung"
      oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(_indexOfWorksheet);
      Console.WriteLine("Name of worksheet: " + oSheet.Name);


      var _cellOfISIN = GetCellByName("Aktie/ETF (ISIN)");

      if (_cellOfISIN.cellColum == 0 || _cellOfISIN.cellLine == 0)
      {
        Console.WriteLine("Cell of ISIN was not found");
      }
      else
      {
        Console.WriteLine("Cell of ISIN was found");
        //Get the amount of stock
        _currLine = _cellOfISIN.cellLine + 1;
        Console.WriteLine("Start line count stock: " + _currLine);
        while (oSheet.Cells[_currLine, _cellOfISIN.cellColum].Value != null)
        {
          _currLine++;
          // Console.WriteLine("Value: " + oSheet.Cells[_cellOfISIN.cellLine, _cellOfISIN.cellColum].Value.ToString());
          _amountOfstocks++;
        }

        Console.WriteLine("Amount of stocks " + _amountOfstocks);
        return _amountOfstocks;
      }

      return 0;

    }

    (int cellLine, int cellColum) GetCellByName(string _name)
    {
      int _columOfName = 0;
      int _lineOfName = 0;
      int _limitSearch = 50;
      int _colum;
      bool _cellWasNotFound = false;
      bool _cellWasFound = false;

      Console.WriteLine("-------------GetCellByName-------------");

      for (int _currLine = 1; _currLine < _limitSearch; _currLine++)
      {
        _cellWasNotFound = false;
        _colum = 1;

        for (int _currColum = 0; _currColum < _limitSearch; _currColum++)
        {
          try
          {
            if (oSheet.Cells[_currLine, _currColum].Value == _name)
            {
              _columOfName = _currColum;
              _lineOfName = _currLine;
              _cellWasFound = true;
              break;
            }
          }
          catch (Exception)
          {

            //throw;
            continue;
          }


        }

        //while ( oSheet.Cells[_currLine, _colum].Value != _name)
        //{
        //    _colum++;

        //    if (_colum > 1000)
        //    {
        //        //Console.WriteLine("Colum of ISIN was not found!");
        //        //throw new InvalidOperationException("Colum of ISIN was not found!");
        //        _cellWasNotFound = true;

        //        break;
        //    }
        //}

        if (_cellWasFound)
        {

          break;
        }

      }

      return (_lineOfName, _columOfName);
    }
    public static (int cellLine, int cellColum) GetCellByName(string _name, string _worksheetName)
    {
      int _columOfName = 0;
      int _lineOfName = 0;
      int _limitSearch = 25;
      int _colum;
      bool _cellWasNotFound = false;
      bool _cellWasFound = false;

      Console.WriteLine("-------------GetCellByName-------------");

      // Check first if the cell we are searching for is already in the list
      for (int i = 0; i < cellNames.Count; i++)
      {
        if (cellNames[i].worksheet.Equals(_worksheetName))
        {
          if (cellNames[i].nameOfCell.Equals(_name))
          {
            return (cellNames[i].cell.cellLine, cellNames[i].cell.cellColum);
          }
        }
      }

      // Check if the worksheet is aready open
      if (worksheetNameOld != _worksheetName)
      {
        Console.WriteLine("_worksheetName: " + _worksheetName);
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }

      if (!(oSheet is null))
      {
        for (int _currLine = 1; _currLine < _limitSearch; _currLine++)
        {
          _cellWasNotFound = false;
          _colum = 1;

          for (int _currColum = 0; _currColum < _limitSearch; _currColum++)
          {
            try
            {
              if (oSheet.Cells[_currLine, _currColum].Value == _name)
              {
                _columOfName = _currColum;
                _lineOfName = _currLine;
                _cellWasFound = true;

                // Save this cell for later seachring
                CellNames cell = new CellNames
                {
                  worksheet = _worksheetName,
                  nameOfCell = _name,
                  cell = (_lineOfName, _columOfName)

                };

                cellNames.Add(cell);

                break;
              }
            }
            catch (Exception)
            {

              //throw;
              continue;
            }


          }

          if (_cellWasFound)
          {

            break;
          }

        }
      }
      

      return (_lineOfName, _columOfName);
    }

    void SaveStockInfos()
    {
      Console.WriteLine("-------------SaveStockInfos-------------");

      this._stockISINs.Clear();
      this._stockNames.Clear();
      this._stockTypes.Clear();

      // Get worksheet "Investment Aufteilung"
      oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(itemNo_InvestmentAufteilung);

      this._amountOfstocks = GetAmountOfStocks(itemNo_InvestmentAufteilung);
      Console.WriteLine("Amount of stocks " + this._amountOfstocks);

      if (this._amountOfstocks > 0)
      {
        var _cellOfISIN = GetCellByName("Aktie/ETF (ISIN)");
        var _cellOfStockType = GetCellByName("ETF oder Aktie");
        var _cellOfStockName = GetCellByName("Aktie/ETF (Name)");

        if (_cellOfISIN.cellColum == 0 || _cellOfISIN.cellLine == 0)
        {
          Console.WriteLine("Cell of ISIN was not found");
        }
        else if (_cellOfStockType.cellColum == 0 || _cellOfStockType.cellLine == 0)
        {
          Console.WriteLine("Cell of stock type was not found");
        }
        else if (_cellOfStockName.cellColum == 0 || _cellOfStockName.cellLine == 0)
        {
          Console.WriteLine("Cell of stock name was not found");
        }
        else
        {
          for (int _currStock = _cellOfISIN.cellLine + 1; _currStock < (_amountOfstocks + _cellOfISIN.cellLine + 1); _currStock++)
          {
            this._stockISINs.Add(oSheet.Cells[_currStock, _cellOfISIN.cellColum].Value.ToString());
            this._stockNames.Add(oSheet.Cells[_currStock, _cellOfStockName.cellColum].Value.ToString());
            this._stockTypes.Add(oSheet.Cells[_currStock, _cellOfStockType.cellColum].Value.ToString());
          }
        }

        CopyStockInfosToEtf_Aktien_Infos();
      }
      else
      {
        Console.WriteLine("Amount of stocks is zero");
      }
    }

    public void RefreshStockPrices()
    {


      string _currURL = "";
      string _currStockType;
      string _strValueBeforePoint;
      string _strValueAfterPoint;
      string _currPriceString;
      double _intValueBeforePoint;
      double _intValueAfterPoint;
      double _intPriceMerged;
      int _startLine;


      Console.WriteLine("-------------RefreshStockPrices-------------");



      if (this._amountOfstocks > 0)
      {
        // Get worksheet "Etf_Aktien_Infos"
        oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(itemNo_Etf_Aktien_Infos);
        Console.WriteLine("Name of worksheet: " + oSheet.Name);

        var _cellOfURL = GetCellByName("URL bei finanzen.net");
        var _cellOfISIN = GetCellByName("Aktie/ETF (ISIN)");
        var _cellOfStockType = GetCellByName("ETF oder Aktie");

        _startLine = _cellOfURL.cellLine + 1;
        for (int _currStock = _startLine; _currStock < (this._amountOfstocks + _startLine); _currStock++)
        {
          if (oSheet.Cells[_currStock, _cellOfURL.cellColum].Value != null)
          {
            _currURL = oSheet.Cells[_currStock, _cellOfURL.cellColum].Value.ToString();

            this._urls.Add(_currURL);
            this._priceOfStocks.Add(webCrawler.GetStockPriceFromFinanzenNet(_currURL, this._stockTypes[_currStock - _startLine]));
          }
          else
          {
            this._urls.Add("No URL");
            this._priceOfStocks.Add("No URL");
          }
        }

        // Get worksheet "Investment Aufteilung"
        oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(itemNo_InvestmentAufteilung);

        _cellOfISIN = GetCellByName("Aktie/ETF (ISIN)");
        var _cellOfStockPrice = GetCellByName("Aktueller Kurs [€]");


        for (int _currStock = _startLine; _currStock < (this._amountOfstocks + _startLine); _currStock++)
        {
          _currPriceString = this._priceOfStocks[_currStock - _startLine];

          if (_currPriceString != null && _currPriceString.IndexOf(',') >= 0)
          {
            //Save the part of the price before the point
            //E.g. the price is "100,88" then save the 100
            _strValueBeforePoint = _currPriceString.Substring(0, _currPriceString.IndexOf(','));
            //Save the part of the price after the point
            //E.g. the price is "100,88" then save the 88
            _strValueAfterPoint = _currPriceString.Substring(_currPriceString.IndexOf(',') + 1, _currPriceString.Length - (_currPriceString.IndexOf(',') + 1));

            //Convert it from a text to a number
            _intValueBeforePoint = Convert.ToDouble(_strValueBeforePoint);
            _intValueAfterPoint = Convert.ToDouble(_strValueAfterPoint);
            //Merge the two parts to one value
            _intPriceMerged = _intValueBeforePoint + (_intValueAfterPoint * (1 / (Math.Pow(10, _strValueAfterPoint.Length))));

            Console.WriteLine("_strValueBeforePoint: " + _strValueBeforePoint);
            Console.WriteLine("_strValueAfterPoint: " + _strValueAfterPoint);
            Console.WriteLine("_intValueBeforePoint: " + _intValueBeforePoint);
            Console.WriteLine("_intValueAfterPoint: " + _intValueAfterPoint);
            Console.WriteLine("_intPriceMerged: " + _intPriceMerged);

            // Console.WriteLine("pause");

            oSheet.Cells[_currStock, _cellOfStockPrice.cellColum].Value = _intPriceMerged;
          }
          else
          {
            oSheet.Cells[_currStock, _cellOfStockPrice.cellColum].Value = "Preis nicht gefunden";
          }

        }
      }



    }

    public void ExtractDataFromFile(string _fileAsPath)
    {

      Excel.Application xlApp2;
      Excel.Workbook oWB2;
      Excel.Worksheet oSheet2;

      Console.WriteLine("-------------OpenExcelFile-------------");

      //Start Excel and get Application object.
      xlApp2 = new Excel.Application();
      xlApp2.Visible = true;
      oWB2 = (Excel.Workbook)xlApp2.Workbooks.Open(_fileAsPath);




    }

    public void RefreshCryptoPrices()
    {
      Console.WriteLine("-------------RefreshCryptoPrices-------------");

      string coinGeckoAPI_url_before_id = "https://api.coingecko.com/api/v3/simple/price?ids=";
      string coinGeckoAPI_url_before_after = "&vs_currencies=eur";

      // Get worksheet "Mein Bestand"
      oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(this.itemNo_Mein_Bestand);

      var cellOfAPI_ID = GetCellByName("CoinGecko API ID");
      var cellOfAnzahlDerCoins = GetCellByName("Anzahl der Coins");
      var cellOfAktuellerPreis = GetCellByName("Aktueller Preis [€]");
      var cellOfKapitalInsgesamt = GetCellByName("Kapital Insgesamt [€]");
      oSheet.Cells[cellOfKapitalInsgesamt.cellLine, cellOfKapitalInsgesamt.cellColum + 1].Value = 0;
      var cellOfStückzahlInsgesamt = GetCellByName("Stückzahl Insgesamt");
      var cellOfCurrentCrypto = GetCellByName("Lezter Ausgeführter Coin:");
      var cellOfCurrentCryptoLine = GetCellByName("Zeile des letzten ausgeführten Coins:");
      double currCryptoPrice;

      Console.WriteLine("Count of cryptos: " + oSheet.Cells[cellOfAnzahlDerCoins.cellLine + 1, cellOfAnzahlDerCoins.cellColum].Value);
      double countOfCryptos = oSheet.Cells[cellOfAnzahlDerCoins.cellLine + 1, cellOfAnzahlDerCoins.cellColum].Value;
      int startLine;
      if (oSheet.Cells[cellOfCurrentCrypto.cellLine, cellOfCurrentCrypto.cellColum + 1].Value != null)
      {
        startLine = oSheet.Cells[cellOfCurrentCryptoLine.cellLine, cellOfCurrentCryptoLine.cellColum + 1].Value;
      }
      else
      {
        startLine = cellOfAPI_ID.cellLine + 1;
      }

      for (int currStock = startLine; currStock < (countOfCryptos + startLine); currStock++)
      {
        if (oSheet.Cells[currStock, cellOfAPI_ID.cellColum].Value != null)
        {
          string currURL = coinGeckoAPI_url_before_id + oSheet.Cells[currStock, cellOfAPI_ID.cellColum].Value.ToString() + coinGeckoAPI_url_before_after;
          //Um den letzten ausgeführten Coin zu speichern
          // oSheet.Cells[cellOfCurrentCrypto.cellLine, cellOfCurrentCrypto.cellColum + 1] = oSheet.Cells[currStock, cellOfAPI_ID.cellColum].Value.ToString();
          //oSheet.Cells[cellOfCurrentCryptoLine.cellLine, cellOfCurrentCryptoLine.cellColum + 1].Value = currStock;

          this.coinGeckoAPIIDS.Add(currURL);
          // this.priceOfCryptos.Add(coinGeckoAPI.GetCurrentPrice(currURL));
          currCryptoPrice = Convert.ToDouble(coinGeckoAPI.GetCurrentPrice(currURL));//this.priceOfCryptos[currStock - startLine]);

          if (currCryptoPrice == null)
          {
            currStock--;
            System.Threading.Thread.Sleep(120000);
            break;
          }

          oSheet.Cells[currStock, cellOfAktuellerPreis.cellColum].Value = currCryptoPrice;
          oSheet.Cells[cellOfKapitalInsgesamt.cellLine, cellOfKapitalInsgesamt.cellColum + 1].Value += currCryptoPrice * oSheet.Cells[currStock, cellOfStückzahlInsgesamt.cellColum].Value;
        }
        else
        {
          this._urls.Add("No URL");
          this._priceOfStocks.Add("No URL");
        }
      }

    }


    public static double GetValueFromCell(int _cellLine, int _cellColum, string _worksheetName)//int _indexOfWorksheet)
    {
      

      if (worksheetNameOld != _worksheetName)
      {
        Console.WriteLine("_worksheetName: " + _worksheetName);
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }
        
      

      if (!(oSheet is null))
      {
        return oSheet.Cells[_cellLine, _cellColum].Value;
      }

      return -9999;
    }
    public static double GetDecimalFromCell(int _cellLine, int _cellColum, string _worksheetName)//int _indexOfWorksheet)
    {


      if (worksheetNameOld != _worksheetName)
      {
        Console.WriteLine("_worksheetName: " + _worksheetName);
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }



      if (!(oSheet is null))
      {
        return oSheet.Cells[_cellLine, _cellColum].Value;
      }

      return -9999;
    }
    public static string GetTextFromCell(int _cellLine, int _cellColum, string _worksheetName)//int _indexOfWorksheet)
    {

      if (worksheetNameOld != _worksheetName)
      {
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }

      if (!(oSheet is null))
      {
        if (oSheet.Cells[_cellLine, _cellColum].Value != null)
        {
          return string.Concat(oSheet.Cells[_cellLine, _cellColum].Value);
        }
        
      }

      return "";//return null;
    }
    public static DateTime GetDateFromCell(int _cellLine, int _cellColum, string _worksheetName)//int _indexOfWorksheet)
    {
      DateTime dateTime = new DateTime();

      if (worksheetNameOld != _worksheetName)
      {
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }

      if (!(oSheet is null))
      {
        if (oSheet.Cells[_cellLine, _cellColum].Value != null)
        {
          return oSheet.Cells[_cellLine, _cellColum].Value;
        }
        
      }

      return dateTime;
    }

    public static void SetTextInCell(string _text, int _cellLine, int _cellColum, string _worksheetName)
    {


      if (worksheetNameOld != _worksheetName)
      {
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }

      oSheet.Cells[_cellLine, _cellColum].Value = _text;

    }

    public static void SetValueInCell(double _value, int _cellLine, int _cellColum, string _worksheetName)
    {


      if (worksheetNameOld != _worksheetName)
      {
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }

      oSheet.Cells[_cellLine, _cellColum].Value = _value;

    }
    public static void ClearRange( string _cell1, string _cell2, string _worksheetName)
    {
      if (worksheetNameOld != _worksheetName)
      {
        // Get worksheet 
        for (int i = 1; i <= oWB.Worksheets.Count; i++)
        {
          oSheet = (Excel._Worksheet)oWB.Worksheets.get_Item(i);

          if (oSheet.Name == _worksheetName)
          {
            worksheetNameOld = _worksheetName;
            break;
          }
        }
      }
      oSheet.Range[_cell1, _cell2].Clear();
    }
  }

  
}
