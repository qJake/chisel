using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Chisel
{
    public class ContinuousLineChart : Chart
    {
        public List<DataPoint> Points { get; private set; }

        #region Dependency Properties

        public Brush LineStroke
        {
            get { return (Brush)GetValue(LineStrokeProperty); }
            set { SetValue(LineStrokeProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty LineStrokeProperty =
            DependencyProperty.Register("LineStroke", typeof(Brush), typeof(ContinuousLineChart), new PropertyMetadata(Brushes.Black));

        public Brush PointStroke
        {
            get { return (Brush)GetValue(PointStrokeProperty); }
            set { SetValue(PointStrokeProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty PointStrokeProperty =
            DependencyProperty.Register("PointStroke", typeof(Brush), typeof(ContinuousLineChart), new PropertyMetadata(Brushes.Black));

        public Brush PointFill
        {
            get { return (Brush)GetValue(PointForegroundProperty); }
            set { SetValue(PointForegroundProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty PointForegroundProperty =
            DependencyProperty.Register("PointForeground", typeof(Brush), typeof(ContinuousLineChart), new PropertyMetadata(Brushes.Black));

        public double PointSize
        {
            get { return (double)GetValue(PointSizeProperty); }
            set { SetValue(PointSizeProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty PointSizeProperty =
            DependencyProperty.Register("PointSize", typeof(double), typeof(ContinuousLineChart), new PropertyMetadata(3D));

        public Point? RangeY
        {
            get { return (Point?)GetValue(RangeYProperty); }
            set { SetValue(RangeYProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty RangeYProperty =
            DependencyProperty.Register("RangeY", typeof(Point?), typeof(ContinuousLineChart), new PropertyMetadata(null));

        public Point? RangeX
        {
            get { return (Point?)GetValue(RangeXProperty); }
            set { SetValue(RangeXProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty RangeXProperty =
            DependencyProperty.Register("RangeX", typeof(Point?), typeof(ContinuousLineChart), new PropertyMetadata(null));

        public int HistoryLength
        {
            get { return (int)GetValue(HistoryLengthProperty); }
            set { SetValue(HistoryLengthProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty HistoryLengthProperty =
            DependencyProperty.Register("HistoryLength", typeof(int), typeof(ContinuousLineChart), new PropertyMetadata(100));

        public FlowDirection InsertDirection
        {
            get { return (FlowDirection)GetValue(InsertDirectionProperty); }
            set { SetValue(InsertDirectionProperty, value); }
        }
        public static readonly DependencyProperty InsertDirectionProperty =
            DependencyProperty.Register("InsertDirection", typeof(FlowDirection), typeof(ContinuousLineChart), new PropertyMetadata(FlowDirection.RightToLeft));

        #endregion
        
        private double MinY
        {
            get
            {
                
                return RangeY != null ? RangeY.Value.X : Points.Min(p => p.Y);
            }
        }

        private double MaxY
        {
            get
            {
                return RangeY != null ? RangeY.Value.Y : Points.Min(p => p.Y);
            }
        }

        private double MinX
        {
            get
            {
                return RangeX != null ? RangeX.Value.X : Points.Min(p => p.Y);
            }
        }

        private double MaxX
        {
            get
            {
                return RangeX != null ? RangeX.Value.Y : Points.Min(p => p.Y);
            }
        }        

        public ContinuousLineChart()
            : base()
        {
            Points = new List<DataPoint>();
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

        private void Refresh()
        {
            Children.Clear();

            Canvas c = new Canvas();
            
            DataPoint prevPoint = null;

            int i = 0;

            IEnumerable<DataPoint> pointSet;

            if (InsertDirection == FlowDirection.RightToLeft)
            {
                pointSet = ((IEnumerable<DataPoint>)Points).Reverse().Take(HistoryLength);
            }
            else
            {
                pointSet = ((IEnumerable<DataPoint>)Points).Reverse().Take(HistoryLength).Reverse();
            }

            foreach (var point in pointSet)
            {
                double translatedY = (1D - Normalize(MinY, MaxY, point.Y)) * ActualHeight;
                double translatedX = (InsertDirection == FlowDirection.RightToLeft) ? (1D - Normalize(0, HistoryLength, i)) * ActualWidth
                                                                                    : Normalize(0, HistoryLength, i) * ActualWidth;

                Ellipse dot = new Ellipse();

                Canvas.SetTop(dot, translatedY - (PointSize / 2));
                Canvas.SetLeft(dot, translatedX - (PointSize / 2));

                dot.Height = PointSize;
                dot.Width = PointSize;
                dot.Stroke = PointStroke;
                dot.Fill = PointFill;

                c.Children.Add(dot);

                if (prevPoint != null)
                {
                    double prevTranslatedY = (1D - Normalize(MinY, MaxY, prevPoint.Y)) * ActualHeight;
                    double prevTranslatedX = (InsertDirection == FlowDirection.RightToLeft) ? (1D - Normalize(0, HistoryLength, i - 1)) * ActualWidth
                                                                                            : Normalize(0, HistoryLength, i - 1) * ActualWidth;

                    Line l = new Line();
                    l.X1 = translatedX;
                    l.Y1 = translatedY;
                    l.X2 = prevTranslatedX;
                    l.Y2 = prevTranslatedY;

                    l.Stroke = LineStroke;

                    c.Children.Add(l);
                }

                prevPoint = point;
                i++;
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
