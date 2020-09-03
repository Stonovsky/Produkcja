using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace GAT_Produkcja.ViewModel._Test
{
    public class TestowyViewModel : ViewModelServiceBase
    {
        public tblProdukcjaRuchTowar Towar { get; set; } = new tblProdukcjaRuchTowar();

        #region Commands
        public RelayCommand GdyZmianaCommand { get; set; }
        #endregion
        public TestowyViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
            GdyZmianaCommand = new RelayCommand(GdyZmianaCommandExecute);


        }

        private void GdyZmianaCommandExecute()
        {
            //await Task.Delay(1000);
            SomeMethod();

        }

        private void SomeMethod()
        {
        }
    }
}
