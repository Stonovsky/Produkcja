using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;

namespace GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja
{
    public class EwidencjaKontrahentowViewModel_old:ViewModelBase
    {  

        #region Properties

        private string tytul;
        private IEnumerable<tblKontrahent> listaKontrahentow;
        private tblKontrahent wybranyKontrahent;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessenger messenger;

        public RelayCommand PokazSzczegolyKontrahentaCommand { get; set; }
        public RelayCommand DodajKontrahentaCommand { get; set; }
        public RelayCommand SzukajCommand { get; set; }


        public IEnumerable<tblKontrahent> ListaKontrahentow
        {
            get { return listaKontrahentow; }
            set { listaKontrahentow = value; RaisePropertyChanged();}
        }

        public tblKontrahent WybranyKontrahent
        {
            get { return wybranyKontrahent; }
            set { wybranyKontrahent = value; RaisePropertyChanged(); }
        }

        private tblKontrahent kontrahentSzukaj;

        public tblKontrahent KontrahentSzukaj
        {
            get { return kontrahentSzukaj; }
            set { kontrahentSzukaj = value; RaisePropertyChanged(); }
        }


        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        #endregion

        #region CTOR
        public EwidencjaKontrahentowViewModel_old(IDialogService dialogService, IViewService viewService, IUnitOfWork unitOfWork, IMessenger messenger)
        {
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.unitOfWork = unitOfWork;
            this.messenger = messenger;

            PokazSzczegolyKontrahentaCommand = new RelayCommand(PokazSzczegolyKontrahentaCommandExecute);
            DodajKontrahentaCommand = new RelayCommand(DodajKontrahentaCommandExecute);
            SzukajCommand = new RelayCommand(SzukajCommandExecute);

            tytul = "Pobieram dane. Proszę czekać.";
            KontrahentSzukaj = new tblKontrahent();

            Task.Run(()=> PobierzWartosciPoczatkowe());

            messenger.Register<string>(this, OdswiezEwidencje);
        }
        #endregion

        #region Commands

        private async void SzukajCommandExecute()
        {
            ListaKontrahentow = await unitOfWork.tblKontrahent.WyszukajKontrahenta(KontrahentSzukaj);
        }

        private void DodajKontrahentaCommandExecute()
        {
            viewService.Show<DodajKontrahentaViewModel_old>();
        }

        private void PokazSzczegolyKontrahentaCommandExecute()
        {
            if (WybranyKontrahent != null)
            {
                viewService.Show<DodajKontrahentaViewModel_old>();
                messenger.Send(WybranyKontrahent);
            }
        }

        #endregion

        private void OdswiezEwidencje(string obj)
        {
            if (obj == "Odswiez")
            {
                Task.Run(() => PobierzWartosciPoczatkowe());
            }
        }

        private async Task PobierzWartosciPoczatkowe()
        {
            ListaKontrahentow= await unitOfWork.tblKontrahent.GetAllAsync();
            Tytul = "Ewidencja kontrahentów";
        }



    }
}
