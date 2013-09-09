using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Chisel
{
    public class LineChart : Chart
    {
        public List<DataPoint> Points { get; private set; }

        public ChartOptions Options { get; private set; }

        private double MinY
        {
            get
            {
                return Points.Min(p => p.Y);
            }
        }

        private double MaxY
        {
            get
            {
                return Points.Max(p => p.Y);
            }
        }

        private double MinX
        {
            get
            {
                return Points.Min(p => p.X);
            }
        }

        private double MaxX
        {
            get
            {
                return Points.Max(p => p.X);
            }
        }        

        private int _HistoryLength;
        public int HistoryLength
        {
            get
            {
                return _HistoryLength;
            }
            set
            {
                if (_HistoryLength != value)
                {
                    _HistoryLength = value;
                    Refresh();
                }
            }
        }

        public LineChart()
            : base()
        {
            // Sensible defaults
            Points = new List<DataPoint>();
            Options = new ChartOptions();
            HistoryLength = 100;

            // Debug
            Background = Brushes.LightGray;

            Refresh();
        }

        public void Add(DataPoint point)
        {
            Points.Add(point);
            if (Points.Count > HistoryLength)
            {
                Points = Points.GetRange(Points.Count - HistoryLength, HistoryLength);
            }
            Refresh();
        }

        public void SetOptions(ChartOptions newOptions)
        {
            Options = newOptions;
            Refresh();
        }

        private void Refresh()
        {
            Children.Clear();

            Canvas c = new Canvas();
            
            DataPoint prevPoint = null;

            int i = 0;

            int dotSize = 3; // TODO: Temp, remove this

            if (Options.UpdateMode == ChartUpdateModes.Continuous)
            {
                // TODO: If Linq is too slow, switch to a for() with indices
                foreach (var point in ((IEnumerable<DataPoint>)Points).Reverse().Take(HistoryLength))
                {
                    if (Options.UpdateMode == ChartUpdateModes.Continuous)
                    {
                        double translatedY = (1D - Normalize(MinY, MaxY, point.Y)) * ActualHeight;
                        double translatedX = (1D - Normalize(0, HistoryLength, i)) * ActualWidth;

                        if (Options.PointStyle != DataPointStyles.None)
                        {
                            Ellipse dot = new Ellipse();

                            // TODO: Take into account chart border margins
                            Canvas.SetTop(dot, translatedY - (dotSize / 2));
                            Canvas.SetLeft(dot, translatedX - (dotSize / 2));

                            // TODO: Temp, use actual dot sizes
                            dot.Height = dotSize;
                            dot.Width = dotSize;

                            dot.Fill = Brushes.Blue;

                            c.Children.Add(dot);
                        }

                        if (Options.LineStyle != DataLineStyles.None && prevPoint != null)
                        {
                            double prevTranslatedY = (1D - Normalize(MinY, MaxY, prevPoint.Y)) * ActualHeight;
                            double prevTranslatedX = (1D - Normalize(0, HistoryLength, i - 1)) * ActualWidth;

                            Line l = new Line();
                            l.X1 = translatedX;
                            l.Y1 = translatedY;
                            l.X2 = prevTranslatedX;
                            l.Y2 = prevTranslatedY;

                            // TODO: Temp, use actual thickness
                            l.Stroke = Brushes.Black;

                            c.Children.Add(l);
                        }

                        prevPoint = point;
                        i++;
                    }
                }
            }
            else
            {
                // TODO: This
            }

            Children.Add(c);

            InvalidateArrange();
        }

        /// <summary>
        /// Normalizes a number based on a min and max value.
        /// </summary>
        /// <returns>The normalized form of <paramref name="value" />, from 0..1.</returns>
        private double Normalize(double min, double max, double value)
        {
            if (value < min || value > max)
            {
                throw new ArgumentException("Value must be between min and max.", "value");
            }
            if (min == max)
            {
                return 0.5; // Center if min and max are the same
            }
            if (!(max > min))
            {
                throw new ArgumentException("Max value must be greater than min value", "max");
            }

            // Optimizations
            if (value == min) return 0;
            if (value == max) return 1;

            return Math.Abs((value - min) / (max - min));
        }
    }
}
