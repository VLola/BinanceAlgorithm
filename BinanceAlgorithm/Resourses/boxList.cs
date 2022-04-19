using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BinanceAlgorithm
{
    public class boxList
    {
        public ObservableCollection<string> cmbContentFileNames { get; set; }
        public boxList(List<string> list)
        {
            cmbContentFileNames = new ObservableCollection<string>();
            foreach (var it in list)
            {
                cmbContentFileNames.Add(it);
            }
        }
    }
}
