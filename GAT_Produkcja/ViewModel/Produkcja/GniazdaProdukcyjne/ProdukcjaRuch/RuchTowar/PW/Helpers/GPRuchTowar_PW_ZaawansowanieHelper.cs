using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    //TODO dorobic interface
    public class GPRuchTowar_PW_ZaawansowanieHelper : IGPRuchTowar_PW_ZaawansowanieHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<tblProdukcjaZlecenieTowar> listaTowarowDlaZlecenia;
        private decimal iloscPlanowanaDlaZlecenia;
        private tblProdukcjaZlecenieTowar zlecenieTowar;
        private IEnumerable<tblProdukcjaRuchTowar> rolkiDlaZlecenieTowar;
        private tblProdukcjaZlecenie zlecenieProdukcyjne;
        private tblProdukcjaZlecenieCiecia zlecenieCiecia;

        public GPRuchTowar_PW_ZaawansowanieHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        #region ZlecenieTowar


        #region Zaawansowanie dla ZlecenieTowar
        public async Task<decimal> PobierzZaawansowanie_ProdukcjaZlecenieTowar(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (zlecenieTowar is null || zlecenieTowar.Ilosc_m2 == 0) return 0;

            var rolkiPW = await PobierzWyprodykowaneRolkiDlaDanegoZlecenia(zlecenieTowar);
            var sumaPW = rolkiPW.Sum(s => s.Ilosc_m2);
            var zaawansowanie = sumaPW / zlecenieTowar.Ilosc_m2;
            
            return zaawansowanie;
        }
        #endregion

        //TODO dodac argument gniazda!!! zlecenie
        //ZelecenieProdukcyjne zaawansowanie jako stosunek rolek wyprodukowanych na obu gniadach do sumy ilosci dla obu gniazd ze zlecenia
        //ZlecenieCiecia zaawansowanie tylko na podstawie rolek z gniazda konfekcji
        private async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzWyprodykowaneRolkiDlaDanegoZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            rolkiDlaZlecenieTowar = await unitOfWork.tblProdukcjaRuchTowar
                                                .WhereAsync(t => t.IDProdukcjaZlecenieTowar == zlecenieTowar.IDProdukcjaZlecenieTowar);

            return rolkiDlaZlecenieTowar
                    .Where(r => r.IDRuchStatus == (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW)
                    .Where(r => r.IDGramatura == zlecenieTowar.IDTowarGeowlokninaParametryGramatura); //nie wlicza w zaawansowanie rolek przekwalifikowanych na inna gramature

        }
        #region Status dla ZlecenieTowar

        public async Task AktualizujStatusZleceniaTowar(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            var zlecenie = await unitOfWork.tblProdukcjaZlecenieTowar.GetByIdAsync(zlecenieTowar.IDProdukcjaZlecenieTowar);

            var status = await PobierzStatusZleceniaTowar(zlecenieTowar);

            if (zlecenie.IDProdukcjaZlecenieStatus != (int)status)
                await unitOfWork.SaveAsync();

        }

        public async Task<ProdukcjaZlecenieStatusEnum> PobierzStatusZleceniaTowar(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            var zaawansowanie = await PobierzZaawansowanie_ProdukcjaZlecenieTowar(zlecenieTowar);

            var status = PobierzStatusZZaawansowania(zaawansowanie);

            return status;
        }
        #endregion

        #region IloscPozostalaDlaZlecenieTowar
        public async Task<(decimal pozostalaIlosc_m2, int pozostalaIlosc_szt)> PobierzIloscPozostalaDlaTowaruZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (zlecenieTowar is null) throw new ArgumentNullException();

            var rolkiWyprodukowane = await PobierzWyprodykowaneRolkiDlaDanegoZlecenia(zlecenieTowar);

            var pozostalaIlosc_m2 = zlecenieTowar.Ilosc_m2 - rolkiWyprodukowane.Sum(s => s.Ilosc_m2);
            var pozostalaIlosc_szt = zlecenieTowar.Ilosc_szt - rolkiWyprodukowane.Count();
            return (pozostalaIlosc_m2: pozostalaIlosc_m2, pozostalaIlosc_szt: pozostalaIlosc_szt);
        }
        #endregion

        private ProdukcjaZlecenieStatusEnum PobierzStatusZZaawansowania(decimal zaawansowanie)
        {
            if (zaawansowanie == 0)
                return ProdukcjaZlecenieStatusEnum.Oczekuje;
            else if (zaawansowanie >= 1)
                return ProdukcjaZlecenieStatusEnum.Zakonczone;
            else
                return ProdukcjaZlecenieStatusEnum.WTrakcie;
        }

        #endregion

        #region Zaawansowanie i status dla CalegoZlecenia
        #region Zaawansowanie dla Zlecenia

        public async Task<decimal> PobierzZawansowanie_ProdukcjaZlecenie(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (zlecenieTowar is null) return 0;

            var towaryZeZlecenia = await PobierzTowaryDlaZlecenia(zlecenieTowar);
            if (towaryZeZlecenia is null) return 0;

            var pozycjeWyprodukowaneDlaZlecenia = await PobierzWyprodykowanePozycjeDlaTowarowZeZlecenia(towaryZeZlecenia);
            if (pozycjeWyprodukowaneDlaZlecenia.Count() == 0) return 0;

            var sumaDlaZlecenia_Ilosc_m2 = towaryZeZlecenia.Sum(s => s.Ilosc_m2);
            if (sumaDlaZlecenia_Ilosc_m2 == 0) return 0;

            var sumaWyprodukowanaDlaDanegoZlecenia_Ilosc_m2 = pozycjeWyprodukowaneDlaZlecenia.Sum(s => s.Ilosc_m2);
            if (sumaWyprodukowanaDlaDanegoZlecenia_Ilosc_m2 == 0) return 0;

            var zaawansowanie = sumaWyprodukowanaDlaDanegoZlecenia_Ilosc_m2 / sumaDlaZlecenia_Ilosc_m2;
            return zaawansowanie;
        }


        private async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzWyprodykowanePozycjeDlaTowarowZeZlecenia(IEnumerable<tblProdukcjaZlecenieTowar> towaryZeZlecenia)
        {
            return await unitOfWork.tblProdukcjaRuchTowar.WhereAsync(t => towaryZeZlecenia
                                                            .Any(tz => tz.IDProdukcjaZlecenieTowar == t.IDProdukcjaZlecenieTowar));
        }

        private async Task<IEnumerable<tblProdukcjaZlecenieTowar>> PobierzTowaryDlaZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (zlecenieTowar.IDProdukcjaZlecenie.HasValue
                && zlecenieTowar.IDProdukcjaZlecenie > 0)
            {
                return await unitOfWork.tblProdukcjaZlecenieTowar
                                            .WhereAsync(t => t.IDProdukcjaZlecenie == zlecenieTowar.IDProdukcjaZlecenie.Value);
            }
            else if (zlecenieTowar.IDProdukcjaZlecenieCiecia.HasValue
                && zlecenieTowar.IDProdukcjaZlecenieCiecia > 0)
            {
                return await unitOfWork.tblProdukcjaZlecenieTowar
                                            .WhereAsync(t => t.IDProdukcjaZlecenieCiecia == zlecenieTowar.IDProdukcjaZlecenieCiecia.Value);
            }
            else
                return null;
        }
        #endregion

        #region Status dla Zlecenia
        public async Task<ProdukcjaZlecenieStatusEnum> PobierzStatusZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            var zaawansowanie = await PobierzZawansowanie_ProdukcjaZlecenie(zlecenieTowar);
            var status = PobierzStatusZZaawansowania(zaawansowanie);

            return status;
        }

        public async Task AktualizujStatusZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            if (zlecenieTowar is null) return;

            var status = await PobierzStatusZlecenia(zlecenieTowar);

            if (zlecenieTowar.IDProdukcjaZlecenie.HasValue
             && zlecenieTowar.IDProdukcjaZlecenie > 0)
            {
                zlecenieProdukcyjne = await unitOfWork.tblProdukcjaZlecenie.GetByIdAsync(zlecenieTowar.IDProdukcjaZlecenie.Value);
                if (zlecenieProdukcyjne.IDProdukcjaZlecenieStatus != (int)status)
                {
                    zlecenieProdukcyjne.IDProdukcjaZlecenieStatus = (int)status;
                }
                else
                    return;
            }
            else if (zlecenieTowar.IDProdukcjaZlecenieCiecia.HasValue
                  && zlecenieTowar.IDProdukcjaZlecenieCiecia > 0)
            {
                zlecenieCiecia = await unitOfWork.tblProdukcjaZlecenieCiecia.GetByIdAsync(zlecenieTowar.IDProdukcjaZlecenieCiecia.Value);

                if (zlecenieCiecia.IDProdukcjaZlecenieStatus != (int)status)
                {
                    zlecenieCiecia.IDProdukcjaZlecenieStatus = (int)status;
                }
                else
                    return;
            }
            else
                return;

            await unitOfWork.SaveAsync();
        }
        #endregion

        
        

        /// <summary>
        /// Pobiera zlecenieTowar - roznicowane jest na gniazda - dla kazdego gniazda jest osobne zlecenieTowar 
        /// (z tym, ze dla lini wloknin oraz kalandra zlecenieTowar moze byc na jednym zlecenie produkcyjnym, ale zawsze dla osobnych gniazd)
        /// Nastepnie sumuje ilosc wyprodukowana dla wszystkich zlecenTowarow i dzieli przez sume ilosci zlecenTowarow
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> PobierzZawansowanie_ProdukcjaZlecenie(int IDProdukcjaZlecenieTowar)
        {

            var zlecenieTowar = await unitOfWork.tblProdukcjaZlecenieTowar.GetByIdAsync(IDProdukcjaZlecenieTowar);

            listaTowarowDlaZlecenia = await PobierzListeTowarowDlaZlecenia(zlecenieTowar);
            iloscPlanowanaDlaZlecenia = listaTowarowDlaZlecenia.Sum(s => s.Ilosc_m2);


            var iloscWyprodukowanaDlaCalegoZlecenia = await PobierzIloscWyprodukowanaDlaCalegoZleceniaAsync();

            if (iloscPlanowanaDlaZlecenia != 0)
                return iloscWyprodukowanaDlaCalegoZlecenia / iloscPlanowanaDlaZlecenia;

            return 0;
        }

        private async Task<decimal> PobierzIloscWyprodukowanaDlaCalegoZleceniaAsync()
        {
            decimal iloscWyprodukowanaDlaCalegoZlecenia = 0;

            foreach (var towar in listaTowarowDlaZlecenia)
            {
                iloscWyprodukowanaDlaCalegoZlecenia += await unitOfWork.tblProdukcjaRuchTowar
                                                                .SumAsync(t => t.IDProdukcjaZlecenieTowar == towar.IDProdukcjaZlecenieTowar
                                                                            && t.IDRuchStatus==(int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW,
                                                                          t => t.Ilosc_m2)
                                                                .ConfigureAwait(false);
            }

            return iloscWyprodukowanaDlaCalegoZlecenia;
        }

        private async Task<IEnumerable<tblProdukcjaZlecenieTowar>> PobierzListeTowarowDlaZlecenia(tblProdukcjaZlecenieTowar zlecenieTowar)
        {
            //W tabeli tblProdukcjaZlecenieTowar albo IDProdukcjaZlecenieProdukcyjne jest null albo IDProdukcjaZlecenieCiecia jest null
            //dla jednego rekordu nigdy obie wartosci nie wystepuja razem, zawsze jedna null
            if (zlecenieTowar.IDProdukcjaZlecenie != null)
            {
                return await unitOfWork.tblProdukcjaZlecenieTowar
                    .WhereAsync(t => t.IDProdukcjaZlecenie == zlecenieTowar.IDProdukcjaZlecenie);
            }
            else
            {
                return await unitOfWork.tblProdukcjaZlecenieTowar
                    .WhereAsync(t => t.IDProdukcjaZlecenieCiecia == zlecenieTowar.IDProdukcjaZlecenieCiecia);
            }
        }

        #endregion

        #region RolkaRW
        public decimal PobierzRozchodRolkiRw(tblProdukcjaRuchTowar rolkaRW, IEnumerable<tblProdukcjaRuchTowar> ListaPW)
        {
            if (rolkaRW is null) return 0;
            if (rolkaRW.Ilosc_m2 == 0) return 0;
            if (ListaPW is null) return 0;
            if (!ListaPW.Any()) return 0;

            var iloscWyprodukowanaDlaDanejRolkiRW = ListaPW.Where(s=>s.IDRolkaBazowa==rolkaRW.IDProdukcjaRuchTowar)
                                                           .Sum(s => s.Ilosc_m2);

            return iloscWyprodukowanaDlaDanejRolkiRW / rolkaRW.Ilosc_m2;
        }
        #endregion
    }
}
