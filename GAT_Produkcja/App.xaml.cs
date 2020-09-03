using Autofac;
using GAT_Produkcja.Startup;
using GAT_Produkcja.Utilities.WPFControls.DataGridControl;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GAT_Produkcja
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public App()
        {
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IoC.Setup();

            //var IoC = new ViewModelLocator();
            
            SetupExceptionHandling();
            SetupDataGridSingleClickEditEvent();

            //Show the main window
            Current.MainWindow = StartUpView.GetStartUpWindow();
            Current.MainWindow.Show();
        }


        #region Wyglad-MaterialDesignThemes
        public IEnumerable<ResourceDictionary> GetThemeDictionariesByName(string nameOfResourceDictionary)
        {
            var dictionaries = Resources.MergedDictionaries.Where(d => d.Source.OriginalString.Contains(nameOfResourceDictionary));
            return dictionaries;
        }

        public void ChangeTheme(ResourceDictionary resourceDictionary, Uri uri)
        {
            int index = Resources.MergedDictionaries.IndexOf(resourceDictionary);

            Resources.MergedDictionaries.RemoveAt(index);
            Resources.MergedDictionaries.Insert(index, new ResourceDictionary() { Source = uri });

        }

        #endregion


        private static void SetupDataGridSingleClickEditEvent()
        {
            EventManager.RegisterClassHandler(typeof(DataGrid), DataGrid.PreviewMouseLeftButtonDownEvent,
                        new RoutedEventHandler(DataGridSingleClickHelper.DataGridPreviewMouseLeftButtonDownEvent));
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException += (s, e) =>
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _logger.Error(ex, "Exception in LogUnhandledException");
            }
            finally
            {
                MessageBox.Show(exception.Message);
                _logger.Error(exception, message);
            }
        }



    }
}
