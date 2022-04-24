using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BinanceAlgorithm.Resourses.EmaResourses
{
    public class EmaCompare
    {
        public ObservableCollection<string> Compare1 { get; set; }
        public ObservableCollection<string> Compare2 { get; set; }
        public EmaCompare(List<string> list)
        {
            Compare1 = new ObservableCollection<string>();
            Compare2 = new ObservableCollection<string>();
            foreach (var it in list)
            {
                Compare1.Add(it);
                Compare2.Add(it);
            }
        }
    }
}
