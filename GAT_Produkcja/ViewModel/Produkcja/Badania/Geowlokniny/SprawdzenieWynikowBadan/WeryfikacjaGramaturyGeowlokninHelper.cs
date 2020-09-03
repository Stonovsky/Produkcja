using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.SprawdzenieWynikowBadan
{
    public class WeryfikacjaGramaturyGeowlokninHelper : IWeryfikacjaGramaturyGeowlokninHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<tblTowarGeowlokninaParametry> listaParametrowWymaganych;
        private IEnumerable<tblTowarGeowlokninaParametryGramatura> listaGramatur;
        private tblProdukcjaRuchTowar towar;

        #region CTOR
        public WeryfikacjaGramaturyGeowlokninHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            //Task.Run(() => LoadAsync());
        }
        #endregion

        #region PobranieDanychWstepnych
        public async Task LoadAsync()
        {
            listaParametrowWymaganych = await unitOfWork.tblTowarGeowlokninaParametry.WhereAsync(e => e.CzyBadanieAktualne == true);
            listaGramatur = await unitOfWork.tblTowarGeowlokninaParametryGramatura.GetAllAsync();
        }
        #endregion

        public bool CzyGramaturaZgodna(decimal gramaturaSrednia, int idGramatura)
        {
            var parametryWymagane = listaParametrowWymaganych.Where(e => e.IDTowarGeowlokninaParametryGramatura == idGramatura)
                                                             .FirstOrDefault();

            if (gramaturaSrednia >= parametryWymagane.MasaPowierzchniowa_Minimum
             && gramaturaSrednia <= parametryWymagane.MasaPowierzchniowa_Maksimum)
                return true;
            else
                return false;
        }

        public bool CzyGramaturaZgodna(tblProdukcjaRuchTowar towar)
        {
            this.towar = towar;

            if (towar.Ilosc_m2 == 0) return false;

            var gramaturaSrednia = towar.Waga_kg / towar.Ilosc_m2 * 1000;
            return CzyGramaturaZgodna(gramaturaSrednia, towar.IDGramatura);
        }


        public tblTowarGeowlokninaParametryGramatura PobierzWlasciwaGramature(decimal gramaturaSrednia, tblProdukcjaRuchTowar towar)
        {
            this.towar = towar;

            var idGramatura = PobierzWlasciwaGramatureId(gramaturaSrednia);

            return listaGramatur.Where(g => g.IDTowarGeowlokninaParametryGramatura == idGramatura)
                                .FirstOrDefault();
        }

        private int PobierzWlasciwaGramatureId(decimal gramaturaSrednia)
        {
            var towarParametry = listaParametrowWymaganych.Where(e => e.MasaPowierzchniowa_Minimum < gramaturaSrednia
                                                                   && e.MasaPowierzchniowa_Maksimum > gramaturaSrednia
                                                                   && e.IDTowarGeowlokninaParametrySurowiec==towar.IDTowarGeowlokninaParametrySurowiec)
                                                          .FirstOrDefault();
            // gdy towar miesci sie w zakresie wraz z tolerancjami
            if (towarParametry != null)
                return towarParametry.IDTowarGeowlokninaParametryGramatura.GetValueOrDefault();

            if (GramaturaNizszaNiz_90(gramaturaSrednia).HasValue) return GramaturaNizszaNiz_90(gramaturaSrednia).GetValueOrDefault();

            if (GramaturaWyzszaNiz_300(gramaturaSrednia).HasValue) return GramaturaWyzszaNiz_300(gramaturaSrednia).GetValueOrDefault();

            return GramaturaPomiedzyZakresami(gramaturaSrednia);
        }

        private int GramaturaPomiedzyZakresami(decimal gramaturaSrednia)
        {
            var listyGramaturDolna = listaParametrowWymaganych.Where(e => e.MasaPowierzchniowa_Minimum <= gramaturaSrednia
                                                                       && e.IDTowarGeowlokninaParametrySurowiec==towar.IDTowarGeowlokninaParametrySurowiec);
            listyGramaturDolna = listyGramaturDolna.OrderByDescending(o => o.MasaPowierzchniowa_Minimum);

            var parametryWybrane = listyGramaturDolna.FirstOrDefault();

            return parametryWybrane.IDTowarGeowlokninaParametryGramatura.GetValueOrDefault();
        }

        private int? GramaturaWyzszaNiz_300(decimal gramaturaSrednia)
        {
            var parmGramatura_300 = listaParametrowWymaganych.Where(e => e.IDTowarGeowlokninaParametryGramatura == (int)TowarGeowlokninaGramaturaEnum.Gramatura_300
                                                                      && e.IDTowarGeowlokninaParametrySurowiec == towar.IDTowarGeowlokninaParametrySurowiec)
                                                             .FirstOrDefault();

            if (gramaturaSrednia > parmGramatura_300.MasaPowierzchniowa_Maksimum)
                return parmGramatura_300.IDTowarGeowlokninaParametryGramatura.GetValueOrDefault();

            return null;
        }

        private int? GramaturaNizszaNiz_90(decimal gramaturaSrednia)
        {
            var parmGramatura_90 = listaParametrowWymaganych.Where(e => e.IDTowarGeowlokninaParametryGramatura == (int)TowarGeowlokninaGramaturaEnum.Gramatura_90
                                                                     && e.IDTowarGeowlokninaParametrySurowiec==towar.IDTowarGeowlokninaParametrySurowiec)
                                                            .FirstOrDefault();

            if (gramaturaSrednia < parmGramatura_90.MasaPowierzchniowa_Minimum)
                throw new GeowlokninaGramaturaException("Gramatura poniżej wartości dla oficjalnych parametrów deklarowanych");

            return null;
        }
    }
}
