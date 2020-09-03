using Autofac;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.Startup;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.MainMenu;
using GAT_Produkcja.ViewModel.MainMenu.Messages;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Helper;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Message;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.States;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly
{
    public class ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel : ListMethodViewModelBase, IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel
    {
        private readonly IZamOdKlientaFiltrHelper filtr;

        public ObservableCollection<vwZamOdKlientaAGG> ListaZamowienOdKlientow { get; set; }
        public tblPracownikGAT Uzytkownik { get; set; } = UzytkownikZalogowany.Uzytkownik;
        public IZKUCState StanZK{ get; set; }
        public bool IsButtonActive { get; set; } = true;

        public ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel(IViewModelService viewModelService,
                                                                IZamOdKlientaFiltrHelper filtr) 
            : base(viewModelService)
        {
            this.filtr = filtr;
        }

        private async Task PobierzListeZK(ZK_Filtr filtrZK)
        {
            ListaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>(
                await filtr.FiltrujAsync(filtrZK));

            ListaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>( ListaZamowienOdKlientow.OrderByDescending(d => d.DataWyst).ToList());

            Podsumuj();
        }

        public override async Task LoadAsync(int? id)
        {
            IsButtonActive = false;

            await PobierzListeZK(null);

            IsButtonActive = true;
        }

        private void Podsumuj()
        {
            if (ListaZamowienOdKlientow is null) return;

            var podsumowanie = new ZK_Podsumowanie();
            podsumowanie.Ilosc = ListaZamowienOdKlientow.Sum(s => s.Ilosc);
            podsumowanie.Wartosc = ListaZamowienOdKlientow.Sum(s => s.WartNetto);

            Messenger.Send(podsumowanie);
        }

        public void SetState(string name)
        {
            switch (name)
            {
                case nameof(MainMenuViewModel):
                    StanZK = new OdswiezZKUCState(this, IoC.Container.Resolve<IZamOdKlientaFiltrHelper>());
                    break;
                case nameof(ZamowienieOdKlientaEwidencjaViewModel):
                    StanZK = new FiltrujZKUCState(this, IoC.Container.Resolve<IZamOdKlientaFiltrHelper>());
                    break;
                default:
                    break;
            }
        }
    }
}
