using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Math;

namespace BinanceAlgorithm
{
    /// <summary>
    /// Логика взаимодействия для WindowChart.xaml
    /// </summary>
    public partial class WindowChart
    {
        public WindowChart()
        {
            DataContext = new Candlestick();
            InitializeComponent();
        }

        public class Candlestick
        {
            private const int PriceCount = 500;
            private const int PricesPerCandle = 10;

            public List<Price> Prices { get; } = new List<Price>(PriceCount + 1);
            public List<Candle> Candles { get; } = new List<Candle>(PriceCount / PricesPerCandle);
            public List<double> Labels { get; } = new List<double>();
            public double PriceCurrent { get; }
            public double PriceMin { get; }
            public double PriceMax { get; }
            public double PriceHeight { get; }

            public Candlestick() {
                Prices.Add(new Price() { Date = DateTime.Now, Value = 100 });
                Candles.Add(new Candle() { 
                    Date = 0               // расстояние между свечами
                    , Min = 290             // координата низ тени
                    , Max = 1               // 
                    , DeltaMax = 1          // 
                    , DeltaMin = 300        // координата низ свечи
                    , DeltaHeight = 10      // высота свечи
                    , Height = 70           // высота тени свечи
                    , IsPositive = true    // цвет свечи
                });
                Candles.Add(new Candle()
                {
                    Date = 10             // расстояние между свечами
                    ,
                    Min = 290             // координата низ тени
                    ,
                    Max = 1               // 
                    ,
                    DeltaMax = 1          //
                    ,
                    DeltaMin = 300        // координата низ свечи
                    ,
                    DeltaHeight = 10      // высота свечи
                    ,
                    Height = 70           // высота тени свечи
                    ,
                    IsPositive = true     // цвет свечи
                });
                Labels.Add(200);
                Labels.Add(250);
                Labels.Add(300);
                Labels.Add(350);
                Labels.Add(400);
                PriceCurrent = 310;         // цена монеты сейчас
                PriceMin = 0;
                PriceMax = 0;
                PriceHeight = 300+110;          // координаты центра графика +110
            }
            //public Candlestick()
            //{
            //    var rnd = new Random(2);
            //    var today = DateTime.Today;
            //    var date = DateTime.Today;
            //    var value = 300;
            //    for (var i = 0; i < Prices.Capacity; i++)
            //        Prices.Add(new Price { Date = date = date.AddMinutes(5), Value = value += rnd.Next(-9, 10) });
            //    for (var i = 0; i < Candles.Capacity; i++)
            //    {
            //        var prices = Prices.Select(p => p.Value).Skip(i * PricesPerCandle).Take(PricesPerCandle + 1);
            //        Candles.Add(new Candle
            //        {
            //            Date = (Prices[i * PricesPerCandle].Date - today).TotalMinutes / 5,
            //            Min = prices.Min(),
            //            Max = prices.Max(),
            //            Height = prices.Max() - prices.Min(),
            //            DeltaMin = prices.First(),
            //            DeltaMax = prices.Last(),
            //            DeltaHeight = Abs(prices.Last() - prices.First()),
            //            IsPositive = prices.First() < prices.Last(),
            //        });
            //    }
            //    Candles.ForEach(c => c.Fix());
            //    PriceCurrent = Prices.Last().Value;
            //    PriceMin = Prices.Min(p => p.Value) - 20;
            //    PriceMax = Prices.Max(p => p.Value) + 20;
            //    PriceHeight = PriceMax - PriceMin - 40;
            //    for (double price = Round(PriceMin / 10) * 10; price < PriceMax; price += 50)
            //        Labels.Add(price);
            //}
        }

        public class Price
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }

        public class Candle
        {
            public double Date { get; set; }
            public double Min { get; set; }
            public double Max { get; set; }
            public double Height { get; set; }
            public double DeltaMin { get; set; }
            public double DeltaMax { get; set; }
            public double DeltaHeight { get; set; }
            public bool IsPositive { get; set; }

            //public void Fix()
            //{
            //    if (!IsPositive)
            //    {
            //        var min = DeltaMin;
            //        DeltaMin = DeltaMax;
            //        DeltaMax = min;
            //    }
            //}
        }
    }
}
