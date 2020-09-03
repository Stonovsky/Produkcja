using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.WeryfikacjaWynikowBadan
{
    public class WeryfikacjaWynikowBadan : IWeryfikacjaWynikowBadan
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Properties

        public List<tblWynikiBadanGeowloknin> ListaBadan { get; set; }
        public List<tblTowarGeowlokninaParametry> ListaParametrowWymaganych { get; set; }

        #endregion

        #region CTOR
        public WeryfikacjaWynikowBadan(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Commands

        public async Task SprawdzCzyWynikiBadanWTolerancjach()
        {
            ListaBadan = await _unitOfWork.tblWynikiBadanGeowloknin.GetAllAsync() as List<tblWynikiBadanGeowloknin>;
            ListaParametrowWymaganych = await _unitOfWork.tblTowarGeowlokninaParametry.GetAllAsync() as List<tblTowarGeowlokninaParametry>;

            foreach (var badanie in ListaBadan)
            {
                tblTowar towar = await ZnajdzTowar(badanie);

                if (string.IsNullOrWhiteSpace(towar.Nazwa))
                { 
                    badanie.UwagiDotyczaceWyniku = "Nie odnaleziono danego towaru w bazie danych";
                    CzyBadanieZgodne(badanie);
                }
                else
                {
                    var parametryWymagane = ListaParametrowWymaganych.SingleOrDefault(p => p.IDTowar == towar.IDTowar);
                    SprawdzZgodnoscBadaniaZWymaganymiParametrami(badanie, parametryWymagane);
                    CzyBadanieZgodne(badanie);
                }

                await _unitOfWork.SaveAsync();
            }
        }
        public async Task SprawdzCzyWynikiBadanWTolerancjach(tblWynikiBadanGeowloknin badanie)
        {
            if (badanie == null) return;

            tblTowar towar = await ZnajdzTowar(badanie);

            if (string.IsNullOrWhiteSpace(towar.Nazwa))
            {
                badanie.UwagiDotyczaceWyniku = "Nie odnaleziono danego towaru w bazie danych";
                CzyBadanieZgodne(badanie);
            }
            else
            {
                ListaParametrowWymaganych = await _unitOfWork.tblTowarGeowlokninaParametry.GetAllAsync() as List<tblTowarGeowlokninaParametry>;
                
                var parametryWymagane = ListaParametrowWymaganych.SingleOrDefault(p => p.IDTowar == towar.IDTowar);
                
                if (parametryWymagane==null)
                {
                    badanie.CzyBadanieZgodne = false;
                    badanie.UwagiDotyczaceWyniku = "Braka parametrów wymaganych dla danego towaru";
                }
                else
                {
                    SprawdzZgodnoscBadaniaZWymaganymiParametrami(badanie, parametryWymagane);
                    CzyBadanieZgodne(badanie);
                }
            }
            await _unitOfWork.SaveAsync();
        }

        private void SprawdzZgodnoscBadaniaZWymaganymiParametrami(tblWynikiBadanGeowloknin przebadanyTowar, 
                                                                  tblTowarGeowlokninaParametry parametryWymagane)
        {
            if (przebadanyTowar==null || parametryWymagane==null)
                return;

            przebadanyTowar.UwagiDotyczaceWyniku = string.Empty;

            if (przebadanyTowar.GramaturaSrednia > parametryWymagane.MasaPowierzchniowa_Maksimum 
                || przebadanyTowar.GramaturaSrednia < parametryWymagane.MasaPowierzchniowa_Minimum)
            {
                przebadanyTowar.UwagiDotyczaceWyniku += ZgodnoscBadaniaUwaga(przebadanyTowar.GramaturaSrednia,
                                                                            parametryWymagane.MasaPowierzchniowa_Maksimum,
                                                                            parametryWymagane.MasaPowierzchniowa_Minimum,
                                                                            "Gramatura średnia");
            }

            if (przebadanyTowar.KierunekBadania.ToLower().Contains("wzdłuż"))
            {
                if (przebadanyTowar.WydluzenieSrednie > parametryWymagane.WydluzeniePrzyZerwaniu_MD_Maksimum 
                    || przebadanyTowar.WydluzenieSrednie < parametryWymagane.WydluzeniePrzyZerwaniu_MD_Minimum)
                {
                    przebadanyTowar.UwagiDotyczaceWyniku += ZgodnoscBadaniaUwaga(przebadanyTowar.WydluzenieSrednie,
                                                                                    parametryWymagane.WydluzeniePrzyZerwaniu_MD_Maksimum,
                                                                                    parametryWymagane.WydluzeniePrzyZerwaniu_MD_Minimum,
                                                                                    "Wydłużenie średnie");
                }

                if (przebadanyTowar.WytrzymaloscSrednia > parametryWymagane.WytrzymaloscNaRozciaganie_MD 
                    || przebadanyTowar.WytrzymaloscSrednia < parametryWymagane.WytrzymaloscNaRozciaganie_MD_Minimum)
                {
                    przebadanyTowar.UwagiDotyczaceWyniku += ZgodnoscBadaniaUwaga(przebadanyTowar.WytrzymaloscSrednia,
                                        parametryWymagane.WytrzymaloscNaRozciaganie_MD,
                                        parametryWymagane.WytrzymaloscNaRozciaganie_MD_Minimum,
                                        "Wytrzymałość średnia");
                }
            }

            if (przebadanyTowar.KierunekBadania.ToLower().Contains("w poprzek"))
            {
                if (przebadanyTowar.WydluzenieSrednie > parametryWymagane.WydluzeniePrzyZerwaniu_CMD_Maksimum 
                    || przebadanyTowar.WydluzenieSrednie < parametryWymagane.WydluzeniePrzyZerwaniu_CMD_Minimum)
                {
                    przebadanyTowar.UwagiDotyczaceWyniku += ZgodnoscBadaniaUwaga(przebadanyTowar.WydluzenieSrednie,
                        parametryWymagane.WydluzeniePrzyZerwaniu_CMD_Maksimum,
                        parametryWymagane.WydluzeniePrzyZerwaniu_CMD_Minimum,
                                        "Wydłużenie średnie");
                }

                if (przebadanyTowar.WytrzymaloscSrednia > parametryWymagane.WytrzymaloscNaRozciaganie_CMD 
                    || przebadanyTowar.WytrzymaloscSrednia < parametryWymagane.WytrzymaloscNaRozciaganie_CMD_Minimum)
                {
                    przebadanyTowar.UwagiDotyczaceWyniku += ZgodnoscBadaniaUwaga(przebadanyTowar.WytrzymaloscSrednia,
                                        parametryWymagane.WytrzymaloscNaRozciaganie_CMD,
                                        parametryWymagane.WytrzymaloscNaRozciaganie_CMD_Minimum,
                                        "Wytrzymałość średnia");
                }
            }
        }

        private void CzyBadanieZgodne(tblWynikiBadanGeowloknin przebadanyTowar)
        {
            if (przebadanyTowar == null)
            {
                przebadanyTowar.CzyBadanieZgodne = false;
            }

            if (string.IsNullOrWhiteSpace(przebadanyTowar.UwagiDotyczaceWyniku))
            {
                przebadanyTowar.UwagiDotyczaceWyniku = "OK";
                przebadanyTowar.CzyBadanieZgodne = true;
            }
            else
            {
                przebadanyTowar.CzyBadanieZgodne = false;
            }
        }

        private async Task<tblTowar> ZnajdzTowar(tblWynikiBadanGeowloknin badanie)
        {
            var surowiec = string.Empty;

            if (badanie.Surowiec.ToLower().Contains("pp"))
                surowiec = "altex at pp";
            else if (badanie.Surowiec.ToLower().Contains("pes"))
                surowiec = "altex at pes";

            var towar = await _unitOfWork.tblTowar.SingleOrDefaultAsync(t => t.Nazwa.ToLower().Contains(surowiec) &&
                                                                             t.Nazwa.ToLower().Contains(badanie.Gramatura));
            return towar;
        }

        private string ZgodnoscBadaniaUwaga(decimal? parametrSprawdzany, 
                                            decimal parametrMaksimum, 
                                            decimal parametrMinimum, 
                                            string nazwaParametru)
        {
            if (parametrSprawdzany < parametrMinimum)
            {
                return $"{nazwaParametru} < Minimum => {parametrSprawdzany - parametrMinimum}\n";
            }
            else if (parametrSprawdzany > parametrMaksimum)
            {
                return $"{nazwaParametru} > Maksimum => {parametrSprawdzany - parametrMaksimum}\n";
            }
            else
            {
                return string.Empty;
            }
        }


        #endregion
    }
}
