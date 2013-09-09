using System.Windows;
using System.Windows.Controls;

namespace Chisel
{
    public abstract class Chart : Canvas
    {
        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }
    }
}
