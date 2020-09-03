using GalaSoft.MvvmLight;
using GAT_Produkcja.db;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess
{
    [AddINotifyPropertyChangedInterface]
    public abstract class ProdukcjaEwidencjaStateBase : ViewModelBase, IProdukcjaEwidencjaState
    {
        #region Properties

        public DateTime DataPoczatek { get; set; } = DateTime.Now.Date;
        public DateTime DataKoniec { get; set; } = DateTime.Now.Date;

        public IUnitOfWorkMsAccess UnitOfWorkMsAccess { get; }
        public IProdukcjaEwidencjaHelper Helper { get; }

        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKalandra { get; set; }
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKonfekcji { get; set; }
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiLiniWloknin { get; set; }
        public ObservableCollection<IGniazdoProdukcyjne> ListaPozycjiWloknin { get; set; }
        public ObservableCollection<IGniazdoProdukcyjne> ListaPozycjiKalandra { get; set; }
        public ObservableCollection<IGniazdoProdukcyjne> ListaPozycjiKonfekcji { get; set; }
        public ProdukcjaEwidencjaPodsumowanieModel Podsumowanie { get; set; }
        protected IEnumerable<tblProdukcjaRozliczenie_PW> listaZgrupowanaBazowa;
        public string TowarNazwaFiltr { get; set; }
        public bool ValidacjaCache { get; set; }
        #endregion

        #region CTOR
        public ProdukcjaEwidencjaStateBase(IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                           IProdukcjaEwidencjaHelper helper)
        {
            UnitOfWorkMsAccess = unitOfWorkMsAccess;
            Helper = helper;

            Task.Run(() => LoadAsync());
        }

        #endregion

        protected virtual async Task<IEnumerable<tblProdukcjaRozliczenie_PW>> GrupujListeRolekWgNazwyTowaru(IEnumerable<IGniazdoProdukcyjne> listaPozycjiDoPodsumowania)
        {
            if (CzyListaPusta(listaPozycjiDoPodsumowania))
                throw new ArgumentException("Brak wyprodukowanych rolek w podanych datach.");

            List<tblProdukcjaRozliczenie_PW> listaZgrupowana = await GrupujPozycje(listaPozycjiDoPodsumowania);

            return Helper.RozliczenieMsAccesHelper.PodsumujPWPodzialTowar(listaZgrupowana);
        }

        private async Task<List<tblProdukcjaRozliczenie_PW>> GrupujPozycje(IEnumerable<IGniazdoProdukcyjne> listaPozycjiDoPodsumowania)
        {
            var listaZgrupowana = new List<tblProdukcjaRozliczenie_PW>();

            var idZlecenProdukcyjnych = listaPozycjiDoPodsumowania.Select(s => s.ZlecenieID)
                                                                  .Distinct();
            foreach (var idZlecenia in idZlecenProdukcyjnych)
            {
                var cenaMieszanki = await Helper.RozliczenieMsAccesHelper.GenerujCeneMieszanki(idZlecenia);
                var pozycjeDlaIdZlecenia = listaPozycjiDoPodsumowania.Where(z => z.ZlecenieID == idZlecenia);

                var zgrupowanePozycjeDlaZlecenia = Helper.RozliczenieMsAccesHelper.GenerujRozliczeniePW(pozycjeDlaIdZlecenia, cenaMieszanki);

                listaZgrupowana.AddRange(zgrupowanePozycjeDlaZlecenia);
            }
            return listaZgrupowana;
        }

        protected bool CzyListaPusta(IEnumerable<IGniazdoProdukcyjne> listaPozycji)
        {
            if (listaPozycji is null || !listaPozycji.Any())
                return true;

            return false;
        }

        protected ProdukcjaEwidencjaPodsumowanieModel GenerujPodsumowanieListy(IEnumerable<tblProdukcjaRozliczenie_PW> listaPozycji)
        {
            if (listaPozycji is null)
            {
                return null;
            }

            return new ProdukcjaEwidencjaPodsumowanieModel
            {
                WagaSuma = listaPozycji.Sum(s => s.Ilosc_kg),
                OdpadSuma = listaPozycji.Sum(s => s.Odpad_kg),
                IloscSuma = listaPozycji.Sum(s => s.Ilosc),
                IloscSztSuma = listaPozycji.Sum(s => s.Ilosc_szt),
                WartoscSuma = listaPozycji.Sum(s => s.Wartosc) + listaPozycji.Sum(s => s.WartoscOdpad),
            };
        }
        public abstract Task PobierzListeRolekZMsAccess();
        public abstract Task GrupujTowary();
        public abstract void PodsumujListe();

        public virtual IEnumerable<tblProdukcjaRozliczenie_PW> FiltrujTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaDoFiltrowania)
        {
            if (string.IsNullOrWhiteSpace(TowarNazwaFiltr))
                return listaDoFiltrowania;

            return listaDoFiltrowania.Where(t => t.NazwaTowaruSubiekt.ToLower().Contains(TowarNazwaFiltr.ToLower()));

        }

        public async Task LoadAsync()
        {
            await Helper.RozliczenieMsAccesHelper.LoadAsync();
        }
    }
}
