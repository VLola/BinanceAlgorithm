using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAlgorithm.Resourses
{
    public class ListEma
    {
        public string Sumbol { get; set; }
        public List<decimal> list { get; set; }
        public ListEma(string Sumbol, List<decimal> list)
        {
            this.Sumbol = Sumbol;
            this.list = list;
        }
    }
}
