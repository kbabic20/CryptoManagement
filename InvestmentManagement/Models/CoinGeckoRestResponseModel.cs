using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManagement.Models
{
  public class CoinGeckoRestResponseModel
  {

    public class CoinsList
    {
      public string Id { get; set; }
      public string Symbol { get; set; }
      public string Name { get; set; }
      public Dictionary<string, string> Platforms { get; set; }
    }

    //public class Class1
    //{
    //  public string id { get; set; }
    //  public string symbol { get; set; }
    //  public string name { get; set; }
    //  public Platforms platforms { get; set; }
    //}
    public class Platforms
    {
      public List<string> Network { get; set; }
    }
    //public class Platforms
    //{
    //  public string ethereum { get; set; }
    //  public string polygonpos { get; set; }
    //  public string energi { get; set; }
    //  public string harmonyshard0 { get; set; }
    //  public string avalanche { get; set; }
    //  public string fantom { get; set; }
    //  public string binancesmartchain { get; set; }
    //  public string xdai { get; set; }
    //  public string aurora { get; set; }
    //  public string nearprotocol { get; set; }
    //  public string arbitrumone { get; set; }
    //  public string solana { get; set; }
    //  public string klaytoken { get; set; }
    //  public string tron { get; set; }
    //  public string pulsechain { get; set; }
    //  public string cardano { get; set; }
    //  public string optimisticethereum { get; set; }
    //  public string sora { get; set; }
    //  public string huobitoken { get; set; }
    //  public string _base { get; set; }
    //  public string metisandromeda { get; set; }
    //  public string conflux { get; set; }
    //  public string aptos { get; set; }
    //  public string polkadot { get; set; }
    //  public string moonbeam { get; set; }
    //  public string chiliz { get; set; }
    //  public string boba { get; set; }
    //  public string kava { get; set; }
    //  public string komodo { get; set; }
    //  public string Bitcichain { get; set; }
    //  public string zksync { get; set; }
    //  public string elrond { get; set; }
    //  public string ardor { get; set; }
    //  public string qtum { get; set; }
    //  public string stellar { get; set; }
    //  public string sui { get; set; }
    //  public string celo { get; set; }
    //  public string cronos { get; set; }
    //  public string osmosis { get; set; }
    //  public string secret { get; set; }
    //  public string bitgert { get; set; }
    //  public string stacks { get; set; }
    //  public string algorand { get; set; }
    //  public string moonriver { get; set; }
    //  public string eos { get; set; }
    //  public string opbnb { get; set; }
    //  public string kusama { get; set; }
    //  public string cosmos { get; set; }
    //  public string theopennetwork { get; set; }
    //  public string terra { get; set; }
    //  public string polygonzkevm { get; set; }
    //  public string shimmer_evm { get; set; }
    //  public string telos { get; set; }
    //  public string flarenetwork { get; set; }
    //  public string core { get; set; }
    //  public string evmos { get; set; }
    //  public string arbitrumnova { get; set; }
    //  public string kardiachain { get; set; }
    //  public string astar { get; set; }
    //  public string okexchain { get; set; }
    //  public string songbird { get; set; }
    //  public string terra2 { get; set; }
    //  public string proofofmemes { get; set; }
    //  public string velas { get; set; }
    //  public string oasis { get; set; }
    //  public string ronin { get; set; }
    //  public string mantle { get; set; }
    //  public string linea { get; set; }
    //  public string alephium { get; set; }
    //  public string icon { get; set; }
    //  public string ordinals { get; set; }
    //  public string nem { get; set; }
    //  public string binancecoin { get; set; }
    //  public string thundercore { get; set; }
    //  public string iotex { get; set; }
    //  public string elastos { get; set; }
    //  public string milkomedacardano { get; set; }
    //  public string theta { get; set; }
    //  public string meter { get; set; }
    //  public string fuse { get; set; }
    //  public string hederahashgraph { get; set; }
    //  public string hoo { get; set; }
    //  public string kucoincommunitychain { get; set; }
    //  public string bittorrent { get; set; }
    //  public string ethereumpow { get; set; }
    //  public string xdcnetwork { get; set; }
    //  public string zilliqa { get; set; }
    //  public string oasys { get; set; }
    //  public string shibarium { get; set; }
    //  public string lightlink { get; set; }
    //  public string scroll { get; set; }
    //  public string mantapacific { get; set; }
    //  public string starknet { get; set; }
    //  public string nuls { get; set; }
    //  public string rootstock { get; set; }
    //  public string mixinnetwork { get; set; }
    //  public string canto { get; set; }
    //  public string smartbch { get; set; }
    //  public string fusionnetwork { get; set; }
    //  public string hydra { get; set; }
    //  public string tomochain { get; set; }
    //  public string neo { get; set; }
    //  public string xrp { get; set; }
    //  public string tezos { get; set; }
    //  public string syscoin { get; set; }
    //  public string stepnetwork { get; set; }
    //  public string dogechain { get; set; }
    //  public string defikingdomsblockchain { get; set; }
    //  public string bitkubchain { get; set; }
    //  public string factom { get; set; }
    //  public string ethereumclassic { get; set; }
    //  public string vechain { get; set; }
    //  public string waves { get; set; }
    //  public string neutron { get; set; }
    //  public string bitcoincash { get; set; }
    //  public string empire { get; set; }
    //  public string ergo { get; set; }
    //  public string kujira { get; set; }
    //  public string everscale { get; set; }
    //  public string exosama { get; set; }
    //  public string findora { get; set; }
    //  public string flow { get; set; }
    //  public string godwoken { get; set; }
    //  public string coinexsmartchain { get; set; }
    //  public string trustlesscomputer { get; set; }
    //  public string stratis { get; set; }
    //  public string cube { get; set; }
    //  public string shidennetwork { get; set; }
    //  public string tombchain { get; set; }
    //  public string sxnetwork { get; set; }
    //  public string rollux { get; set; }
    //  public string bitrock { get; set; }
    //  public string ontology { get; set; }
    //  public string eosevm { get; set; }
    //  public string omni { get; set; }
    //  public string bitshares { get; set; }
    //  public string wanchain { get; set; }
    //  public string clover { get; set; }
    //  public string functionx { get; set; }
    //  public string beam { get; set; }
    //  public string skale { get; set; }
    //  public string callisto { get; set; }
    //  public string tenet { get; set; }
    //  public string neonevm { get; set; }
    //  public string thorchain { get; set; }
    //  public string gochain { get; set; }
    //  public string celernetwork { get; set; }
    //  public string vite { get; set; }
    //  public string onus { get; set; }
    //  public string wemixnetwork { get; set; }
    //  public string _ { get; set; }
    //  public string enqenecuum { get; set; }
    //}


    public class CoinsMarkets
    {
      public string Id { get; set; }
      public string Symbol { get; set; }
      public string Name { get; set; }
      public string Image { get; set; }
      public decimal? Current_price { get; set; }
      public float? Market_cap { get; set; }
      public object Market_cap_rank { get; set; }
      public float? Fully_diluted_valuation { get; set; }
      public float? Total_volume { get; set; }
      public float? High_24h { get; set; }
      public float? Low_24h { get; set; }
      public float? Price_change_24h { get; set; }
      public float? Price_change_percentage_24h { get; set; }
      public float? Market_cap_change_24h { get; set; }
      public float? Market_cap_change_percentage_24h { get; set; }
      public float? Circulating_supply { get; set; }
      public float? Total_supply { get; set; }
      public float? Max_supply { get; set; }
      public float? Ath { get; set; }
      public float? Ath_change_percentage { get; set; }
      public DateTime? Ath_date { get; set; }
      public float? Atl { get; set; }
      public float? Atl_change_percentage { get; set; }
      public DateTime? Atl_date { get; set; }
      public Roi Roi { get; set; }
      public DateTime? Last_updated { get; set; }
    }

    public class Roi
    {
      public float Times { get; set; }
      public string Currency { get; set; }
      public float Percentage { get; set; }
    }

  }
}
