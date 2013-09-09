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

            Task.Run(() =>
            {
                PerformanceCounter cpuCounter = new PerformanceCounter()
                {
                    CategoryName = "Processor",
                    CounterName = "% Processor Time",
                    InstanceName = "_Total"
                };

                while (true)
                {
                    Dispatcher.Invoke(() => Chart.Add(new DataPoint() { Y = cpuCounter.NextValue() }));

                    Thread.Sleep(1000);
                }
            });
        }
    }
}
