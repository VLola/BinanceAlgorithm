using BinanceAlgorithm.Resourses;
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
        public WindowChart(ListKlines list, List<decimal> list_ema_long, List<decimal> list_ema_short)
        {
            DataContext = new Candlestick(list, list_ema_long, list_ema_short);
            InitializeComponent();
            MouseWheel += WindowChart_MouseWheel;
            MouseMove += WindowChart_MouseMove;
        }


        public class Candlestick
        {
            public List<Candle> Candles { get; } = new List<Candle>();
            public List<Ema> ListEmaLong { get; } = new List<Ema>();
            public List<Ema> ListEmaShort { get; } = new List<Ema>();
            public class Candle
            {
                public double Date { get; set; }                            // расстояние между свечами
                public decimal Low { get; set; }                            // координата низ тени
                public decimal High { get; set; }                           // высота тени свечи
                public decimal Open { get; set; }                           // координата низ свечи
                public decimal Close { get; set; }                          // высота свечи
                public bool IsPositive { get; set; }                        // цвет свечи
            }
            public class Ema
            {
                public decimal X_1 { get; set; }
                public decimal X_2 { get; set; }
                public decimal Y_1 { get; set; }
                public decimal Y_2 { get; set; }
            }
            public Candlestick(ListKlines list, List<decimal> list_ema_long, List<decimal> list_ema_short) {

                try
                {
                    decimal Max = 0m;
                    foreach (var it in list.listKlines)
                    {
                        if (it.High > Max) Max = it.High;
                    }

                    decimal mul = 100m / Max;
                    decimal X = 200m;
                    decimal minus = 97m;

                    int date = 0;
                    int count = 0;
                    foreach (var it in list.listKlines)
                    {

                        Candle candle = new Candle();
                        Ema ema_long = new Ema();
                        Ema ema_short = new Ema();

                        candle.Date = date;

                        decimal high = ((it.High * mul) - minus) * X;
                        decimal low = ((it.Low * mul) - minus) * X;
                        decimal open = ((it.Open * mul) - minus) * X;
                        decimal close = ((it.Close * mul) - minus) * X;
                        if (count < list.listKlines.Count - 1)
                        {
                            ema_long.X_1 = ((list_ema_long[count] * mul) - minus) * X;
                            ema_long.Y_1 = date;
                            ema_long.X_2 = ((list_ema_long[count + 1] * mul) - minus) * X;
                            ema_long.Y_2 = date + 7;

                            ema_short.X_1 = ((list_ema_short[count] * mul) - minus) * X;
                            ema_short.Y_1 = date;
                            ema_short.X_2 = ((list_ema_short[count + 1] * mul) - minus) * X;
                            ema_short.Y_2 = date + 7;
                        }


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

                        if (it.Open <= it.Close)
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
                        date += 7;
                        count++;
                        Candles.Add(candle);
                        ListEmaLong.Add(ema_long);
                        ListEmaShort.Add(ema_short);
                    }
                }
                catch (Exception e)
                {
                    ErrorText.Add(e.Message);
                }
            }
        }

        private void WindowChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = 0;
                if (e.GetPosition(this).X - x_old > 0) x = 10;
                else if (e.GetPosition(this).X - x_old < 0) x = -10;
                if (x_old != 0)
                {
                    double width = Chart1.Width;
                    //if (Chart1.Width + x >= 1300 && Chart1.Width + x <= 3500)
                    Chart1.Width = width + x;
                }

                //double y = 0;
                //if (e.GetPosition(this).Y - y_old > 0) y = -10;
                //else if (e.GetPosition(this).Y - y_old < 0) y = 10;
                //if (y_old != 0)
                //{
                //    double heigth = Chart1.Height;
                //    if (Chart1.Height + y >= 300 && Chart1.Height + y <= 1700) Chart1.Height = heigth + y;
                //    else Chart1.Height = heigth - y;
                //}
            }
            x_old = e.GetPosition(this).X;
            y_old = e.GetPosition(this).Y;
        }

        private void WindowChart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000 > 0) player1Scale.ScaleY = player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000;
        }
    }
}
