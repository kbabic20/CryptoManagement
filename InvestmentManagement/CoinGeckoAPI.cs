using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Net;

namespace InvestmentManagement
{
  class CoinGeckoAPI
  {

    private string GetRequest(string _url)
    {
      
      HttpWebRequest req = WebRequest.Create(_url) as HttpWebRequest;
      string result = null;
      try
      {
        using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
        {
          StreamReader reader = new StreamReader(resp.GetResponseStream());
          result = reader.ReadToEnd();
        }
      }
      catch (WebException e)
      {

        return null;
      }



      return result;
    }


    public string GetCurrentPrice(string _url)
    {
      string result = null;
      result = GetRequest(_url);

      if (!(result is null))
      {
        result = result.Substring(result.IndexOf("eur\":") + 5);
        result = result.Substring(0, result.Length - 2);
        result = result.Replace(",", "");
        result = result.Replace(".", ",");
        Console.WriteLine("Price: " + result);
      }
      else
      {
        // If the result is null wait 2 min and then try it again
        Console.WriteLine("Wait 2 min to continue Timestamp: " + DateTime.Now.ToString());
        System.Threading.Thread.Sleep(120000); //120000 ms
        result = GetCurrentPrice(_url);
      }

      return result;
    }
  }
}
