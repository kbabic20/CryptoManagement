using System.Threading.Tasks;

namespace InvestmentManagement.Services
{
  public class OptimismScanService : BaseBlockScanService
  {

    const string baseUrl = "https://api-optimistic.etherscan.io/api";
    const string apiKey = "VA57V5UT27BKQAAS7Q8SWYMMD5ZJM3VB49";
    const string walletAddress = "0x2c8Ac232c76498fE46811879D20cE34B92983A9e";

    public override string BaseUrl { get => baseUrl; }
    public override string ApiKey { get => apiKey; }
    public override string WalletAddress { get => walletAddress; }

    const string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Optimism Network";

    public async Task GetNetworkTxn()
    {
      await GetDataAndSaveToCsv(filePathRoot);
    }
  }
}
