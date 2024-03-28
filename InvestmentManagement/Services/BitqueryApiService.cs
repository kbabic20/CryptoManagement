using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace InvestmentManagement.Services
{
  class BitqueryApiService
  {
    const string apiKey = "BQYarm2ooiEOZ7mzqfLNpzSpGrkbtTxl";
    const string solanaAddress = "98VqVrteynhFauNcDCy6GekgCQpZCZyquF7EQvZVDAsK";

    string apiUrl = $"https://api.bitquery.io/v1/graphql?apikey={apiKey}";

    // Hier den GraphQL-Query für die gewünschten Daten anpassen
    string query = $@"
        {{
          ethereum(network: solana) {{
            transactions(
              options: {{ desc: ""block.timestamp"", limit: 10 }},
              sender: {{ is: ""{solanaAddress}"" }}
            ) {{
              hash
              block {{
                timestamp {{
                  time(format: ""%Y-%m-%d %H:%M:%S"")
                }}
              }}
            }}
          }}
        }}
        ";

    public async Task SendQuery()
    {
      // HTTP-Anfrage erstellen
      using (HttpClient client = new HttpClient())
      {
        // GraphQL-Query als JSON-Objekt erstellen
        var requestBody = new StringContent($"{{\"query\":\"{query}\"}}");

        // HTTP-Post-Anfrage an die Bitquery-API senden
        var response = await client.PostAsync(apiUrl, requestBody);

        // Die Antwort als Text abrufen
        var responseText = await response.Content.ReadAsStringAsync();

        // Hier kannst du die Antwort verarbeiten, z.B. in ein Datenmodell umwandeln
        // ...

        Console.WriteLine(responseText);
      }
    }
  }
}
