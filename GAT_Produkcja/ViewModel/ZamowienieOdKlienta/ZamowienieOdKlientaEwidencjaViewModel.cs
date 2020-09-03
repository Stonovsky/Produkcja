using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Models;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta
{
    public class ZamowienieOdKlientaEwidencjaViewModel : ListCommandViewModelBase
    {
        private IEnumerable<vwZamOdKlientaAGG> listaZamowien;
        private List<string> status = new List<string> { "Nie zrealizowano", "Zrealizowano" };
        private ObservableCollection<vwZamOdKlientaAGG> listaZamowienOdKlientow;
        private string wybranaGrupa;
        private string wybranyStatus;
        private string nazwaTowaru;
        #region Properties
        public ObservableCollection<vwZamOdKlientaAGG> ListaZamowienOdKlientow
        {
            get => listaZamowienOdKlientow;
            set
            {
                listaZamowienOdKlientow = value;
            }
        }

        public vwZamOdKlientaAGG WybraneZamowienieOdKlienta { get; set; }

        public string NazwaTowaru
        {
            get => nazwaTowaru; set
            {
                nazwaTowaru = value;
                Filtr.NazwaTowaru = NazwaTowaru;
            }
        }
        public List<string> Status { get; set; } = new List<string> { "Nie zrealizowano", "Zrealizowano" };
        public string WybranyStatus
        {
            get => wybranyStatus; set
            {
                wybranyStatus = value;
                Filtr.Status = WybranyStatus;
            }
        }

        public List<string> Grupa { get; set; }
        public string WybranaGrupa
        {
            get => wybranaGrupa;
            set
            {
                wybranaGrupa = value;
                Filtr.Grupa = WybranaGrupa;
            }
        }
        public ZK_Filtr Filtr { get; set; } = new ZK_Filtr();
        public string Tytul { get; set; }
        public ZK_Podsumowanie Podsumowanie { get; set; }
        public bool CzySzukajButtonAktywny { get; set; } = true;
        public tblPracownikGAT Uzytkownik { get; set; } = UzytkownikZalogowany.Uzytkownik;
        #endregion

        #region Command
        public RelayCommand FiltrujCommand { get; set; }
        public IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel SzczegolyUCViewModel { get; }

        #endregion

        #region CTOR
        public ZamowienieOdKlientaEwidencjaViewModel(IViewModelService viewModelService,
                                                     IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel SzczegolyUCViewModel) 
            : base(viewModelService)
        {
            FiltrujCommand = new RelayCommand(FiltrujCommandExecute, FiltrujCommandCanExecute);

            Tytul = "Ewidencja zamówień od Klientów";

            WybranyStatus = Status.First();
            this.SzczegolyUCViewModel = SzczegolyUCViewModel;
            Messenger.Register<ZK_Podsumowanie>(this, GdyPrzeslanoPodsumowanie);
        }

        #endregion
        private void GdyPrzeslanoPodsumowanie(ZK_Podsumowanie obj)
        {
            if (obj is null) return;
            Podsumowanie = obj;
            CzySzukajButtonAktywny = true;
        }

        #region FiltrujCommand
        private bool FiltrujCommandCanExecute()
        {
            return CzySzukajButtonAktywny;
        }

        private void FiltrujCommandExecute()
        {
            CzySzukajButtonAktywny = false;
            Messenger.Send(Filtr);
        }
        #endregion

        protected override async void LoadCommandExecute()
        {
            SzczegolyUCViewModel.SetState(this.GetType().Name);
            await SzczegolyUCViewModel.LoadAsync(null);
            listaZamowien = await UnitOfWork.vwZamOdKlientaAGG.GetAllAsync();
            Grupa = listaZamowien.Select(s => s.Grupa).Distinct().ToList();
            Grupa.Sort();
        }
    }
}
