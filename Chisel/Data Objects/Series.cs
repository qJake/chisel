using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Chisel
{
    [ContentProperty("Points")]
    public class Series : FrameworkElement
    {
        public event Action<object, NotifyCollectionChangedEventArgs> PointsChanged;

        public ObservableCollection<Point> Points
        {
            get { return (ObservableCollection<Point>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(ObservableCollection<Point>), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnPointsChanged));
        
        private static void OnPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Series).OnPointsChangedInstance((ObservableCollection<Point>)e.OldValue, (ObservableCollection<Point>)e.NewValue);
        }

        private void OnPointsChangedInstance(ObservableCollection<Point> OldData, ObservableCollection<Point> NewData)
        {
            if (OldData != null)
            {
                OldData.CollectionChanged -= OnPointCollectionChanged;
            }
            if (NewData != null)
            {
                NewData.CollectionChanged += OnPointCollectionChanged;
            }
        }

        private void OnPointCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            if (PointsChanged != null)
            {
                PointsChanged(s, e);
            }
        }

        public string SeriesName
        {
            get { return (string)GetValue(SeriesNameProperty); }
            set { SetValue(SeriesNameProperty, value); }
        }
        public static readonly DependencyProperty SeriesNameProperty =
            DependencyProperty.Register("SeriesName", typeof(string), typeof(Series), new PropertyMetadata(""));

        public Brush LineStroke
        {
            get { return (Brush)GetValue(LineStrokeProperty); }
            set { SetValue(LineStrokeProperty, value); }
        }
        public static readonly DependencyProperty LineStrokeProperty =
            DependencyProperty.Register("LineStroke", typeof(Brush), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double? LineThickness
        {
            get { return (double?)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }
        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register("LineThickness", typeof(double?), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public Brush PointStroke
        {
            get { return (Brush)GetValue(PointStrokeProperty); }
            set { SetValue(PointStrokeProperty, value); }
        }
        public static readonly DependencyProperty PointStrokeProperty =
            DependencyProperty.Register("PointStroke", typeof(Brush), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double? PointStrokeThickness
        {
            get { return (double?)GetValue(PointStrokeThicknessProperty); }
            set { SetValue(PointStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty PointStrokeThicknessProperty =
            DependencyProperty.Register("PointStrokeThickness", typeof(double?), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public Brush PointFill
        {
            get { return (Brush)GetValue(PointForegroundProperty); }
            set { SetValue(PointForegroundProperty, value); }
        }
        public static readonly DependencyProperty PointForegroundProperty =
            DependencyProperty.Register("PointForeground", typeof(Brush), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double? PointSize
        {
            get { return (double?)GetValue(PointSizeProperty); }
            set { SetValue(PointSizeProperty, value); }
        }
        public static readonly DependencyProperty PointSizeProperty =
            DependencyProperty.Register("PointSize", typeof(double?), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public PointStyle? PointStyle
        {
            get { return (PointStyle?)GetValue(PointStyleProperty); }
            set { SetValue(PointStyleProperty, value); }
        }
        public static readonly DependencyProperty PointStyleProperty =
            DependencyProperty.Register("PointStyle", typeof(PointStyle?), typeof(Series), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

        public Series()
        {
            Points = new ObservableCollection<Point>();
        }
    }
}
