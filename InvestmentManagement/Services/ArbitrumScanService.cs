using System.Threading.Tasks;

namespace InvestmentManagement.Services
{
  public class ArbitrumScanService:BaseBlockScanService
  {

    const string baseUrl = "https://api.arbiscan.io/api";
    const string apiKey = "MHQ9BMYD3H5UTRS8Z6PYHRBISM1V8CKWNZ";
    const string walletAddress = "0x2c8Ac232c76498fE46811879D20cE34B92983A9e";

    public override string BaseUrl { get => baseUrl; }
    public override string ApiKey { get => apiKey;}
    public override string WalletAddress { get => walletAddress; }

    const string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Arbitrum Network";

    public async Task GetNetworkTxn()
    {
      await GetDataAndSaveToCsv(filePathRoot);
    }

   

  }
}
