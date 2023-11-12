using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement.Services
{
  public class BlockScanService
  {
    ArbitrumScanService arbitrumScanService = new ArbitrumScanService();
    AvalancheScanService avalancheScanService = new AvalancheScanService();
    BscScanService bscScanService = new BscScanService();
    EthereumScanService etherScanService = new EthereumScanService();
    FantomScanService fantomScanService = new FantomScanService();
    MoonbeamScanService moonbeamScanService = new MoonbeamScanService();
    OptimismScanService optimismScanService = new OptimismScanService();
    PolygonScanService polygonScanService = new PolygonScanService();

    public async Task GetBlockScannerNetworkTxn()
    {
      //await arbitrumScanService.GetNetworkTxn();
      //await avalancheScanService.GetNetworkTxn();
      //await bscScanService.GetNetworkTxn();
      //await etherScanService.GetNetworkTxn();
      //await fantomScanService.GetNetworkTxn();
      //await moonbeamScanService.GetNetworkTxn();
      //await optimismScanService.GetNetworkTxn();
      //await polygonScanService.GetNetworkTxn();

      //Task task1 = arbitrumScanService.GetNetworkTxn();
      //Task task2 = avalancheScanService.GetNetworkTxn();
      //Task task3 = bscScanService.GetNetworkTxn();
      //Task task4 = etherScanService.GetNetworkTxn();
      //Task task5 = fantomScanService.GetNetworkTxn();
      //Task task6 = moonbeamScanService.GetNetworkTxn();
      //Task task7 = optimismScanService.GetNetworkTxn();
      //Task task8 = polygonScanService.GetNetworkTxn();
      Task[] tasks = { arbitrumScanService.GetNetworkTxn(),
                       avalancheScanService.GetNetworkTxn(),
                        bscScanService.GetNetworkTxn(),
                        etherScanService.GetNetworkTxn(),
                        fantomScanService.GetNetworkTxn(),
                        moonbeamScanService.GetNetworkTxn(),
                        optimismScanService.GetNetworkTxn(),
                        polygonScanService.GetNetworkTxn()};

      Task.WhenAll(tasks);

    }
  }
}
