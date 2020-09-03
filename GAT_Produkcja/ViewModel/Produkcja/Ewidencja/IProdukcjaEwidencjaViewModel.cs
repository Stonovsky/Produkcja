using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja
{
    public interface IProdukcjaEwidencjaViewModel
    {
        DateTime DataKoniec { get; set; }
        DateTime DataPoczatek { get; set; }
        IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKalandra { get; }
        IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKonfekcji { get; }
        IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiLiniWloknin { get; }
        ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiWloknin { get; }
        ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiKalandra { get; }
        ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiKonfekcji { get; }
        ProdukcjaEwidencjaPodsumowanieModel Podsumowanie { get; }
        RelayCommand SzukajCommand { get; set; }
        string TowarNazwaFiltr { get; set; }
        int ZaznaczonyTabItem { get; set; }
    }
}