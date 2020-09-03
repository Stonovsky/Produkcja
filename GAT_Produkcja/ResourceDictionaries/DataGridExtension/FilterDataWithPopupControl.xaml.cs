namespace GAT_Produkcja.UI.ResourceDictionaries.DataGridExtension
{
    using System;
    using System.Windows;

    using DataGridExtensions;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Interaction logic for FilterWithPopupControl.xaml
    /// </summary>
    public partial class FilterDataWithPopupControl
    {

        public RelayCommand WyczyscFiltrCommand { get; set; }

        public FilterDataWithPopupControl()
        {
            WyczyscFiltrCommand = new RelayCommand(WyczyscFiltrCommandExecute);
            InitializeComponent();
        }

        private void WyczyscFiltrCommandExecute()
        {
            FromDate = null;
            ToDate = null;
            Range_Changed();
        }

        public DateTime? FromDate
        {
            get { return (DateTime?)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }
        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty FromDateProperty = DependencyProperty.Register("FromDate", typeof(DateTime?), typeof(FilterDataWithPopupControl)
                , new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterDataWithPopupControl)sender).Range_Changed()));

        public DateTime? ToDate
        {
            get { return (DateTime?)GetValue(ToDateProperty); }
            set { SetValue(ToDateProperty, value); }
        }
        /// <summary>
        /// Identifies the Maximum dependency property
        /// </summary>
        public static readonly DependencyProperty ToDateProperty = DependencyProperty.Register("ToDate", typeof(DateTime?), typeof(FilterDataWithPopupControl)
                , new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterDataWithPopupControl)sender).Range_Changed()));


        private void Range_Changed()
        {
            Filter = ToDate > FromDate ? new ContentFilter(FromDate, ToDate) : null;
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
            DependencyProperty.Register("Filter", typeof(IContentFilter), typeof(FilterDataWithPopupControl), 
                                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterDataWithPopupControl)sender).Filter_Changed()));


        private void Filter_Changed()
        {
            var filter = Filter as ContentFilter;
            if (filter == null)
                return;

            FromDate = filter.FromDate;
            ToDate = filter.ToDate;
        }

        class ContentFilter : IContentFilter
        {
            private readonly DateTime? _fromDate;
            private readonly DateTime? _toDate;

            public ContentFilter(DateTime? fromDate, DateTime? toDate)
            {
                _fromDate = fromDate;
                _toDate = toDate;
            }

            public DateTime? FromDate
            {
                get
                {
                    return _fromDate;
                }
            }

            public DateTime? ToDate
            {
                get
                {
                    return _toDate;
                }
            }

            public bool IsMatch(object value)
            {
                if (value == null)
                    return false;

                DateTime inputDate;

                if (!DateTime.TryParse(value.ToString(), out inputDate))
                {
                    return false;
                }

                return (inputDate >= _fromDate) && (inputDate <= _toDate.GetValueOrDefault().AddDays(1));
            }
        }

    }
}
