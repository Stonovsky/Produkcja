using DocumentFormat.OpenXml.Math;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Services.CustomPinMessageBox
{
    public class CustomPinMessageBoxViewModel : ViewModelServiceBase
    {
        private int? pin1;
        #region Properties
        public int? Pin1
        {
            get => pin1; set
            {
                pin1 = value;
                CheckPin();
            }
        }


        public int? Pin2 { get; set; }
        public int? Pin3 { get; set; }
        public int? Pin4 { get; set; }

        #endregion


        #region Commands
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }

        private tblPracownikGAT operatorGniazda;
        #endregion

        #region CTOR

        public CustomPinMessageBoxViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {
            CancelCommand = new RelayCommand(CancelCommandExecute);
            LoadCommand = new RelayCommand(LoadCommandExecute);

            operatorGniazda = UzytkownikZalogowany.Uzytkownik;
        }

        private void LoadCommandExecute()
        {
        }
        #endregion

        private void CancelCommandExecute()
        {
        }
        private void CheckPin()
        {
            if (Pin1 is null || Pin2 is null || Pin3 is null || Pin4 is null) return;

        }

        public bool BoolResult()
        {
            throw new NotImplementedException();
        }


    }
}
