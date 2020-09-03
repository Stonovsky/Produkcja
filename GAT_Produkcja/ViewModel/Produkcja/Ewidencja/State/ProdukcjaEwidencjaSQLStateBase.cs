using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Models;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State
{
    public abstract class ProdukcjaEwidencjaSQLStateBase : IProdukcjaEwidencjaSQLState
    {
        protected IUnitOfWork unitOfWork;
        protected IProdukcjaEwidencjaHelper produkcjaEwidencjaHelper;
        protected IRozliczenieSQLHelper rozliczenieSQLHelper;
        protected IProdukcjaEwidencjaHelper helper;
        protected IEnumerable<tblProdukcjaRozliczenie_PW> listaZgrupowanaBazowa;
        protected IEnumerable<tblProdukcjaZlecenie> listaZlecen;

        public DateTime DataKoniec { get; set; } = DateTime.Now.Date;
        public DateTime DataPoczatek { get; set; } = DateTime.Now.Date;
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKalandra { get; set; }
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiKonfekcji { get; set; }
        public IEnumerable<tblProdukcjaRozliczenie_PW> ListaZgrupowanychPozycjiLiniWloknin { get; set; }
        public ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiWloknin { get; set; }
        public ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiKalandra { get; set; }
        public ObservableCollection<tblProdukcjaRuchTowar> ListaPozycjiKonfekcji { get; set; }
        public ProdukcjaEwidencjaPodsumowanieModel Podsumowanie { get; set; }
        public string TowarNazwaFiltr { get; set; }


        #region CTOR
        public ProdukcjaEwidencjaSQLStateBase(IUnitOfWork unitOfWork,
                                              IRozliczenieSQLHelper rozliczenieSQLHelper,
                                              IProdukcjaEwidencjaHelper helper)
        {
            this.unitOfWork = unitOfWork;
            this.rozliczenieSQLHelper = rozliczenieSQLHelper;
            this.helper = helper;
        }
        #endregion
        public abstract Task PobierzListeRolek();
        public abstract Task GrupujTowary();
        public abstract void PodsumujListe();


        public async Task LoadAsync()
        {
            if (listaZlecen is null)
            {
                listaZlecen = await unitOfWork.tblProdukcjaZlecenie.GetAllAsync();
                await helper.RozliczenieMsAccesHelper.LoadAsync();
            }
        }


        protected virtual async Task<IEnumerable<tblProdukcjaRozliczenie_PW>> GrupujListeRolekWgNazwyTowaru(IEnumerable<tblProdukcjaRuchTowar> listaPozycjiDoPodsumowania)
        {
            if (CzyListaPusta(listaPozycjiDoPodsumowania))
                throw new ArgumentException("Brak wyprodukowanych rolek w podanych datach.");

            List<tblProdukcjaRozliczenie_PW> listaZgrupowana = await GrupujPozycje(listaPozycjiDoPodsumowania);

            return rozliczenieSQLHelper.PodsumujPWPodzialTowar(listaZgrupowana);
        }
        private async Task<List<tblProdukcjaRozliczenie_PW>> GrupujPozycje(IEnumerable<tblProdukcjaRuchTowar> listaPozycjiDoPodsumowania)
        {
            var listaZgrupowana = new List<tblProdukcjaRozliczenie_PW>();

            var idZlecenProdukcyjnych = listaPozycjiDoPodsumowania.Select(s => s.IDProdukcjaZlecenieProdukcyjne)
                                                                  .Distinct();
            foreach (var idZlecenia in idZlecenProdukcyjnych)
            {
                var zlecenieProdukcyjne = listaZlecen.SingleOrDefault(z => z.IDProdukcjaZlecenie == idZlecenia.Value);
                var cenaMieszanki = await PobierzCeneMieszanki(zlecenieProdukcyjne);

                var pozycjeDlaIdZlecenia = listaPozycjiDoPodsumowania.Where(z => z.IDProdukcjaZlecenieProdukcyjne == idZlecenia.Value);

                var zgrupowanePozycjeDlaZlecenia = rozliczenieSQLHelper.GenerujRozliczeniePW(pozycjeDlaIdZlecenia, cenaMieszanki);

                listaZgrupowana.AddRange(zgrupowanePozycjeDlaZlecenia);
            }
            return listaZgrupowana;
        }
        private async Task<decimal> PobierzCeneMieszanki(tblProdukcjaZlecenie zlecenie)
        {
            if (zlecenie is null) throw new ArgumentNullException(nameof(zlecenie));

            if (zlecenie.IDMsAccess != null && zlecenie.IDMsAccess > 0)
                return await helper.RozliczenieMsAccesHelper.GenerujCeneMieszanki(zlecenie.IDMsAccess.GetValueOrDefault());
            else
                return zlecenie.CenaMieszanki_zl;

        }
        protected bool CzyListaPusta(IEnumerable<tblProdukcjaRuchTowar> listaPozycji)
        {
            if (listaPozycji is null || !listaPozycji.Any())
                return true;

            return false;
        }
        public virtual IEnumerable<tblProdukcjaRozliczenie_PW> FiltrujTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaDoFiltrowania)
        {
            if (string.IsNullOrWhiteSpace(TowarNazwaFiltr))
                return listaDoFiltrowania;

            return listaDoFiltrowania.Where(t => t.NazwaTowaruSubiekt.ToLower().Contains(TowarNazwaFiltr.ToLower()));

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

    }
}
