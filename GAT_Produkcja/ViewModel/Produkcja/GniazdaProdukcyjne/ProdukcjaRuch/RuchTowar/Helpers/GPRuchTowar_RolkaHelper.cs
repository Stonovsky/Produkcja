using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Migrations;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Helpers

{
    /// <summary>
    /// Klasa Helper dla rolek RW oraz PW
    /// </summary>
    public class GPRuchTowar_RolkaHelper : IGPRuchTowar_RolkaHelper
    {
        private readonly IUnitOfWork unitOfWork;
        const decimal WSP_KORYGUJACY_LiniaKalandra = 0.035m;
        public GPRuchTowar_RolkaHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Pobiera ID rolki bazowej dla rolki RW
        /// </summary>
        /// <param name="rolkaRW"></param>
        /// <param name="gniazdoProdukcyjnePW"></param>
        /// <returns></returns>
        public virtual async Task<int?> PobierzIDRolkiBazowejAsync(tblProdukcjaRuchTowar rolkaRW, GniazdaProdukcyjneEnum gniazdoProdukcyjnePW)
        {
            tblProdukcjaGniazdoProdukcyjne gniazdoRW = await PobierzGniazdoProdukcyjne(rolkaRW).ConfigureAwait(false);

            if (gniazdoRW is null) return null;

            switch (gniazdoProdukcyjnePW)
            {
                case GniazdaProdukcyjneEnum.LiniaWloknin:
                    return null;

                case GniazdaProdukcyjneEnum.LiniaDoKalandowania:
                    if (gniazdoRW.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
                        return rolkaRW.IDProdukcjaRuchTowar;
                    else
                        return rolkaRW.IDRolkaBazowa;

                case GniazdaProdukcyjneEnum.LiniaDoKonfekcji:
                    if (gniazdoRW.IDProdukcjaGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania)
                        return rolkaRW.IDProdukcjaRuchTowar;
                    else
                        return rolkaRW.IDRolkaBazowa;

                default:
                    return null;
            }
        }

        private async Task<tblProdukcjaGniazdoProdukcyjne> PobierzGniazdoProdukcyjne(tblProdukcjaRuchTowar rolkaRW)
        {
            tblProdukcjaGniazdoProdukcyjne gniazdo = null;
            if (rolkaRW?.tblProdukcjaRuchNaglowek?.tblProdukcjaGniazdoProdukcyjne is null)
            {
                var naglowek = await unitOfWork.tblProdukcjaRuchNaglowek
                                                    .SingleOrDefaultAsync(n => n.IDProdukcjaRuchNaglowek == rolkaRW.IDProdukcjaRuchNaglowek)
                                                    .ConfigureAwait(false);
                gniazdo = naglowek?.tblProdukcjaGniazdoProdukcyjne;
            }
            else
            {
                gniazdo = rolkaRW.tblProdukcjaRuchNaglowek.tblProdukcjaGniazdoProdukcyjne;
            }

            return gniazdo;
        }

        /// <summary>
        /// Pobiera odpad rolki dla rolki RW
        /// </summary>
        /// <param name="idRolkaRW"> id rolki RW</param>
        /// <returns></returns>
        public async Task<decimal> PobierzOdpadZRolkiRwAsync(int idRolkaRW)
        {
            var rolkaRW = await unitOfWork.tblProdukcjaRuchTowar.GetByIdAsync(idRolkaRW);
            if (rolkaRW is null) return 0;

            var listaPWDlaDanejRolkiRW = await unitOfWork.tblProdukcjaRuchTowar.WhereAsync(r => r.IDRolkaBazowa == idRolkaRW);

            if (!listaPWDlaDanejRolkiRW.Any())
                throw new ArgumentException("Dla wybranej rolki RW brak rolek do rozliczenia na PW." +
                                                      "\nRozliczenie (waga odpadu) nie będzie kontynuowane.");


            var wagaPW = listaPWDlaDanejRolkiRW.Sum(s => s.Waga_kg);

            return rolkaRW.Waga_kg - wagaPW;
        }

        public async Task<decimal> PobierzOdpadZRolkiRwAsync(int idRolkaRW, tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne)
        {
            var rolkaRW = await unitOfWork.tblProdukcjaRuchTowar.GetByIdAsync(idRolkaRW);
            if (rolkaRW is null) return 0;

            var listaPWDlaDanejRolkiRW = await unitOfWork.tblProdukcjaRuchTowar.WhereAsync(r => r.IDRolkaBazowa == idRolkaRW
                                                                                             && r.IDProdukcjaGniazdoProdukcyjne == gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);

            if (!listaPWDlaDanejRolkiRW.Any())
                throw new ArgumentException("Dla wybranej rolki RW brak rolek do rozliczenia na PW." +
                                                      "\nRozliczenie (waga odpadu) nie będzie kontynuowane.");

            decimal odpad = ObliczOdpad(gniazdoProdukcyjne, rolkaRW, listaPWDlaDanejRolkiRW);

            return odpad;
        }

        private decimal ObliczOdpad(tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne,
                                     tblProdukcjaRuchTowar rolkaRW,
                                     IEnumerable<tblProdukcjaRuchTowar> listaPWDlaDanejRolkiRW)
        {
            var wagaPW = listaPWDlaDanejRolkiRW.Sum(s => s.Waga_kg);

            var odpad = rolkaRW.Waga_kg * (1 - gniazdoProdukcyjne.WspZmniejszeniaMasy) - wagaPW;
            odpad = odpad < 0 ? 0 : odpad;

            return odpad;
        }


        #region PobierzKolejnyNrRolki


        /// <summary>
        /// Pobiera kolejny numer rolki dla danego gniazda produkcyjnego
        /// </summary>
        /// <param name="idGniazdoProdukcyjne"></param>
        /// <returns><see cref="int"/></returns>
        public async Task<int> PobierzKolejnyNrRolkiAsync(int idGniazdoProdukcyjne)
        {
            if (idGniazdoProdukcyjne == 0) return 0;

            DateTime miesiac = DateTime.Now.Date.AddMonths(-1);
            DateTime tydzien = DateTime.Now.Date.AddDays(-7);

            var nrRolki = await unitOfWork.tblProdukcjaRuchTowar
                    .GetNewNumberAsync(t => t.tblProdukcjaRuchNaglowek.IDProdukcjaGniazdoProdukcyjne == idGniazdoProdukcyjne
                                         && t.DataDodania >= tydzien,
                                       t => t.NrRolki);
            if (nrRolki > 0) return nrRolki;


            return await unitOfWork.tblProdukcjaRuchTowar
                    .GetNewNumberAsync(t => t.tblProdukcjaRuchNaglowek.IDProdukcjaGniazdoProdukcyjne == idGniazdoProdukcyjne
                                         && t.DataDodania >= miesiac,
                                       t => t.NrRolki);
        }

        /// <summary>
        /// Pobiera nast nr rolki
        /// </summary>
        /// <param name="gniazdoProdukcyjne"></param>
        /// <returns></returns>
        public async Task<int> PobierzKolejnyNrRolkiAsync(tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne)
        {
            if (gniazdoProdukcyjne is null) return 0;
            return await PobierzKolejnyNrRolkiAsync(gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);
        }

        /// <summary>
        /// Pobiera kolejny nr rolki wraz ze sprawdzeniem numeru w bazie
        /// </summary>
        /// <param name="gniazdoProdukcyjne"></param>
        /// <param name="listaTowarow"></param>
        /// <returns></returns>
        public async Task<int> PobierzKolejnyNrRolkiAsync(tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne,
                                                          IEnumerable<tblProdukcjaRuchTowar> listaTowarow)
        {
            int iloscElementowNieDodanychDoBazy = 0;
            if (listaTowarow != null)
                iloscElementowNieDodanychDoBazy = listaTowarow.Where(t => t.IDProdukcjaRuchTowar == 0).ToList().Count();

            var nrRolkiZBazy = await PobierzKolejnyNrRolkiAsync(gniazdoProdukcyjne);

            return nrRolkiZBazy + iloscElementowNieDodanychDoBazy;// +1;
        }


        #endregion

        /// <summary>
        /// Pobiera pelny nr rolki uwzgl rolke bazowa
        /// </summary>
        /// <param name="gniazdoProdukcyjne"></param>
        /// <param name="ruchTowar"></param>
        /// <param name="listaTowarow"></param>
        /// <returns></returns>
        public async Task<string> PobierzKolejnyPelnyNrRolkiAsync(tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne,
                                                                  tblProdukcjaRuchTowar ruchTowar,
                                                                  IEnumerable<tblProdukcjaRuchTowar> listaTowarow)
        {
            if (gniazdoProdukcyjne is null) return null;

            var idRolkiBazowej = await PobierzIDRolkiBazowejAsync(ruchTowar, (GniazdaProdukcyjneEnum)gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne);

            tblProdukcjaRuchTowar rolkaBazowa = null;
            if (idRolkiBazowej.HasValue && idRolkiBazowej != 0)
                rolkaBazowa = await unitOfWork.tblProdukcjaRuchTowar.GetByIdAsync(idRolkiBazowej.Value);

            var kolejnyNrRolki = await PobierzKolejnyNrRolkiAsync(gniazdoProdukcyjne, listaTowarow);

            return GenerujPelnyNumer(gniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne, rolkaBazowa, kolejnyNrRolki);


        }

        public async Task<string> PobierzKolejnyPelnyNrRolkiAsync(tblProdukcjaRuchTowar ruchTowar)
        {
            if (ruchTowar is null) return null;
            if (ruchTowar.IDProdukcjaGniazdoProdukcyjne is null || ruchTowar.IDProdukcjaGniazdoProdukcyjne == 0) return null;

            var idRolkiBazowej = ruchTowar.IDRolkaBazowa;

            tblProdukcjaRuchTowar rolkaBazowa = null;
            if (idRolkiBazowej.HasValue && idRolkiBazowej != 0)
                rolkaBazowa = await unitOfWork.tblProdukcjaRuchTowar.GetByIdAsync(idRolkiBazowej.Value);

            var kolejnyNrRolki = await PobierzKolejnyNrRolkiAsync(ruchTowar.IDProdukcjaGniazdoProdukcyjne.Value);

            return GenerujPelnyNumer(ruchTowar.IDProdukcjaGniazdoProdukcyjne.Value, rolkaBazowa, kolejnyNrRolki);
        }

        /// <summary>
        /// Generuje pelny nr rolki uwzgl gniazda oraz nr rolki bazowej
        /// </summary>
        /// <param name="gniazdoProdukcyjne"></param>
        /// <param name="rolkaBazowa"></param>
        /// <param name="kolejnyNrRolki"></param>
        /// <returns></returns>
        private string GenerujPelnyNumer(int idGniazdoProdukcyjne, tblProdukcjaRuchTowar rolkaBazowa, int kolejnyNrRolki)
        {
            if (rolkaBazowa is null)
                return kolejnyNrRolki.ToString();

            if (idGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaWloknin)
                return $"W{kolejnyNrRolki}";

            if (idGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania)
                return $"{rolkaBazowa.NrRolkiPelny}_K{kolejnyNrRolki}";

            if (idGniazdoProdukcyjne == (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji)
                return $"{rolkaBazowa.NrRolkiPelny}_KO{kolejnyNrRolki}";

            return null;
        }

        /// <summary>
        /// Pobiera koszt mieszanki za kg, i przypisuje do towaru na PW. Metoda do stosowania dla encji zapisanych w bazie! 
        /// </summary>
        /// <param name="rolka">rolka RW lub PW (w przypadku linii wloknin brak RW)</param>
        /// <param name="gniazdaProdukcyjneEnum"></param>
        /// <returns></returns>
        public async Task<decimal> PobierzKosztRolki(tblProdukcjaRuchTowar rolka, GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {

            if (rolka is null) return 0;
            if (gniazdaProdukcyjneEnum == 0) return 0;

            rolka = await unitOfWork.tblProdukcjaRuchTowar.GetByIdAsync(rolka.IDProdukcjaRuchTowar);

            if (rolka is null) return 0;

            if (gniazdaProdukcyjneEnum == GniazdaProdukcyjneEnum.LiniaWloknin)
            {
                return PobierzCeneZeZleceniaProdukcyjnego(rolka);
            }
            else
            {
                return await PobierzCeneDlaRolkiBazowej(rolka, gniazdaProdukcyjneEnum);
            }
        }

        /// <summary>
        /// Pobiera cene z rolki bazowej 
        /// </summary>
        /// <param name="rolka">rolka pobrana z bazy z child obiektami</param>
        /// <param name="gniazdaProdukcyjneEnum"></param>
        /// <returns></returns>
        private async Task<decimal> PobierzCeneDlaRolkiBazowej(tblProdukcjaRuchTowar rolka, GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {
            var idRolkiBazowej = await PobierzIDRolkiBazowejAsync(rolka, gniazdaProdukcyjneEnum);

            if (idRolkiBazowej.HasValue)
            {
                var rolkaBazowa = await unitOfWork.tblProdukcjaRuchTowar.GetByIdAsync(idRolkiBazowej.Value);
                return rolkaBazowa.Cena_kg;
            }
            else
            {
                return PobierzCeneZeZleceniaProdukcyjnego(rolka);
            }
        }

        /// <summary>
        /// Pobiera cene ze zlecenia produkcyjnego dla rolki pobranej z bazy ze wszystkimi child obiektami
        /// </summary>
        /// <param name="rolkaRW">rolka pobrana z bazy z child obiektami</param>
        /// <returns></returns>
        public decimal PobierzCeneZeZleceniaProdukcyjnego(tblProdukcjaRuchTowar rolkaRW)
        {
            if (rolkaRW.tblProdukcjaRuchNaglowek is null) throw new ArgumentException($"Brak rolki jako argumentu funkcji");
            if (rolkaRW.tblProdukcjaRuchNaglowek.tblProdukcjaZlcecenieProdukcyjne is null) throw new ArgumentException($"Brak rolki jako argumentu funkcji");

            return rolkaRW.tblProdukcjaRuchNaglowek.tblProdukcjaZlcecenieProdukcyjne.CenaMieszanki_zl;
        }

    }
}
