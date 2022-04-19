using System;

namespace BinanceAlgorithm
{
    public class Kline
    {
        public DateTime OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public DateTime CloseTime { get; set; }
        public Kline(DateTime OpenTime, decimal Open, decimal High, decimal Low, decimal Close, DateTime CloseTime)
        {
            this.OpenTime = OpenTime;
            this.Open = Open;
            this.High = High;
            this.Low = Low;
            this.Close = Close;
            this.CloseTime = CloseTime;
        }
    }
}
