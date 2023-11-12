using System.Threading.Tasks;

namespace InvestmentManagement.Services
{
  public class BscScanService : BaseBlockScanService
  {

    const string baseUrl = "https://api.bscscan.com/api";
    const string apiKey = "W6UHYMCWEYJRB82NVG4448GEG6AX7KUD5B";
    const string walletAddress = "0x2c8Ac232c76498fE46811879D20cE34B92983A9e";

    public override string BaseUrl { get => baseUrl; }
    public override string ApiKey { get => apiKey; }
    public override string WalletAddress { get => walletAddress; }

    const string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Bsc Network";

    public async Task GetNetworkTxn()
    {
      await GetDataAndSaveToCsv(filePathRoot);
    }
  }
}
