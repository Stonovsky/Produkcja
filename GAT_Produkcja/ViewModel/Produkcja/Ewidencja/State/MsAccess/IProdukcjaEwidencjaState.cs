using GAT_Produkcja.db;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess
{
    public interface IProdukcjaEwidencjaState
    {
        #region Properties

        DateTime DataKoniec { get; set; }
        DateTime DataPoczatek { get; set; }

        #region Listy zgrupowane po towarze
        IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKalandra { get; set; }
        IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKonfekcji { get; set; }
        IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiLiniWloknin { get; set; }
        #endregion

        #region Listy w podziale na rolki
        ObservableCollection<IGniazdoProdukcyjne> ListaPozycjiWloknin { get; set; }
        ObservableCollection<IGniazdoProdukcyjne> ListaPozycjiKalandra { get; set; }
        ObservableCollection<IGniazdoProdukcyjne> ListaPozycjiKonfekcji { get; set; }
        #endregion

        ProdukcjaEwidencjaPodsumowanieModel Podsumowanie { get; set; }
        string TowarNazwaFiltr { get; set; }

        #endregion


        Task PobierzListeRolekZMsAccess();
        Task GrupujTowary();
        void PodsumujListe();
        Task LoadAsync();

    }
}
