using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Helper;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.States
{
    public class FiltrujZKUCState : IZKUCState
    {
        private ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel ewidencjaSzczegolyUCViewModel;
        private readonly IZamOdKlientaFiltrHelper filtr;

        public FiltrujZKUCState(ZamowienieOdKlientaEwidencjaSzczegolyUCViewModel ewidencjaSzczegolyUCViewModel,
                                IZamOdKlientaFiltrHelper filtr)
        {
            this.ewidencjaSzczegolyUCViewModel = ewidencjaSzczegolyUCViewModel;
            this.filtr = filtr;

            ewidencjaSzczegolyUCViewModel.Messenger.Register<ZK_Filtr>(this, GdyPrzeslanoFiltr);

        }

        private async void GdyPrzeslanoFiltr(ZK_Filtr obj)
        {
            await PobierzListeZK(obj);
        }

        private async Task PobierzListeZK(ZK_Filtr filtrZK)
        {
            ewidencjaSzczegolyUCViewModel.ListaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                (await filtr.FiltrujAsync(filtrZK));
            ewidencjaSzczegolyUCViewModel.ListaZamowienOdKlientow = new ObservableCollection<vwZamOdKlientaAGG>
                (ewidencjaSzczegolyUCViewModel.ListaZamowienOdKlientow.OrderByDescending(d => d.DataWyst).ToList());

            Podsumuj();
        }
        private void Podsumuj()
        {
            if (ewidencjaSzczegolyUCViewModel.ListaZamowienOdKlientow is null) return;

            var podsumowanie = new ZK_Podsumowanie();
            podsumowanie.Ilosc = ewidencjaSzczegolyUCViewModel.ListaZamowienOdKlientow.Sum(s => s.Ilosc);
            podsumowanie.Wartosc = ewidencjaSzczegolyUCViewModel.ListaZamowienOdKlientow.Sum(s => s.WartNetto);

            ewidencjaSzczegolyUCViewModel.Messenger.Send(podsumowanie);
        }
    }
}
