using System.Threading.Tasks;

namespace InvestmentManagement.Services
{
  public class FantomScanService : BaseBlockScanService
  {

    const string baseUrl = "https://api.ftmscan.com/api";
    const string apiKey = "7Z4QGCEQG5J9D6XMK83W6VXYXBVTKQ9VHC";
    const string walletAddress = "0x2c8Ac232c76498fE46811879D20cE34B92983A9e";

    public override string BaseUrl { get => baseUrl; }
    public override string ApiKey { get => apiKey; }
    public override string WalletAddress { get => walletAddress; }

    const string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Fantom Network";

    public async Task GetNetworkTxn()
    {
      await GetDataAndSaveToCsv(filePathRoot);
    }
  }
}
