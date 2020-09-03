using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Utilities.ExcelReportGenerator;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Models;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.Factory;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja
{
    public class ProdukcjaEwidencjaViewModel : ListCommandViewModelBase, IProdukcjaEwidencjaViewModel
    {
        #region Fields
        private readonly IProdukcjaEwidencjaSQLStateFactory stateFactory;
        private int zaznaczonyTabItem;
        
        #endregion

        #region Properties

        public ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiKonfekcji { get => State.ListaPozycjiKonfekcji; set => State.ListaPozycjiKonfekcji = value; }
        public ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiKalandra { get => State.ListaPozycjiKalandra; set => State.ListaPozycjiKalandra = value; }
        public ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiWloknin { get => State.ListaPozycjiWloknin; set => State.ListaPozycjiWloknin = value; }

        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKonfekcji { get => State.ListaZgrupowanychPozycjiKonfekcji; set => State.ListaZgrupowanychPozycjiKonfekcji = value; }
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKalandra { get => State.ListaZgrupowanychPozycjiKalandra; set => State.ListaZgrupowanychPozycjiKalandra = value; }
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiLiniWloknin { get => State.ListaZgrupowanychPozycjiLiniWloknin; set => State.ListaZgrupowanychPozycjiLiniWloknin = value; }

        public DateTime DataPoczatek { get => State.DataPoczatek; set => State.DataPoczatek = value; }
        public DateTime DataKoniec { get => State.DataKoniec; set => State.DataKoniec = value; }
        public int ZaznaczonyTabItem
        {
            get => zaznaczonyTabItem;

            set
            {
                zaznaczonyTabItem = value;
                State = stateFactory.PobierzStan(zaznaczonyTabItem);
            }
        }

        public ProdukcjaEwidencjaPodsumowanieModel Podsumowanie { get => State.Podsumowanie; set => State.Podsumowanie = value; }
        public string TowarNazwaFiltr { get => State.TowarNazwaFiltr; set => State.TowarNazwaFiltr = value; }

        public IProdukcjaEwidencjaSQLState State { get; set; }

        #endregion

        #region Commands
        public RelayCommand SzukajCommand { get; set; }
        #endregion

        #region CTOR

        public ProdukcjaEwidencjaViewModel(IViewModelService viewModelService,
                                           IProdukcjaEwidencjaSQLStateFactory stateFactory)
            : base(viewModelService)
        {
            this.stateFactory = stateFactory;

            State = stateFactory.PobierzStan(0);

            SzukajCommand = new RelayCommand(SzukajCommandExecute);
        }

        #endregion

        private async void SzukajCommandExecute()
        {
            try
            {
                await State.LoadAsync();
                await State.PobierzListeRolek();
                await State.GrupujTowary();
                State.PodsumujListe();
                PobierzWlasciwosciZeState();
            }
            catch (Exception ex)
            {
                DialogService.ShowInfo_BtnOK(ex.Message);
                //Gdy brak rolek wyswietla puste encje (pusta tabele)
                PobierzWlasciwosciZeState();
            }
        }

        #region Grupowanie towarow
        private void PobierzWlasciwosciZeState()
        {
            ListaPozycjiWloknin = State.ListaPozycjiWloknin;
            ListaPozycjiKalandra = State.ListaPozycjiKalandra;
            ListaPozycjiKonfekcji = State.ListaPozycjiKonfekcji;

            ListaZgrupowanychPozycjiLiniWloknin = State.ListaZgrupowanychPozycjiLiniWloknin;
            ListaZgrupowanychPozycjiKalandra = State.ListaZgrupowanychPozycjiKalandra;
            ListaZgrupowanychPozycjiKonfekcji = State.ListaZgrupowanychPozycjiKonfekcji;

            Podsumowanie = State.Podsumowanie;
        }
        #endregion

        protected override async void LoadCommandExecute()
        {
            await State.LoadAsync();
        }


    }
}
