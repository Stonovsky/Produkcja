using Autofac;
using System.Windows;
using System;

namespace GAT_Produkcja.Startup
{
    public class ViewModelLocator
    {
        #region AutoWire View with ViewModel using Reflection

        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoWireViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewType = d.GetType();
            var viewName = viewType.FullName;
            var viewModelName = viewName + "Model";
            var viewModelType = Type.GetType(viewModelName);
            var viewModel = IoC.Container.Resolve(viewModelType);
            //var viewModel = Activator.CreateInstance(viewModelType);
            ((FrameworkElement)d).DataContext = viewModel;
        }
        #endregion    
    }
}
