using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.PobieranieBadanZPliku;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny
{
    public class BadaniaGeowlokninViewModel:ViewModelBase
    {
        #region Properties
        private ObservableCollection<tblWynikiBadanGeowloknin> listaBadan;
        private tblWynikiBadanGeowloknin wybraneBadanie;
        private string tytul;
        private readonly IBadaniaGeowlokninRepository repository;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IBadaniaGeowloknin badania;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessenger messenger;

        public RelayCommand OdswiezZestawienieCommand { get; set; }
        public RelayCommand PokazPlikZBadaniemCommand { get; set; }
        public RelayCommand DodajBadanieCommand { get; set; }
        public RelayCommand PokazSzczegolyBadaniaCommand { get; set; }

        public ObservableCollection<tblWynikiBadanGeowloknin> ListaBadan
        {
            get { return listaBadan; }
            set { listaBadan = value; RaisePropertyChanged(); }
        }
        public tblWynikiBadanGeowloknin WybraneBadanie
        {
            get { return wybraneBadanie; }
            set { wybraneBadanie = value; RaisePropertyChanged(); }
        }
        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }
        #endregion

        #region CTOR
        public BadaniaGeowlokninViewModel(  IDialogService dialogService,
                                            IViewService viewService,
                                            IBadaniaGeowloknin badania,
                                            IUnitOfWork unitOfWork,
                                            IMessenger messenger
            )
        {
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.badania = badania;
            this.unitOfWork = unitOfWork;
            this.messenger = messenger;

            OdswiezZestawienieCommand = new RelayCommand(OdswiezZestawienieCommandExecute);
            DodajBadanieCommand = new RelayCommand(DodajBadanieCommandExecute);
            PokazSzczegolyBadaniaCommand = new RelayCommand(PokazSzczegolyBadaniaCommandExecute);
            Tytul = "Wyniki badań geowłóknin";

            messenger.Register<string>(this, "MainMenu", GdyPrzeslanoOdswiezZMainMenu);
            messenger.Register<string>(this, "EwidencjaBadan", GdyDodanoLubUsunietoBadanie);
            
            Task.Run(()=> PobierzBadaniaZBazyAsync() );
        }

        private async void GdyDodanoLubUsunietoBadanie(string obj)
        {
            await PobierzBadaniaZBazyAsync();
        }

        private void PokazSzczegolyBadaniaCommandExecute()
        {
            viewService.Show<ImportBadanZRaportuXlsViewModel>();
            messenger.Send(WybraneBadanie);
        }

        private async void GdyPrzeslanoOdswiezZMainMenu(string obj)
        {
            await PobierzBadaniaZBazyAsync();
        }

        private void DodajBadanieCommandExecute()
        {
            viewService.Show<ImportBadanZRaportuXlsViewModel>();
        }

        #endregion


        private async void OdswiezZestawienieCommandExecute()
        {
            //await badania.DodajBadaniaZPlikowExcel();
            await PobierzBadaniaZBazyAsync();   
        }
        
        private async Task PobierzBadaniaZBazyAsync()
        {
            var lista = await unitOfWork.tblWynikiBadanGeowloknin.GetAllAsync();
            lista = lista.OrderByDescending(d => d.DataBadania);
            ListaBadan = new ObservableCollection<tblWynikiBadanGeowloknin>(lista);
        }

        private void PobierzBadaniaZBazy()
        {
            //var lista = await unitOfWork.tblWynikiBadanGeowloknin.GetAllAsync();
            var lista = unitOfWork.tblWynikiBadanGeowloknin.PobierzBadania();
            lista = lista.OrderByDescending(d => d.DataBadania);
            ListaBadan = new ObservableCollection<tblWynikiBadanGeowloknin>(lista);
        }
    }
}
