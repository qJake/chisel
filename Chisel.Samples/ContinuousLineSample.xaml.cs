using System;
using System.Collections.Generic;
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

            Chart.SetOptions(new ChartOptions() { UpdateMode = ChartUpdateModes.Continuous });

            Task.Run(() =>
            {
                Random r = new Random();

                while (true)
                {
                    Dispatcher.Invoke(() => Chart.Add(new DataPoint() { Y = r.Next(0, 100) }));

                    Thread.Sleep(50);
                }
            });
        }
    }
}
