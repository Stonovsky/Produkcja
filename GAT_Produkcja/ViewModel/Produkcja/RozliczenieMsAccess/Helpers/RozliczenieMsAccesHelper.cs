using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Models.Adapters;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.EppFile;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.Utilities.Wrappers;
using GAT_Produkcja.ViewModel.Produkcja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Dictionaries;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Adapters;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.RW;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public class RozliczenieMsAccesHelper : IRozliczenieMsAccesHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;
        private readonly IRozliczenieSQL_RW_Helper rwHelper;
        private readonly IRozliczenieSQL_PW_Helper pwHelper;
        //private readonly SurowceDictionary surowceDictionary;
        private readonly NazwaTowaruSubiektHelper nazwaTowaruSubiekt;
        private readonly ISurowiecSubiektStrategy surowiecStrategy;
        private IEnumerable<Surowiec> listaSurowcow;
        private IEnumerable<NormyZuzycia> listNormZuzycia;
        private IEnumerable<vwMagazynRuchGTX> listaSurowcowSubiekt;
        private IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> listaCenTransferowychGTEX;

        public IXlsService ExcelReportGenerator { get; }
        public IEppFileGenerator EppFileGenerator { get; }
        public IDirectoryHelper DirectoryHelper { get; }

        public RozliczenieMsAccesHelper(IUnitOfWork unitOfWork,
                                        IUnitOfWorkMsAccess unitOfWorkMsAccess,
                                        IXlsService excelReportGenerator,
                                        IEppFileGenerator eppFileGenerator,
                                        IRozliczenieSQL_RW_Helper rwHelper,
                                        IRozliczenieSQL_PW_Helper pwHelper,
                                        IDirectoryHelper directoryHelper
                                        )
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;
            this.rwHelper = rwHelper;
            this.pwHelper = pwHelper;

            DirectoryHelper = directoryHelper;
            ExcelReportGenerator = excelReportGenerator;
            EppFileGenerator = eppFileGenerator;

            nazwaTowaruSubiekt = new NazwaTowaruSubiektHelper();
        }

        /// <summary>
        /// Pobiera dane wejsciowe np. liste surowcow z MsAccess
        /// </summary>
        /// <returns></returns>
        public async Task LoadAsync()
        {
            await  rwHelper.LoadAsync();
            await  pwHelper.LoadAsync();
        }

        #region RW
        /// <summary>
        /// Tworzy rozliczenie RW dla pozycji mieszanki wraz z idTowaru z Subiekt oraz udzialami
        /// </summary>
        /// <param name="mieszanka">encje NormZuzycia dla mieszanki</param>
        /// <returns></returns>
        public async Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia)
        {
            return await rwHelper.GenerujRozliczenieRWAsync(wybranaPozycjaKonfekcjiDoRozliczenia);
        }

        /// <summary>
        /// Dodaje do listy RW ilosci zgodnie z udzialem. 
        /// Sumaryczna ilosc kg bierze z PW jako sume z wagi i sume z odpadu dla wszystkich elementow
        /// </summary>
        /// <param name="listaRWSurowca"></param>
        /// <param name="listaPwWyrobuGotowego"></param>
        public async Task DodajIlosciKgIWartoscDoRW(ObservableCollection<tblProdukcjaRozliczenie_RW> listaRWSurowca,
                                              ObservableCollection<tblProdukcjaRozliczenie_PW> listaPwWyrobuGotowego)
        {
            await rwHelper.DodajIlosciKgIWartoscDoRW(listaRWSurowca, listaPwWyrobuGotowego);
        }

        public RwPodsumowanieModel PodsumujRW(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW)
        {
            return rwHelper.PodsumujRW(listaRW);
        }

        public async Task<decimal> GenerujCeneMieszanki(IEnumerable<NormyZuzycia> mieszanka)
        {
            throw new NotImplementedException();
        }

        public decimal GenerujCeneMieszanki(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW)
        {
            return rwHelper.GenerujCeneMieszanki(listaRW);
        }

        /// <summary>
        /// Generuje cene mieszanki dla konkretnego zlecenia
        /// </summary>
        /// <param name="idZlecenia"></param>
        /// <returns></returns>
        public async Task<decimal> GenerujCeneMieszanki(int idZlecenia)
        {
            return await rwHelper.GenerujCeneMieszanki(idZlecenia);
        }

        #endregion

        #region PW
        public IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IGniazdoProdukcyjne> listaPozycjiKonfekcji, decimal cenaKgMieszanki)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IProdukcjaRuchTowar> listaPozycjiKonfekcji, decimal cenaKgMieszanki)
        {
            return pwHelper.GenerujRozliczeniePW(listaPozycjiKonfekcji, cenaKgMieszanki);
        }

        public tblProdukcjaRozliczenie_PW GenerujOdpadDlaPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {
            return pwHelper.GenerujOdpadDlaPW(listaPW);
        }

        public async Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie)
        {
            return await rwHelper.GenerujOdpadDlaPW(idZlecenie);
        }

        public PwPodsumowanieModel PodsumujPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {
            return pwHelper.PodsumujPW(listaPW);
        }

        public IEnumerable<tblProdukcjaRozliczenie_PW> PodsumujPWPodzialTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW)
        {
            return pwHelper.PodsumujPWPodzialTowar(listaPW);
        }

        public PwPodsumowanieModel PodsumujListe(IEnumerable<Konfekcja> lista)
        {
            throw new NotImplementedException();

        }
        public PwPodsumowanieModel PodsumujListe(IEnumerable<tblProdukcjaRozliczenie_PW> lista)
        {
            return pwHelper.PodsumujListe(lista);
        }
        private string PobierzZlecenia(IEnumerable<Konfekcja> lista)
        {
            lista = lista.OrderBy(s => s.ZlecenieID).ToList();
            var zlecenia = lista.Select(s => s.Zlecenie).Distinct();

            return string.Join(",", zlecenia);
        }
        #endregion

        #region Laczone RW i PW
        public async Task<IEnumerable<tblProdukcjaRozliczenie_PW>> GenerujZgrupowanaListePoNazwieZObliczeniemKosztow(IEnumerable<IGniazdoProdukcyjne> listaPobranaZMsAccess)
        {
            var listaZgrupowana = new List<tblProdukcjaRozliczenie_PW>();

            var idZlecenProdukcyjnych = listaPobranaZMsAccess.Select(s => s.ZlecenieID)
                                                                  .Distinct();
            foreach (var idZlecenia in idZlecenProdukcyjnych)
            {
                var cenaMieszanki = await GenerujCeneMieszanki(idZlecenia);
                var pozycjeDlaIdZlecenia = listaPobranaZMsAccess.Where(z => z.ZlecenieID == idZlecenia);

                var zgrupowanePozycjeDlaZlecenia = GenerujRozliczeniePW(pozycjeDlaIdZlecenia, cenaMieszanki);

                listaZgrupowana.AddRange(zgrupowanePozycjeDlaZlecenia);
            }
            return listaZgrupowana;

        }

        #endregion

        #region Konfekcja
        public async Task<IEnumerable<Konfekcja>> PobierzListeKonfekcjiDlaZlecenia(int idZlecenia)
        {
            if (idZlecenia == 0)
                throw new ArgumentException("Zerowe idZlecenia przesłane do metody");

            var lista = await unitOfWorkMsAccess.Konfekcja.GetByZlecenieIdAsync(idZlecenia);
            lista = lista.Where(k => k.CzyZaksiegowano == false)
                         .Where(k => k.Przychody == "Linia")
                         .Where(k => k.NrWz != "0")
                         .ToList();
            return lista;
        }

        public async Task<IEnumerable<IProdukcjaRuchTowar>> PobierzKonfekcjeDoRozliczenia()
        {
            var listaKonfekcji = await unitOfWorkMsAccess.Konfekcja.GetUnaccountedAsync().ConfigureAwait(false);

            if (listaKonfekcji is null)
                throw new ArgumentException("Brak wyprodukowanych rolek dla danych parametrów.");

            listaKonfekcji = listaKonfekcji.Where(l => l.NrWz != "0").OrderByDescending(x => x.Data);

            return listaKonfekcji.Select(konf => new KonfekcjaAdapter(konf));
        }
        #endregion
    }
}
