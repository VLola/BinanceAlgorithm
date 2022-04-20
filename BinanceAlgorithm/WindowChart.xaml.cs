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

            public List<Candle> Candles { get; } = new List<Candle>();
            public List<double> Labels { get; } = new List<double>();
            public double PriceCurrent { get; }                             // цена монеты сейчас
            public double PriceHeight { get; }                              // координаты центра графика +110

            public class Candle
            {
                public double Date { get; set; }                            // расстояние между свечами
                public double Min { get; set; }                             // координата низ тени
                public double Height { get; set; }                          // высота тени свечи
                public double DeltaMin { get; set; }                        // координата низ свечи
                public double DeltaHeight { get; set; }                     // высота свечи
                public bool IsPositive { get; set; }                        // цвет свечи
            }

            public Candlestick() {
                
                Labels.Add(0);
                Labels.Add(50);
                Labels.Add(100);
                Labels.Add(150);
                Labels.Add(200);
                Labels.Add(250);
                Labels.Add(300);
                Labels.Add(350);
            }
            
        }


    }
}
