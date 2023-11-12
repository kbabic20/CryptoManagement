using System.Threading.Tasks;

namespace InvestmentManagement.Services
{
  public class AvalancheScanService : BaseBlockScanService
  {

    const string baseUrl = "https://api.snowtrace.io/api";
    const string apiKey = "1GJCA6VZBT4S8QXVIPYX1AARFDGSV11ET1";
    const string walletAddress = "0x2c8Ac232c76498fE46811879D20cE34B92983A9e";

    public override string BaseUrl { get => baseUrl; }
    public override string ApiKey { get => apiKey; }
    public override string WalletAddress { get => walletAddress; }

    const string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Avalanche Network";

    public async Task GetNetworkTxn()
    {
      await GetDataAndSaveToCsv(filePathRoot);
    }
  }
}
