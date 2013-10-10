using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Markup;

namespace Chisel
{
    public class ContinuousLineChart : Chart
    {
        #region Dependency Properties

        public Point? RangeY
        {
            get { return (Point?)GetValue(RangeYProperty); }
            set { SetValue(RangeYProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty RangeYProperty =
            DependencyProperty.Register("RangeY", typeof(Point?), typeof(ContinuousLineChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public int HistoryLength
        {
            get { return (int)GetValue(HistoryLengthProperty); }
            set { SetValue(HistoryLengthProperty, value); Refresh(); }
        }
        public static readonly DependencyProperty HistoryLengthProperty =
            DependencyProperty.Register("HistoryLength", typeof(int), typeof(ContinuousLineChart), new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.AffectsArrange));

        public FlowDirection InsertDirection
        {
            get { return (FlowDirection)GetValue(InsertDirectionProperty); }
            set { SetValue(InsertDirectionProperty, value); }
        }
        public static readonly DependencyProperty InsertDirectionProperty =
            DependencyProperty.Register("InsertDirection", typeof(FlowDirection), typeof(ContinuousLineChart), new FrameworkPropertyMetadata(FlowDirection.RightToLeft, FrameworkPropertyMetadataOptions.AffectsArrange));

        #endregion

        private double MinY
        {
            get
            {
                return RangeY != null ? RangeY.Value.X : DataSeries.Min(d => d.Points.Min(p => p.Y));
            }
        }

        private double MaxY
        {
            get
            {
                return RangeY != null ? RangeY.Value.Y : DataSeries.Max(d => d.Points.Max(p => p.Y));
            }
        }

        public override void Refresh()
        {
            Children.Clear();

            foreach (var series in DataSeries)
            {
                Point? prevPoint = null;

                int i = 0;

                IEnumerable<Point> pointSet;

                if (InsertDirection == FlowDirection.RightToLeft)
                {
                    pointSet = ((IEnumerable<Point>)series.Points).Reverse().Take(HistoryLength);
                }
                else
                {
                    pointSet = ((IEnumerable<Point>)series.Points).Reverse().Take(HistoryLength).Reverse();
                }

                foreach (var point in pointSet)
                {
                    // Prepare some local variables for efficiency
                    var lLineStroke = NullUtilities.Coalesce(series.LineStroke, LineStroke);
                    var lLineThickness = NullUtilities.Coalesce(series.LineThickness, LineThickness);
                    var lPointSize = NullUtilities.Coalesce(series.PointSize, PointSize);
                    var lPointStroke = NullUtilities.Coalesce(series.PointStroke, PointStroke);
                    var lPointStrokeThickness = NullUtilities.Coalesce(series.PointStrokeThickness, PointStrokeThickness);
                    var lPointFill = NullUtilities.Coalesce(series.PointFill, PointFill);

                    double translatedY = (1D - Normalize(MinY, MaxY, point.Y)) * ActualHeight;
                    double translatedX = (InsertDirection == FlowDirection.RightToLeft) ? (1D - Normalize(0, HistoryLength, i)) * ActualWidth
                                                                                        : Normalize(0, HistoryLength, i) * ActualWidth;

                    // Draw Line
                    if (prevPoint != null)
                    {
                        double prevTranslatedY = (1D - Normalize(MinY, MaxY, prevPoint.Value.Y)) * ActualHeight;
                        double prevTranslatedX = (InsertDirection == FlowDirection.RightToLeft) ? (1D - Normalize(0, HistoryLength, i - 1)) * ActualWidth
                                                                                                : Normalize(0, HistoryLength, i - 1) * ActualWidth;

                        Line l = new Line();
                        l.X1 = translatedX;
                        l.Y1 = translatedY;
                        l.X2 = prevTranslatedX;
                        l.Y2 = prevTranslatedY;

                        l.Stroke = lLineStroke;
                        l.StrokeThickness = lLineThickness;

                        SetZIndex(l, 100);

                        Children.Add(l);
                    }

                    // Draw Point
                    FrameworkElement dot = null;

                    switch (NullUtilities.Coalesce(series.PointStyle, PointStyle))
                    {
                        case PointStyle.Circle:
                            dot = new Ellipse()
                            {
                                Height = lPointSize,
                                Width = lPointSize,
                                Stroke = lPointStroke,
                                StrokeThickness = lPointStrokeThickness,
                                Fill = lPointFill
                            };
                            break;

                        case PointStyle.Square:
                            dot = new Rectangle()
                            {
                                Height = lPointSize,
                                Width = lPointSize,
                                Stroke = lPointStroke,
                                StrokeThickness = lPointStrokeThickness,
                                Fill = lPointFill
                            };
                            break;
                    }

                    if (dot != null)
                    {
                        Canvas.SetTop(dot, translatedY - (lPointSize / 2));
                        Canvas.SetLeft(dot, translatedX - (lPointSize / 2));

                        SetZIndex(dot, 200);
                        Children.Add(dot);
                    }

                    prevPoint = point;
                    i++;
                }
            }
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
