namespace GAT_Produkcja.UI.ResourceDictionaries.DataGridExtension
{
    using System;
    using System.Windows;

    using DataGridExtensions;
    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Interaction logic for FilterWithPopupControl.xaml
    /// </summary>
    public partial class FilterWithPopupControl
    {
        public RelayCommand WyczyscFiltrCommand { get; set; }
        public FilterWithPopupControl()
        {
            WyczyscFiltrCommand = new RelayCommand(WyczyscFiltrCommandExecute);

            InitializeComponent();
        }

        private void WyczyscFiltrCommandExecute()
        {
            Minimum = null;
            Maximum = null;
            Range_Changed();
        }

        public double? Minimum
        {
            get { return (double?)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double?), typeof(FilterWithPopupControl)
                , new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterWithPopupControl)sender).Range_Changed()));

        public double? Maximum
        {
            get { return (double?)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        /// <summary>
        /// Identifies the Maximum dependency property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double?), typeof(FilterWithPopupControl)
                , new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterWithPopupControl)sender).Range_Changed()));


        private void Range_Changed()
        {
            //if (Maximum<Minimum)
            //    Filter = new ContentFilter(Minimum, double?.MaxValue);

            //Filter = new ContentFilter(Minimum, Maximum);

            Filter = Maximum > Minimum ? new ContentFilter(Minimum, Maximum) : null;
        }

        public IContentFilter Filter
        {
            get { return (IContentFilter)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        /// <summary>
        /// Identifies the Filter dependency property
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(IContentFilter), typeof(FilterWithPopupControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterWithPopupControl)sender).Filter_Changed()));


        private void Filter_Changed()
        {
            var filter = Filter as ContentFilter;
            if (filter == null)
                return;

            Minimum = filter.Min;
            Maximum = filter.Max;
        }

        class ContentFilter : IContentFilter
        {
            private readonly double? _min;
            private readonly double? _max;

            public ContentFilter(double? min, double? max)
            {
                _min = min;
                _max = max;
            }

            public double? Min
            {
                get
                {
                    return _min;
                }
            }

            public double? Max
            {
                get
                {
                    return _max;
                }
            }

            public bool IsMatch(object value)
            {
                if (value == null)
                    return false;

                double number;

                if (!double.TryParse(value.ToString(), out number))
                {
                    return false;
                }

                return (number >= _min) && (number <= _max);
            }
        }

    }
}
