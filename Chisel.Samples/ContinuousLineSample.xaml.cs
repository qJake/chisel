using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Chisel.Samples
{
    /// <summary>
    /// Shows a continuous line graph sample.
    /// </summary>
    public partial class ContinuousLineSample : Window
    {
        public ContinuousLineSample()
        {
            InitializeComponent();

            Loaded += (_, __) =>
            {
                Task a = Task.Run(() =>
                {
                    Random r = new Random();

                    while (true)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            RandomChart.DataSeries[0].Points.Add(new Point() { Y = r.Next(0, 100) });
                            RandomChart.DataSeries[1].Points.Add(new Point() { Y = r.Next(0, 100) });
                        });

                        Thread.Sleep(200);
                    }
                });

                Task.Run(() =>
                {
                    int coreCount = Environment.ProcessorCount;
                    PerformanceCounter[] counters = new PerformanceCounter[coreCount];

                    Random r = new Random();

                    for (int i = 0; i < (coreCount - 1); i++)
                    {
                        Dispatcher.Invoke(() => CPUChart.DataSeries.Add(new Series() { LineThickness = 1, LineStroke = new SolidColorBrush(new Color() { A = (byte)255, R = (byte)r.Next(0, 215), G = (byte)r.Next(0, 215), B = (byte)r.Next(0, 215) }) }));
                        counters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                    }

                    while (true)
                    {
                        for (int i = 0; i < (coreCount - 1); i++)
                        {
                            Dispatcher.Invoke(() => { CPUChart.DataSeries[i].Points.Add(new Point() { Y = counters[i].NextValue() }); });
                        }
                        Dispatcher.Invoke(() => CPUChart.Refresh());
                        Thread.Sleep(1000);
                    }
                });
            };
        }
    }
}
