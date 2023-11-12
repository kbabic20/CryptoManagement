using System.Threading.Tasks;

namespace InvestmentManagement.Services
{
  public class MoonbeamScanService : BaseBlockScanService
  {

    const string baseUrl = "https://api-moonbeam.moonscan.io/api";
    const string apiKey = "9RP8SR5FDE3Q2IZY8HJSJUG4CTIIZBIM8W";
    const string walletAddress = "0x2c8Ac232c76498fE46811879D20cE34B92983A9e";

    public override string BaseUrl { get => baseUrl; }
    public override string ApiKey { get => apiKey; }
    public override string WalletAddress { get => walletAddress; }

    const string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Moonbeam Network";

    public async Task GetNetworkTxn()
    {
      await GetDataAndSaveToCsv(filePathRoot);
    }
  }
}
