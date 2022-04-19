using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAlgorithm.Resourses
{
    public class ObjectListEma
    {
        public int PriceChange { get; set; }
        public List<ListEma> Ema { get; set; }
        public ObjectListEma(int PriceChange, List<ListEma> Ema)
        {
            this.PriceChange = PriceChange;
            this.Ema = Ema;
        }
    }
}
