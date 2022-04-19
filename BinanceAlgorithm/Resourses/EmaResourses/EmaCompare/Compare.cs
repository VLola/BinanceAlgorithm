using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAlgorithm.Resourses.EmaResourses
{
    class Compare
    {
        public List<string> list = new List<string>();
        private void EmaCompare(int compare_1, int compare_2)
        {
            try
            {
                list.Add($"{compare_1}-{compare_2}");
                DataTable dt = new DataTable("myTable");
                DataColumn column_sumbol = new DataColumn("Sumbol", typeof(string));
                dt.Columns.Add(column_sumbol);
                foreach (string it in list)
                {
                    DataColumn column = new DataColumn(it, typeof(string));
                    dt.Columns.Add(column);
                }
                string path_ema = System.IO.Path.Combine(Environment.CurrentDirectory, "ema");

                List<List<ObjectListEma>> listic = new List<List<ObjectListEma>>();
                foreach (string it in list)
                {
                    ColumnTableEma ema_name = new ColumnTableEma(it);
                    string json1 = File.ReadAllText(path_ema + "\\" + ema_name.ema_1);
                    string json2 = File.ReadAllText(path_ema + "\\" + ema_name.ema_2);
                    var list_ema1 = JsonConvert.DeserializeObject<ObjectListEma>(json1);
                    var list_ema2 = JsonConvert.DeserializeObject<ObjectListEma>(json2);
                    List<ObjectListEma> list_two_ema = new List<ObjectListEma>();
                    list_two_ema.Add(list_ema1);
                    list_two_ema.Add(list_ema2);
                    listic.Add(list_two_ema);
                    //for(int i = 0;i < list_ema1.Ema.Count ;i++)
                    //{
                    //    string sumbol = list_ema1.Ema[i].Sumbol;

                    //    for(int j = 0;j < list.Count; j++)
                    //    {
                    //        decimal a = list_ema1.Ema[i].list[0];
                    //        decimal b = list_ema2.Ema[i].list[0];
                    //        decimal result;
                    //        if (a == b) result = 0;
                    //        else if (a > b) result = (a - b) / a * 100;
                    //        else result = -(a - b) / a * 100;
                    //        dt.Rows.Add(sumbol, result);
                    //    }
                    //}
                }
                for (int i = 0; i < listic.Count; i++)
                {
                    string json1 = File.ReadAllText(path_ema + "\\" + listic[i][0].PriceChange.ToString());
                    string json2 = File.ReadAllText(path_ema + "\\" + listic[i][1].PriceChange.ToString());
                    var list_ema1 = JsonConvert.DeserializeObject<ObjectListEma>(json1);
                    var list_ema2 = JsonConvert.DeserializeObject<ObjectListEma>(json2);
                }

                //myDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception e)
            {
                ErrorText.Add($"EmaCompare {e.Message}");
            }
        }
    }
}
