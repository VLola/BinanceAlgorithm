using System.Collections.Generic;

namespace BinanceAlgorithm
{
    public class ListKlines
    {
        public string symbol { get; set; }
        public List<Kline> listKlines { get; set; }
        public ListKlines(string symbol, List<Kline> listKlines)
        {
            this.symbol = symbol;
            this.listKlines = listKlines;
        }

    }

}
