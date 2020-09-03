using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.Sprzedaz.Ewidencja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.Sprzedaz.Ewidencja
{
    public class SprzedazEwidencjaViewModel : ListCommandViewModelBase
    {
        #region Properties

        public IEnumerable<vwZestSprzedazyAGG> ListaSprzedazy { get; set; }
        public vwZestSprzedazyAGG WybranaSprzedaz { get; set; }

        private IEnumerable<vwZestSprzedazyAGG> sprzedaz;

        public List<string> ListaGrup { get; set; }
        public string WybranaGrupa { get; set; }
        public List<string> ListaHandlowcow { get; set; }
        public string WybranyHandlowiec { get; set; }
        public string NazwaKontrahenta { get; set; }
        public string Towar { get; set; }
        public DateTime DataSprzedazyOd { get; set; } = DateTime.Now.Date.StartOfWeek(DayOfWeek.Monday);
        public DateTime DataSprzedazyDo { get; set; } = DateTime.Now.Date.StartOfWeek(DayOfWeek.Monday).AddDays(7);

        public PodsumowanieSprzedazyModel Podsumowanie { get; set; }
        public string Tytul { get; set; }
        #endregion

        #region Commands
        public RelayCommand FiltrujCommand{ get; set; }
        #endregion
        public SprzedazEwidencjaViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
            FiltrujCommand = new RelayCommand(FiltrujCommandExecute);

            Tytul = "Ewidencja sprzedaży AG Geosynthetics sp. z o.o., sp.k.";
        }

        private void FiltrujCommandExecute()
        {
            ListaSprzedazy = new List<vwZestSprzedazyAGG>(sprzedaz);
            if (WybranaGrupa != null)
               ListaSprzedazy = ListaSprzedazy.Where(g => g.Grupa == WybranaGrupa).ToList();
            
            if (!string.IsNullOrEmpty(WybranyHandlowiec))
                ListaSprzedazy = ListaSprzedazy.Where(h => h.Handlowiec == WybranyHandlowiec).ToList();
                
            if(DataSprzedazyOd!=null)
                ListaSprzedazy = ListaSprzedazy.Where(h => h.DataSprzedazy >= DataSprzedazyOd).ToList();

            if (DataSprzedazyDo != null)
                ListaSprzedazy = ListaSprzedazy.Where(h => h.DataSprzedazy <= DataSprzedazyDo).ToList();

            if(NazwaKontrahenta != null)
                ListaSprzedazy = ListaSprzedazy.Where(h => h.NazwaKontrahenta.ToLower().Contains(NazwaKontrahenta.ToLower())).ToList();

            if (Towar != null)
                ListaSprzedazy = ListaSprzedazy.Where(h => h.Towar.ToLower().Contains(Towar.ToLower())).ToList();

            Podsumuj();
        }

        protected override async void LoadCommandExecute()
        {
            sprzedaz = await UnitOfWork.vwZestSprzedazyAGG.GetAllAsync();
            ListaSprzedazy = new List<vwZestSprzedazyAGG>(sprzedaz);

            ListaGrup = sprzedaz.Select(s => s.Grupa).Distinct().ToList();
            ListaGrup.Sort();

            ListaHandlowcow = sprzedaz.Select(s => s.Handlowiec).Distinct().ToList();
            ListaHandlowcow.Sort();

            FiltrujCommandExecute();
            //Podsumuj();
        }

        private void Podsumuj()
        {
            Podsumowanie = new PodsumowanieSprzedazyModel
            {
                Ilosc = ListaSprzedazy.Sum(s => s.Ilosc),
                Netto = ListaSprzedazy.Sum(s => s.Netto),
                Zysk = ListaSprzedazy.Sum(s => s.Zysk),
            };

            if (Podsumowanie.Netto != 0)
                Podsumowanie.Marza = Podsumowanie.Zysk / Podsumowanie.Netto;
        }
    }
}
