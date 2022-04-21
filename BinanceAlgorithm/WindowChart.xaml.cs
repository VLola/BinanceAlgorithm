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
        public WindowChart(ListKlines list)
        {
            DataContext = new Candlestick(list);
            InitializeComponent();
            MouseWheel += WindowChart_MouseWheel;
        }

        private void WindowChart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(Chart.Width >= 1300 && Chart.Width <= 3500)
            {
                double width = Chart.Width;
                if(width - e.Delta <= 3500 && width - e.Delta >= 1300) Chart.Width = width - e.Delta;
            }
        }

        public class Candlestick
        {
            public decimal mul;
            public List<Candle> Candles { get; } = new List<Candle>();
            public List<double> Labels { get; } = new List<double>();
            public double PriceCurrent { get; }                             // цена монеты сейчас

            public class Candle
            {
                public double Date { get; set; }                            // расстояние между свечами
                public decimal Low { get; set; }                             // координата низ тени
                public decimal High { get; set; }                          // высота тени свечи
                public decimal Open { get; set; }                        // координата низ свечи
                public decimal Close { get; set; }                     // высота свечи
                public bool IsPositive { get; set; }                        // цвет свечи
            }

            public Candlestick(ListKlines list) {
                
                Labels.Add(0);
                Labels.Add(50);
                Labels.Add(100);
                Labels.Add(150);
                Labels.Add(200);
                Labels.Add(250);
                Labels.Add(300);
                Labels.Add(350);
                Labels.Add(400);

                //PriceCurrent = Convert.ToDouble(list.listKlines[0].Close);
                PriceCurrent = 0;
                decimal Max = 0m;
                foreach (var it in list.listKlines)
                {
                    if (it.High > Max) Max = it.High;
                }
                decimal Min = Max;
                foreach (var it in list.listKlines)
                {
                    if (it.Low < Min) Min = it.Low;
                }


                mul = 100m / Max;
                decimal X = 100m;
                decimal minus = 97m;

                int date = 0;
                foreach (var it in list.listKlines)
                {

                    Candle candle = new Candle();

                    candle.Date = date;
                    date += 7;

                    decimal high = ((it.High * mul) - minus) * X;
                    decimal low = ((it.Low * mul)- minus) * X;
                    decimal open = ((it.Open * mul)- minus) * X;
                    decimal close = ((it.Close * mul)- minus) * X;


                    if (it.High < it.Low)
                    {
                        candle.Low = (high);
                        candle.High = (low - high);
                    }
                    else
                    {
                        candle.Low = (low);
                        candle.High = (high - low);
                    }

                    if(it. Open < it.Close)
                    {
                        candle.IsPositive = true;
                        candle.Open = (open);
                        candle.Close = (close - open);
                    }
                    else
                    {
                        candle.IsPositive = false;
                        candle.Open = (close);
                        candle.Close = (open - close);
                    }

                    Candles.Insert(0, candle);
                }

            }

        }


    }
}
