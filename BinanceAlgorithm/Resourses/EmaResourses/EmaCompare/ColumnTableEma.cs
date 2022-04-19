using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAlgorithm.Resourses.EmaResourses
{
    public class ColumnTableEma
    {
        public string ema_1 { get; set; }
        public string ema_2 { get; set; }
        public ColumnTableEma(string ema)
        {
            foreach(char it in ema)
            {
                if (it == '-') break;
                ema_1 += it;
            }
            bool check = false;
            foreach (char it in ema)
            {
                if (check) ema_2 += it;
                if (it == '-') check = true;
            }

        }
    }
}
