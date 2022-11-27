using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace InvestmentManagement
{
    public class WebCrawler
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        string _url = "https://www.onvista.de/etf/iShares-Core-S-P-500-ETF-IE00B5BMR087";//"https://www.onvista.de/aktien/IE00B5BMR087"; // * ISIN of the Stock
        HttpClient _httpClient = new HttpClient();
        string _stockPrice;

        private ObservableCollection<EntryModel> _entries = new ObservableCollection<EntryModel>();

        public ObservableCollection<EntryModel> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        public async Task DoStuff()
        {
            AllocConsole();

            Console.WriteLine("Bin drin");

            var _html = await _httpClient.GetStringAsync(_url);

            Console.WriteLine("Nach AWAIT");

            var _htmlDocument = new HtmlDocument();
            _htmlDocument.LoadHtml(_html);

            Console.WriteLine(_htmlDocument.Text); 

            var _datas = _htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("inner-spacing--medium-left-sm inner-spacing--medium-left-sm inner-spacing--medium-right-sm inner-spacing--xsmall-bottom-sm inner-spacing--none-top col col-12 ov-fullwidth-sm ov-snapshot__quote-bar-wrapper")).ToList();


            Console.WriteLine("Count of _datas: " + _datas.Count);


            foreach (var _data in _datas)
            {
                _stockPrice = _data.GetAttributeValue("value", "");

                Console.WriteLine("Stock Price: " + _stockPrice);
            }
        }

        public void DoStuff2(string _page)
        {
            var _web = new HtmlWeb();
            var _doc = _web.Load(_page);

            var _node = _doc.DocumentNode.SelectNodes("//*[@class = 'ov-content grid-container grid-container--limited-lg']");
            var _node2 = _doc.DocumentNode.Descendants("span").ToList();

            Console.WriteLine("Text: " +_doc.Text);

            if (_doc.Text.Contains("\"idTypePrice\":1, \"last\":381"))
            {
                Console.WriteLine( "Gefunden");
            }

            //if (_node != null)
            //{
            //    Console.WriteLine("Count:  " + _node.Count);

            //    Console.WriteLine("Count:  " + _node[0].ChildNodes.Count);

            //    foreach (var child in _node[0].ChildNodes)
            //    {
            //        Console.WriteLine("Name: " + child.GetAttributeValue("class",""));
            //        Console.WriteLine("Count of ChildChild: " + child.ChildNodes.Count);

            //        foreach (var childChild in child.ChildNodes)
            //        {
            //            Console.WriteLine("Innertext: " + childChild.InnerText);
            //        }

                    
            //    }
            //    Console.WriteLine("Count node2: " + _node2.Count);

            //    foreach (var child2 in _node2)
            //    {
            //        Console.WriteLine("Innertext child 2: " + child2.InnerText);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Node is null");
            //}
            
        }

        public string GetTickerFromISIN(string _page)
        {
            Console.WriteLine("-------------GetTickerFromISIN-------------");
            var _web = new HtmlWeb();
            var _doc = _web.Load(_page);

            var _node = _doc.DocumentNode.SelectNodes("//*[@class = 'col-auto ng-star-inserted']");


            if (_node != null)
            {
                foreach (var item in _node)
                {
                    Console.WriteLine("Innertext: " + item.InnerText);
                    Console.WriteLine("Innertext Sub: " + item.InnerText.Substring(item.InnerText.IndexOf(':') + 2));
                    return item.InnerText.Substring(item.InnerText.IndexOf(':') + 2);
                }
            }
            else
            {
                Console.WriteLine("class not found");
            }

            return null;
        }

        public string GetStockPriceFromFinanzenNet(string _url, string _stockType)
        {
            Console.WriteLine("-------------GetStockPriceFromFinanzenNet-------------");
            string _nodeInnertext;
            HtmlNodeCollection _nodes;
            var _web = new HtmlWeb();
            var _doc = _web.Load(_url);

            if (_url.Length > 0)
            {
                if (_stockType == "ETF")
                {
                    _nodes = _doc.DocumentNode.SelectNodes("//*[@class = 'table-responsive quotebox']");

                    if (_nodes != null)
                    {
                        for (int _currNodeNo = 0; _currNodeNo < _nodes.Count; _currNodeNo++)
                        {
                            Console.WriteLine("Name node: " + _nodes[_currNodeNo].Name);
                            Console.WriteLine("Child node count: " + _nodes[_currNodeNo].ChildNodes.Count);
                            Console.WriteLine("?: " + _nodes[_currNodeNo].ChildNodes[0].InnerText);

                            var _childNodes = _nodes[_currNodeNo].ChildNodes;

                            for (int _currChildNodeNo = 0; _currChildNodeNo < _childNodes.Count; _currChildNodeNo++)
                            {
                                Console.WriteLine("Name child node: " + _childNodes[_currChildNodeNo].Name);
                                Console.WriteLine("count childs under node: " + _childNodes[_currChildNodeNo].ChildNodes.Count);
                                if (_childNodes[_currChildNodeNo].Name == "table")
                                {
                                    var _childChildNodes = _childNodes[_currChildNodeNo].ChildNodes;

                                    for (int _currChildChildNodeNo = 0; _currChildChildNodeNo < _childChildNodes.Count; _currChildChildNodeNo++)
                                    {

                                        Console.WriteLine("Name child child node: " + _childChildNodes[_currChildChildNodeNo].Name);

                                        if (_childChildNodes[_currChildChildNodeNo].Name == "tr")
                                        {
                                            var _childChildChildNodes = _childChildNodes[_currChildChildNodeNo].ChildNodes;

                                            for (int _currChildChildChildNodeNo = 0; _currChildChildChildNodeNo < _childChildChildNodes.Count; _currChildChildChildNodeNo++)
                                            {
                                                _nodeInnertext = _childChildChildNodes[_currChildChildChildNodeNo].InnerText;
                                                //Console.WriteLine("Name child child child node: " + _childChildChildNodes[_currChildChildChildNodeNo].Name);
                                                //Console.WriteLine("Innertext child child child node: " + _nodeInnertext);

                                                if (_nodeInnertext.Substring(_nodeInnertext.Length - 3) == "EUR")
                                                {
                                                    Console.WriteLine("Gefunden");
                                                    Console.WriteLine(_nodeInnertext);
                                                    return _nodeInnertext.Substring(0, _nodeInnertext.Length - 3);
                                                }

                                            }
                                        }
                                    }
                                }


                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("Nodes für Aktie/URL " + _url + " wurde nicht gefunden.");
                    }
                }
                else if (_stockType == "Aktie")
                {
                    _nodes = _doc.DocumentNode.SelectNodes("//*[@class = 'col-xs-5 col-sm-4 text-sm-right text-nowrap']");

                    if (_nodes != null)
                    {
                        for (int _currNodeNo = 0; _currNodeNo < _nodes.Count; _currNodeNo++)
                        {
                            Console.WriteLine("Name node: " + _nodes[_currNodeNo].Name);
                            Console.WriteLine("Child node count: " + _nodes[_currNodeNo].ChildNodes.Count);
                            Console.WriteLine("?: " + _nodes[_currNodeNo].ChildNodes[0].InnerText);

                            return _nodes[_currNodeNo].ChildNodes[0].InnerText;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nodes für Aktie/URL " + _url + " wurde nicht gefunden.");
                    }


                }
                else
                {
                    Console.WriteLine("Keinen Aktien Typ angegeben!");
                }
            }

           

            

            return null;
        }

        public void OpenWebsite()
        {
            Console.WriteLine("-------------OpenWebsite-------------");

            string _page = "https://www.finanzen.net/";
            System.Diagnostics.Process.Start("https://www.finanzen.net/");

            var _web = new HtmlWeb();
            var _doc = _web.Load(_page);

            var _nodes = _doc.DocumentNode.SelectNodes("//*[@class = 'input-field__text-input']");

            if (_nodes != null)
            {
                for (int _currNodeNo = 0; _currNodeNo < _nodes.Count; _currNodeNo++)
                {
                    Console.WriteLine("Name: " + _nodes[_currNodeNo].Name);
                    Console.WriteLine("Att placeholder: " + _nodes[_currNodeNo].GetAttributeValue("placeholder", ""));

                    _nodes[_currNodeNo].SetAttributeValue("value", "DE0007100000");

                    Console.WriteLine("Att value: " + _nodes[_currNodeNo].GetAttributeValue("value", ""));
                   
                }
                var _nodes2 = _doc.DocumentNode.SelectNodes("//*[@id= 'form-submit']");
                if (_nodes2 != null)
                {
                    for (int _currNodeNo = 0; _currNodeNo < _nodes2.Count; _currNodeNo++)
                    {
                        Console.WriteLine("Name: " + _nodes2[_currNodeNo].Name);
                        Console.WriteLine("Att type: " + _nodes2[_currNodeNo].GetAttributeValue("type", ""));

                        _nodes2[_currNodeNo].SetAttributeValue("value", "TRUE");

                        Console.WriteLine("Att value: " + _nodes2[_currNodeNo].GetAttributeValue("value", ""));

                    }
                }
            }
            else
            {
                Console.WriteLine("node is null");
            }
        }
        
    }
}
