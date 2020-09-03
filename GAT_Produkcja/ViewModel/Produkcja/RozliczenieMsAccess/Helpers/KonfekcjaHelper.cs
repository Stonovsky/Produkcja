using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy.Factory;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public class KonfekcjaHelper : IKonfekcjaHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISurowiecSubiektStrategy surowiecSubiektStrategy;
        #region CTOR
        public KonfekcjaHelper(IUnitOfWork unitOfWork,
                               ISurowiecSubiektStrategyFactory surowiecSubiektStrategyFactory)
        {
            this.unitOfWork = unitOfWork;

            surowiecSubiektStrategy = surowiecSubiektStrategyFactory.PobierzStrategie(SurowiecSubiektFactoryEnum.ZNazwy);

        }
        #endregion

        public async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzKonfekcjeDoRozliczenia()
        {
            return await unitOfWork.tblProdukcjaRuchTowar.WhereAsync(e => e.IDProdukcjaRozliczenieStatus == (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono
                                                                       && e.KierunekPrzychodu == "Linia"
                                                                       && e.NrPalety != 0);
        }

        public async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzListeKonfekcjiDlaZlecenia(int idZlecenia)
        {
            if (idZlecenia == 0)
                throw new ArgumentException(nameof(idZlecenia));

            return await unitOfWork.tblProdukcjaRuchTowar
                                    .WhereAsync(e => e.IDProdukcjaZlecenieProdukcyjne == idZlecenia
                                                && e.IDProdukcjaRozliczenieStatus == (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono
                                                && e.KierunekPrzychodu == "Linia"
                                                && e.NrPalety != 0);
        }

        public async Task<decimal> PobierzCeneMieszankiDlaZleceniaProdukcji(int idZlecenieProdukcyjne)
        {
            if (idZlecenieProdukcyjne == 0) throw new ArgumentException(nameof(idZlecenieProdukcyjne));

            var mieszanka = await unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka.WhereAsync(m => m.IDProdukcjaZlecenieProdukcyjne == idZlecenieProdukcyjne);
            decimal cena = 0;
            foreach (var surowiec in mieszanka)
            {
                var towarRuch = await unitOfWork.vwMagazynRuchGTX.WhereAsync(t => t.IdTowar == surowiec.IDTowar
                                                                               && t.Ilosc > 0
                                                                               && t.Cena > 0);
                cena += surowiec.ZawartoscProcentowa * towarRuch.First().Cena;
            }

            return cena;
        }

        public async Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia)
        {
            if (wybranaPozycjaKonfekcjiDoRozliczenia is null)
                throw new ArgumentNullException($"Brak zlecenia jako argumentu funkcji {nameof(GenerujRozliczenieRWAsync)}.");

            //TODO pobierzRolkeBazowa
            //var rolkaBazowa = await unitOfWork.tblProdukcjaRuchTowar.SingleOrDefaultAsync(r=>r.IDProdukcjaRuchTowar==wybranaPozycjaKonfekcjiDoRozliczenia.idr)

            // pobierz mieszanke dla zlecenia
            var mieszanka = await unitOfWork.tblProdukcjaZlecenieProdukcyjne_Mieszanka
                                    .WhereAsync(e => e.IDProdukcjaZlecenieProdukcyjne == wybranaPozycjaKonfekcjiDoRozliczenia.IDZlecenie);

            //dla kazdej pozycji wygeneruj rozliczenie wg udziału, ceny i ilosci
            var listaRW = new List<tblProdukcjaRozliczenie_RW>();
            foreach (var surowiec in mieszanka)
            {
                vwMagazynRuchGTX surowiecDostepny = await PobierzSurowiecDostepny(surowiec);
                tblProdukcjaRozliczenie_RW surowiecRozliczenie = GenerujEncjeRozliczenia(surowiec, surowiecDostepny);

                listaRW.Add(surowiecRozliczenie);
            }
            return listaRW;
        }

        private tblProdukcjaRozliczenie_RW GenerujEncjeRozliczenia(tblProdukcjaZlecenieProdukcyjne_Mieszanka surowiec, vwMagazynRuchGTX surowiecDostepny)
        {
            return new tblProdukcjaRozliczenie_RW
            {

                IDNormaZuzyciaMsAccess = surowiec.IDMsAccess.GetValueOrDefault(),
                IDSurowiecSubiekt = surowiecDostepny.IdTowar,
                CenaJednostkowa = surowiecDostepny.Cena,
                NazwaTowaruSubiekt = surowiecDostepny.TowarNazwa,
                SymbolTowaruSubiekt = surowiecDostepny.TowarSymbol,
                IDZlecenie = surowiec.IDProdukcjaZlecenieProdukcyjne.GetValueOrDefault(),
                Udzial = surowiec.ZawartoscProcentowa,
                DataDodania = DateTime.Now,
                IDJm = (int)JmEnum.kg,
                Jm = "kg"
            };
        }

        private async Task<vwMagazynRuchGTX> PobierzSurowiecDostepny(tblProdukcjaZlecenieProdukcyjne_Mieszanka surowiec)
        {
            var towar = await unitOfWork.vwTowarGTX.SingleOrDefaultAsync(e => e.IdTowar == surowiec.IDTowar);
            vwMagazynRuchGTX surowiecDostepny = null;
            var surowiecDostepnyNaMagazynie = await unitOfWork.vwMagazynRuchGTX.WhereAsync(e => e.IdTowar == surowiec.IDTowar);

            if (surowiecDostepnyNaMagazynie is null || surowiecDostepnyNaMagazynie.Sum(s => s.Ilosc) == 0)
                surowiecDostepny = await surowiecSubiektStrategy.PobierzSurowiecZSubiektDla(towar.Symbol.Replace("_", ""));

            return surowiecDostepnyNaMagazynie.FirstOrDefault();
        }
    }
}
