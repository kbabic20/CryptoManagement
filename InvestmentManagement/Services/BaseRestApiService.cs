using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace InvestmentManagement.Services
{
  public class BaseRestApiService
  {

    class ApiResponseMapper<T>
    {
      public List<T> result { get; set; }
    }

    public async Task<List<T>> GetApiResultsAsListAsync<T>(string url)
    {
      List<T> resultList = new List<T>();

      using (HttpClient client = new HttpClient())
      {
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        HttpResponseMessage response = await client.GetAsync(url);



        if (response.IsSuccessStatusCode)
        {
          string responseContent = response.Content.ReadAsStringAsync().Result;
          if (responseContent[0].Equals('[') && responseContent.Last().Equals(']'))
          {

            // Deserialisierung der Antwort in die Liste von Txlist-Objekten
            resultList = JsonConvert.DeserializeObject<List<T>>(responseContent);
          }
          else
          {
            // Deserialisierung der Antwort in die Liste von Txlist-Objekten
            var result = JsonConvert.DeserializeObject<ApiResponseMapper<T>>(responseContent);

            if (result != null && result.result != null)
            {
              resultList = result.result;
            }
          }

          resultList = JsonConvert.DeserializeObject<List<T>>(responseContent);


        }
        else if ((int)response.StatusCode == 429)
        {
          // If the result is null wait 2 min and then try it again
          Console.WriteLine("Wait 2 min to continue Timestamp: " + DateTime.Now.ToString());
          System.Threading.Thread.Sleep(75000); //120000 ms
          resultList = await GetApiResultsAsListAsync<T>(url);
        }
        else
        {
          Console.WriteLine("Fehler beim Abrufen der Daten. Statuscode: " + response.StatusCode);
        }
      }

      return resultList;
    }
    public async Task<List<T>> GetApiResultsAsListAsync<T>(string url, Dictionary<string,string> header)
    {
      List<T> transactionList = new List<T>();

      using (HttpClient client = new HttpClient())
      {
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        if (!(header is null))
        {
          foreach (var item in header)
          {
            client.DefaultRequestHeaders.Add(item.Key, item.Value);
          }
        }

        HttpResponseMessage response = await client.GetAsync(url);

        

        if (response.IsSuccessStatusCode)
        {
          string responseContent = response.Content.ReadAsStringAsync().Result;

          if (true)
          {

          }
          if (responseContent[0].Equals('[') && responseContent.Last().Equals(']'))
          {

            // Deserialisierung der Antwort in die Liste von Txlist-Objekten
            transactionList = JsonConvert.DeserializeObject<List<T>>(responseContent);
          }
          else
          {
            // Deserialisierung der Antwort in die Liste von Txlist-Objekten
            var result = JsonConvert.DeserializeObject<ApiResponseMapper<T>>(responseContent);

            if (result != null && result.result != null)
            {
              transactionList = result.result;
            }
          }


          //transactionList = JsonConvert.DeserializeObject<List<T>>(responseContent);


        }
        else if ((int)response.StatusCode == 429)
        {
          // If the result is null wait 2 min and then try it again
          Console.WriteLine("Wait 2 min to continue Timestamp: " + DateTime.Now.ToString());
          System.Threading.Thread.Sleep(75000); //120000 ms
          transactionList = await GetApiResultsAsListAsync<T>(url);
        }
        else
        {
          Console.WriteLine("Fehler beim Abrufen der Daten. Statuscode: " + response.StatusCode);
        }
      }

      return transactionList;
    }
    public async Task<T> GetApiResultsAsync<T>(string url)
    {
      T responseResult = default;

      using (HttpClient client = new HttpClient())
      {
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        HttpResponseMessage response = await client.GetAsync(url);



        if (response.IsSuccessStatusCode)
        {
          string responseContent = response.Content.ReadAsStringAsync().Result;

          responseResult = JsonConvert.DeserializeObject<T>(responseContent);


        }
        else if ((int)response.StatusCode == 429)
        {
          // If the result is null wait 2 min and then try it again
          Console.WriteLine("Wait 2 min to continue Timestamp: " + DateTime.Now.ToString());
          System.Threading.Thread.Sleep(75000); //120000 ms
          responseResult = await GetApiResultsAsync<T>(url);
        }
        else
        {
          Console.WriteLine("Fehler beim Abrufen der Daten. Statuscode: " + response.StatusCode);
        }
      }

      return responseResult;
    }
    public async Task<T> GetApiResultsAsync<T>(string url, Dictionary<string, string> header)
    {
      T responseResult = default;

      using (HttpClient client = new HttpClient())
      {
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        if (!(header is null))
        {
          foreach (var item in header)
          {
            client.DefaultRequestHeaders.Add(item.Key, item.Value);
          }
        }

        HttpResponseMessage response = await client.GetAsync(url);



        if (response.IsSuccessStatusCode)
        {
          string responseContent = response.Content.ReadAsStringAsync().Result;

          responseResult = JsonConvert.DeserializeObject<T>(responseContent);


        }
        else if ((int)response.StatusCode == 429)
        {
          // If the result is null wait 2 min and then try it again
          Console.WriteLine("Wait 2 min to continue Timestamp: " + DateTime.Now.ToString());
          System.Threading.Thread.Sleep(75000); //120000 ms
          responseResult = await GetApiResultsAsync<T>(url,header);
        }
        else
        {
          Console.WriteLine("Fehler beim Abrufen der Daten. Statuscode: " + response.StatusCode);
        }
      }

      return responseResult;
    }
    public async Task<Dictionary<string, Dictionary<string, T>>> GetApiResultsDicAsync<T>(string url)
    {
      Dictionary<string, Dictionary<string, T>> transactionList = new Dictionary<string, Dictionary<string, T>>();

      using (HttpClient client = new HttpClient())
      {
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        HttpResponseMessage response = await client.GetAsync(url);



        if (response.IsSuccessStatusCode)
        {
          string responseContent = response.Content.ReadAsStringAsync().Result;
          //if (responseContent[0].Equals('[') && responseContent.Last().Equals(']'))
          //{

          //  // Deserialisierung der Antwort in die Liste von Txlist-Objekten
          //  transactionList = JsonConvert.DeserializeObject<Dictionary<string, T>>(responseContent);
          //}
          //else
          //{
          //  // Deserialisierung der Antwort in die Liste von Txlist-Objekten
          //  var result = JsonConvert.DeserializeObject<ApiResponseMapper<T>>(responseContent);

          //  if (result != null && result.result != null)
          //  {
          //    transactionList = result.result;
          //  }
          //}

          // Deserialisierung der Antwort in die Liste von Txlist-Objekten
          transactionList = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, T>>>(responseContent);


        }
        else if ((int)response.StatusCode == 429)
        {
          // If the result is null wait 2 min and then try it again
          Console.WriteLine("Wait 2 min to continue Timestamp: " + DateTime.Now.ToString());
          System.Threading.Thread.Sleep(75000); //120000 ms
          transactionList = await GetApiResultsDicAsync<T>(url);
        }
        else
        {
          Console.WriteLine("Fehler beim Abrufen der Daten. Statuscode: " + response.StatusCode);
        }
      }

      return transactionList;
    }
    public async Task<List<T>> GetApiResultsRestSharperAsync<T>(string url)
    {
      List<T> transactionList = new List<T>();

      using (RestClient client = new RestClient(url))
      {
        var request = new RestRequest("",Method.Get);

        request.AddQueryParameter("vs_currency", "usd");
        request.AddQueryParameter("order", "market_cap_desc");
        request.AddQueryParameter("per_page", "100");
        request.AddQueryParameter("page", "1");

        var response = client.Execute(request);
        if (response.IsSuccessStatusCode)
        {
          string responseContent = response.Content;
          //if (responseContent[0].Equals('[') && responseContent.Last().Equals(']'))
          //{

          //  // Deserialisierung der Antwort in die Liste von Txlist-Objekten
          //  transactionList = JsonConvert.DeserializeObject<List<T>>(responseContent);
          //}
          //else
          //{
          //  // Deserialisierung der Antwort in die Liste von Txlist-Objekten
          //  var result = JsonConvert.DeserializeObject<ApiResponseMapper<T>>(responseContent);

          //  if (result != null && result.result != null)
          //  {
          //    transactionList = result.result;
          //  }
          //}



        }
        else
        {
          Console.WriteLine("Fehler beim Abrufen der Daten. Statuscode: " + response.StatusCode);
        }

        ////var client = new RestClient("https://api.example.com");
        //var request = new RestRequest("/resource", Method.GET);

        //IRestResponse response = client.Execute(request);
        //Console.WriteLine(response.Content);
      }

      return transactionList;
    }

  }
}
