using System.Collections.Generic;
using System.Threading.Tasks;


namespace InvestmentManagement.Services
{
  public class EtherScanApiResponse<T>
  {
    public List<T> result { get; set; }
  }
  public class EthereumScanService:BaseBlockScanService
  {

    const string baseUrl = "https://api.etherscan.io/api";
    const string apiKey = "9BREBCM8PSDB844MPFKFH3IX28VJF57AED";
    const string walletAddress = "0x2c8Ac232c76498fE46811879D20cE34B92983A9e";

    public override string BaseUrl { get => baseUrl; }
    public override string ApiKey { get => apiKey; }
    public override string WalletAddress { get => walletAddress; }
    const string filePathRoot = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente\Ethereum Network";

    public async Task GetNetworkTxn()
    {
      await GetDataAndSaveToCsv(filePathRoot);
    }
  }

    
}
