using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Decorator
{
    /// <summary>
    /// Klasa uzupelniajaca rolke o dane niezbedne do dodania do bazy
    /// </summary>
    public class RolkaPWDecorator
    {
        private NazwaTowaruSubiektHelper nazwaTowaruSubiektHelper;

        #region CTOR
        public RolkaPWDecorator()
        {
            nazwaTowaruSubiektHelper = new NazwaTowaruSubiektHelper();

        }
        #endregion

        /// <summary>
        /// Uzupelnienie niezbednych danych dla rolki PW przed dodaniem do bazy
        /// </summary>
        /// <param name="idRuchNaglowek">id naglowka</param>
        /// <param name="pozycjaPW">rolka PW</param>
        /// <returns>void</returns> 
        public virtual async Task UzupelnijPozycjePW(int? idRuchNaglowek,
                                              tblProdukcjaRuchTowar pozycjaPW,
                                              GPRuchTowarPWViewModel pwViewModel)
        {
            pozycjaPW.IDProdukcjaGniazdoProdukcyjne = pwViewModel.GniazdoProdukcyjne?.IDProdukcjaGniazdoProdukcyjne;
            pozycjaPW.IDProdukcjaZlecenieTowar = pwViewModel.ZlecenieTowar?.IDProdukcjaZlecenieTowar ?? 0;
            pozycjaPW.IDProdukcjaRuchNaglowek = idRuchNaglowek.Value;
            pozycjaPW.IDProdukcjaRuchTowarWyjsciowy = pwViewModel.RolkaRW?.IDProdukcjaRuchTowar;
            pozycjaPW.IDRolkaBazowa = pwViewModel.RolkaRW?.IDProdukcjaRuchTowar;
            pozycjaPW.IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW;
            pozycjaPW.IDProdukcjaRuchNaglowek = idRuchNaglowek.GetValueOrDefault();
            pozycjaPW.IDProdukcjaRozliczenieStatus = (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
            pozycjaPW.Cena_kg = await pwViewModel.PwHelper.RolkaHelper.PobierzKosztRolki(pozycjaPW, (GniazdaProdukcyjneEnum)pwViewModel.GniazdoProdukcyjne?.IDProdukcjaGniazdoProdukcyjne);
            pozycjaPW.Cena_m2 = PobierzCenaDla(pozycjaPW);
            pozycjaPW.TowarNazwaMsAccess = nazwaTowaruSubiektHelper.GenerujNazweTowaru(pozycjaPW);
            pozycjaPW.TowarSymbolSubiekt = nazwaTowaruSubiektHelper.GenerujSymbolTowaru(pozycjaPW);
            pozycjaPW.NrDokumentu = pozycjaPW.NrRolkiPelny;
            pozycjaPW.SurowiecSkrot = pozycjaPW.tblTowarGeowlokninaParametrySurowiec?.Skrot;

            var towar = await pwViewModel.UnitOfWork.tblTowar.PobierzTowarZParametrowAsync(pwViewModel.WybranaGramatura, pwViewModel.WybranySurowiec, false);
            pozycjaPW.IDTowar = towar == null ? (int?)null : towar.IDTowar;
            pozycjaPW.TowarNazwaSubiekt = new NazwaTowaruGenerator().GenerujNazweTowaru(pozycjaPW);
        }

        private decimal PobierzCenaDla(tblProdukcjaRuchTowar pozycjaPW)
        {
            if (pozycjaPW.Szerokosc_m != 0 && pozycjaPW.Dlugosc_m != 0)
            {
                var wagaM2 = pozycjaPW.Waga_kg / (pozycjaPW.Szerokosc_m * pozycjaPW.Dlugosc_m);
                return wagaM2 * pozycjaPW.Cena_kg;
            }
            return 0;
        }


    }
}
