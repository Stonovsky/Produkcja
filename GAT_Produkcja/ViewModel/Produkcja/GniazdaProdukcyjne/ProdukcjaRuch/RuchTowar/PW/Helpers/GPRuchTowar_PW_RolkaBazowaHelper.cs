using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public class GPRuchTowar_PW_RolkaBazowaHelper : IGPRuchTowar_PW_RolkaBazowaHelper
    {
        #region Fields

        private readonly IGPRuchTowar_RolkaHelper rolkaHelper;

        #endregion

        #region CTOR
        public GPRuchTowar_PW_RolkaBazowaHelper(IGPRuchTowar_RolkaHelper rolkaHelper)
        {
            this.rolkaHelper = rolkaHelper;
        }

        #endregion

        #region Uzupelnienie danych ze zlecenia

        /// <summary>
        /// Uzupelnia dane bazowejRolkiPW na podstawie towaru na zleceniu produkcyjnym lub zleceniu ciecia
        /// </summary>
        /// <param name="bazowaRolkaPW">rolka bazowa do uzupelnienia</param>
        /// <param name="zlecenieTowar">towar ze zlecenia</param>
        /// <returns></returns>
        public tblProdukcjaRuchTowar PobierzDaneZeZlecenia(tblProdukcjaRuchTowar bazowaRolkaPW,
                                                           tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (bazowaRolkaPW is null)
                throw new ArgumentException("Rolka bazowa jest null!");
            if (zlecenieTowar is null) return bazowaRolkaPW;

            bazowaRolkaPW.IDGramatura = zlecenieTowar.IDTowarGeowlokninaParametryGramatura;
            bazowaRolkaPW.IDTowarGeowlokninaParametrySurowiec = zlecenieTowar.IDTowarGeowlokninaParametrySurowiec;
            bazowaRolkaPW.IDProdukcjaZlecenieTowar = zlecenieTowar.IDProdukcjaZlecenieTowar;
            bazowaRolkaPW.Szerokosc_m = zlecenieTowar.Szerokosc_m;
            bazowaRolkaPW.Dlugosc_m = zlecenieTowar.Dlugosc_m;
            bazowaRolkaPW.NazwaRolkiBazowej = new NazwaTowaruGenerator().GenerujNazweTowaru(bazowaRolkaPW);
            bazowaRolkaPW.NrDokumentu = zlecenieTowar.tblProdukcjaZlecenie?.NrDokumentu;
            bazowaRolkaPW.IDProdukcjaZlecenieProdukcyjne = zlecenieTowar.tblProdukcjaZlecenie?.IDProdukcjaZlecenie;
            bazowaRolkaPW.NrZlecenia = zlecenieTowar.tblProdukcjaZlecenie == null ? 0 : zlecenieTowar.tblProdukcjaZlecenie.NrZlecenia.GetValueOrDefault();
            return bazowaRolkaPW;
        }

        /// <summary>
        /// Zwraca nr zlecenia uwzgledniajac zlecenie ciecia oraz zlecenie produkcyjne
        /// </summary>
        /// <returns></returns>
        private int PobierzNrZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            int nrDefault = 0;

            if (zlecenieTowar is null) return 0;

            if (zlecenieTowar.tblProdukcjaZlecenieCiecia != null)
                return PobierzNrZeZleceniaCiecia(zlecenieTowar.tblProdukcjaZlecenieCiecia);

            if (zlecenieTowar.tblProdukcjaZlecenie != null)
                return PobierzNrZeZleceniaProdukcyjnego(zlecenieTowar.tblProdukcjaZlecenie);

            return nrDefault;
        }

        private int PobierzNrZeZleceniaCiecia(tblProdukcjaZlecenieCiecia zlecenieCiecia)
        {
            if (zlecenieCiecia != null)
                return zlecenieCiecia.NrZleceniaCiecia;

            return 0;
        }

        private int PobierzNrZeZleceniaProdukcyjnego(tblProdukcjaZlecenie zlecenieProdukcyjne)
        {
            if (zlecenieProdukcyjne != null)
            {
                if (zlecenieProdukcyjne.NrZlecenia.HasValue)
                    return zlecenieProdukcyjne.NrZlecenia.Value;
            }

            return 0;
        }

        #endregion

        #region Uzupelnienie danych z RW
        /// <summary>
        /// Przypisuje do <see cref="BazowaRolkaPW"/> dane pochodzace z rolki przslanej z formularza RW
        /// </summary>
        /// <returns></returns>
        public tblProdukcjaRuchTowar PobierzDaneZRolkiRw(tblProdukcjaRuchTowar bazowaRolkaPW,
                                                                     tblProdukcjaRuchTowar rolkaRW,
                                                                     tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne)
        {
            if (bazowaRolkaPW is null)
                throw new ArgumentException("Rolka bazowa jest null!");

            if (rolkaRW is null) return bazowaRolkaPW;

            bazowaRolkaPW.NrRolkiBazowej = rolkaRW.NrRolkiPelny;
            bazowaRolkaPW.CzyKalandrowana = rolkaRW.CzyKalandrowana;
            bazowaRolkaPW.SymbolRolkiBazowej = rolkaRW.TowarSymbolSubiekt;
            bazowaRolkaPW.NazwaRolkiBazowej = rolkaRW.TowarNazwaSubiekt;
            bazowaRolkaPW.IDProdukcjaRuchTowarWyjsciowy = rolkaRW.IDProdukcjaRuchTowar;
            bazowaRolkaPW.KodKreskowyRolkiBazowej = rolkaRW.KodKreskowy;
            bazowaRolkaPW.IDZleceniePodstawowe = rolkaRW.IDZleceniePodstawowe;
            bazowaRolkaPW.NrZleceniaPodstawowego = rolkaRW.NrZleceniaPodstawowego;
            bazowaRolkaPW.KierunekPrzychodu = rolkaRW.KierunekPrzychodu;
            if (gniazdoProdukcyjne is null) return bazowaRolkaPW;
            
            if (gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne != (int)GniazdaProdukcyjneEnum.LiniaWloknin)
                bazowaRolkaPW.IDRolkaBazowa = rolkaRW.IDProdukcjaRuchTowar;


            return bazowaRolkaPW;
        }

        #endregion

        #region Uzupelnienie danych z Gniazda produkcyjnego

        public async Task<tblProdukcjaRuchTowar> PobierzNoweNryDlaRolki(tblProdukcjaRuchTowar bazowaRolkaPW,
                                                                                  tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne,
                                                                                  IEnumerable<tblProdukcjaRuchTowar> listaPW)
        {
            if (bazowaRolkaPW is null)
                throw new ArgumentException("Rolka bazowa jest null!");

            if (gniazdoProdukcyjne is null) return bazowaRolkaPW;

            bazowaRolkaPW.NrRolkiPelny = await rolkaHelper.PobierzKolejnyPelnyNrRolkiAsync(gniazdoProdukcyjne, bazowaRolkaPW, listaPW).ConfigureAwait(false);
            bazowaRolkaPW.NrRolki = await rolkaHelper.PobierzKolejnyNrRolkiAsync(gniazdoProdukcyjne, listaPW).ConfigureAwait(false);

            return bazowaRolkaPW;
        }

        #endregion

    }
}
