using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbComarch.UnitOfWork;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Dictionaries;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.PW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.RW;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Naglowek
{
    [AddINotifyPropertyChangedInterface]

    public class RozliczenieMsAccessNaglowekViewModel : SaveCommandGenericViewModelBase
    {
        public IRozliczenieMsAccessPWViewModel RozliczenieMsAccessPWViewModel { get; private set; }
        public IRozliczenieMsAccessRWViewModel RozliczenieMsAccessRWViewModel { get; private set; }

        private tblTowar wybranySurowiec;

        public tblProdukcjaRozliczenie_Naglowek DaneWejsciowe { get; set; } = new tblProdukcjaRozliczenie_Naglowek();
        public tblProdukcjaRozliczenie_Naglowek DaneWejscioweOrg { get; set; }

        public IEnumerable<tblTowar> ListaSurowcow { get; private set; }
        public tblTowar WybranySurowiec
        {
            get => wybranySurowiec;
            set { wybranySurowiec = value;
            }
        }


        #region Commands
        public RelayCommand RozliczCommand { get; set; }

        #endregion

        public RozliczenieMsAccessNaglowekViewModel(IViewModelService viewModelService,
                                                    IRozliczenieMsAccessPWViewModel RozliczenieMsAccessPWViewModel,
                                                    IRozliczenieMsAccessRWViewModel RozliczenieMsAccessRWViewModel
                                                    )
            : base(viewModelService)
        {
            this.RozliczenieMsAccessPWViewModel = RozliczenieMsAccessPWViewModel;
            this.RozliczenieMsAccessRWViewModel = RozliczenieMsAccessRWViewModel;

            RozliczCommand = new RelayCommand(RozliczCommandExecute, RozliczCommandCanExecute);

        }

        private void RozliczCommandExecute()
        {
            Messenger.Send(DaneWejsciowe);
        }

        private bool RozliczCommandCanExecute()
        {
            return IsValid;
        }

        public override bool IsChanged => DaneWejsciowe.Compare(DaneWejscioweOrg);

        public override bool IsValid => DaneWejsciowe.IsValid 
                                        && RozliczenieMsAccessPWViewModel.IsValid 
                                        && RozliczenieMsAccessRWViewModel.IsValid;


        public override void IsChanged_False()
        {
            DaneWejscioweOrg = DaneWejsciowe.DeepClone();
        }

        protected override async void LoadCommandExecute()
        {
            ListaSurowcow = await UnitOfWork.tblTowar.WhereAsync(t => t.IDTowarGrupa == 7);
            //listaPozycjiKonfekcji = await unitOfWorkMsAccess.Konfekcja.GetAllAsync();
            //listaPozycjiKalandra = await unitOfWorkMsAccess.Kalander.GetAllAsync();
            //listaPozycjiProdukcji = await unitOfWorkMsAccess.Produkcja.GetAllAsync();
            //listaNormZuzycia = await unitOfWorkMsAccess.NormyZuzycia.GetAllAsync();
            //listaSurowcow = await unitOfWorkMsAccess.Surowiec.GetAllAsync();
            //listaSurowcowZCenamiZComarch = await unitOfWorkComarch.Surowiec.PobierzListeSurowcowZCenamiAsync();
            //listaZlecen = await unitOfWorkMsAccess.Dyspozycje.GetAllAsync();
        }

        protected override void DeleteCommandExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteCommandCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void SaveCommandExecute()
        {
            if (DaneWejsciowe.IDProdukcjaRozliczenie_Naglowek == 0)
            {
                UnitOfWork.tblProdukcjaRozliczenie_Dane.Add(DaneWejsciowe);
            }
        }

        protected override bool SaveCommandCanExecute()
        {
            //TODO GdyBedaWsyzstkieViewModele
            throw new NotImplementedException();
        }
    }
}
