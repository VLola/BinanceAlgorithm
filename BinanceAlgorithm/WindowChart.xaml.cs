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
        public double x_old;
        public double y_old;
        public double scale;
        public WindowChart(ListKlines list)
        {
            DataContext = new Candlestick(list);
            InitializeComponent();
            MouseWheel += WindowChart_MouseWheel;
            MouseMove += WindowChart_MouseMove;

        }

        private void WindowChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = 0;
                double y = 0;
                if (e.GetPosition(this).X - x_old > 0) x = -10;
                else if (e.GetPosition(this).X - x_old < 0) x = 10;
                if (e.GetPosition(this).Y - y_old > 0) y = -10;
                else if (e.GetPosition(this).Y - y_old < 0) y = 10;

                if (x_old != 0)
                {
                    double width = Chart1.Width;
                    if (Chart1.Width + x >= 1300 && Chart1.Width + x <= 3500) Chart1.Width = width + x;
                }

                if (y_old != 0)
                {
                    //double heigth = Chart1.Height;
                    //Chart1.Height = heigth + y;
                    double heigth = Chart1.Height;
                    if (Chart1.Height + y >= 300 && Chart1.Height + y <= 1700) Chart1.Height = heigth + y;
                    else Chart1.Height = heigth - y;
                }
            }
            x_old = e.GetPosition(this).X;
            y_old = e.GetPosition(this).Y;
        }

        private void WindowChart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000 > 0) player1Scale.ScaleY = player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000;
        }

        public class Candlestick
        {
            public List<Candle> Candles { get; } = new List<Candle>();

            public class Candle
            {
                public double Date { get; set; }                            // расстояние между свечами
                public decimal Low { get; set; }                            // координата низ тени
                public decimal High { get; set; }                           // высота тени свечи
                public decimal Open { get; set; }                           // координата низ свечи
                public decimal Close { get; set; }                          // высота свечи
                public bool IsPositive { get; set; }                        // цвет свечи
            }

            public Candlestick(ListKlines list) {

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


                decimal mul = 100m / Max;
                decimal X = 200m;
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
