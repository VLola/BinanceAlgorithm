using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BinanceAlgorithm.Resourses.History;

namespace BinanceAlgorithm.Resourses
{
    public class Candlestick
    {
        public List<Candle> Candles { get; set; } = new List<Candle>();
        public List<Ema> ListEmaLong { get; set; } = new List<Ema>();
        public List<Ema> ListEmaShort { get; set; } = new List<Ema>();
        public List<double> Labels { get; set; } = new List<double>();
        public List<MovementHistory> movement_history { get; set; } = new List<MovementHistory>();
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
        public Candlestick(List<MovementHistory> history, ListKlines list, List<decimal> list_ema_long, List<decimal> list_ema_short)
        {

            try
            {
                movement_history = history;

                for (int i = -5000; i<5000; i += 100)
                {
                    Labels.Add(i);
                }
                decimal Max = 0m;
                foreach (var it in list.listKlines)
                {
                    if (it.High > Max) Max = it.High;
                }


                decimal mul = 100m / Max;
                decimal X = 200m;
                decimal minus = 97m;

                for (int i = 0; i < movement_history.Count; i++)
                {
                    movement_history[i].Y1 = ((movement_history[i].Y1 * mul) - minus) * X;
                    movement_history[i].X1 = movement_history[i].X1 * 7;
                    movement_history[i].Y2 = ((movement_history[i].Y2 * mul) - minus) * X;
                    movement_history[i].X2 = movement_history[i].X2 * 7;
                }

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
                        ema_long.Y_1 = ((list_ema_long[count] * mul) - minus) * X;
                        ema_long.X_1 = date;
                        ema_long.Y_2 = ((list_ema_long[count + 1] * mul) - minus) * X;
                        ema_long.X_2 = date + 7;

                        ema_short.Y_1 = ((list_ema_short[count] * mul) - minus) * X;
                        ema_short.X_1 = date;
                        ema_short.Y_2 = ((list_ema_short[count + 1] * mul) - minus) * X;
                        ema_short.X_2 = date + 7;
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
}
