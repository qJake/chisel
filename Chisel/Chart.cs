using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Chisel
{
    [ContentProperty("DataSeries")]
    public abstract class Chart : Canvas
    {
        public ObservableCollection<Series> DataSeries
        {
            get { return (ObservableCollection<Series>)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }
        public static readonly DependencyProperty DataSeriesProperty =
            DependencyProperty.Register("DataSeries", typeof(ObservableCollection<Series>), typeof(Chart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnDataSeriesChanged));

        private static void OnDataSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Chart).OnDataSeriesChangedInstance((ObservableCollection<Series>)e.OldValue, (ObservableCollection<Series>)e.NewValue);
        }

        private void OnDataSeriesChangedInstance(ObservableCollection<Series> OldData, ObservableCollection<Series> NewData)
        {
            if (OldData != null)
            {
                OldData.CollectionChanged -= OnSeriesCollectionChanged;
            }
            if (NewData != null)
            {
                NewData.CollectionChanged += OnSeriesCollectionChanged;
            }
        }

        private void OnSeriesCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Series series in e.OldItems)
                {
                    series.PointsChanged -= OnSeriesPointsChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (Series series in e.NewItems)
                {
                    series.PointsChanged += OnSeriesPointsChanged;
                }
            }
            OnPropertyChanged(new DependencyPropertyChangedEventArgs(DataSeriesProperty, null, null));
        }

        private void OnSeriesPointsChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(new DependencyPropertyChangedEventArgs(DataSeriesProperty, null, null));
        }

        public Brush LineStroke
        {
            get { return (Brush)GetValue(LineStrokeProperty); }
            set { SetValue(LineStrokeProperty, value); }
        }
        public static readonly DependencyProperty LineStrokeProperty =
            DependencyProperty.Register("LineStroke", typeof(Brush), typeof(Chart), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }
        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register("LineThickness", typeof(double), typeof(Chart), new FrameworkPropertyMetadata(1D, FrameworkPropertyMetadataOptions.AffectsArrange));

        public Brush PointStroke
        {
            get { return (Brush)GetValue(PointStrokeProperty); }
            set { SetValue(PointStrokeProperty, value); }
        }
        public static readonly DependencyProperty PointStrokeProperty =
            DependencyProperty.Register("PointStroke", typeof(Brush), typeof(Chart), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double PointStrokeThickness
        {
            get { return (double)GetValue(PointStrokeThicknessProperty); }
            set { SetValue(PointStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty PointStrokeThicknessProperty =
            DependencyProperty.Register("PointStrokeThickness", typeof(double), typeof(Chart), new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsArrange));

        public Brush PointFill
        {
            get { return (Brush)GetValue(PointForegroundProperty); }
            set { SetValue(PointForegroundProperty, value); }
        }
        public static readonly DependencyProperty PointForegroundProperty =
            DependencyProperty.Register("PointForeground", typeof(Brush), typeof(Chart), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double PointSize
        {
            get { return (double)GetValue(PointSizeProperty); }
            set { SetValue(PointSizeProperty, value); }
        }
        public static readonly DependencyProperty PointSizeProperty =
            DependencyProperty.Register("PointSize", typeof(double), typeof(Chart), new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsArrange));

        public PointStyle PointStyle
        {
            get { return (PointStyle)GetValue(PointStyleProperty); }
            set { SetValue(PointStyleProperty, value); }
        }
        public static readonly DependencyProperty PointStyleProperty =
            DependencyProperty.Register("PointStyle", typeof(PointStyle), typeof(Chart), new FrameworkPropertyMetadata(PointStyle.None, FrameworkPropertyMetadataOptions.AffectsArrange));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Equals(DataSeriesProperty))
            {
                Refresh();
            }
            // TODO: Implement different property changes, and refresh only what changed (i.e. line color, point style, etc) instead of inefficiently re-rendering it all.
            base.OnPropertyChanged(e);
        }

        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }

        public Chart()
        {
            DataSeries = new ObservableCollection<Series>();
        }

        public abstract void Refresh();
    }
}
