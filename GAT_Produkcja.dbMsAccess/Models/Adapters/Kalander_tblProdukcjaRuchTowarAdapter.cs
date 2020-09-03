using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class Kalander_tblProdukcjaRuchTowarAdapter : tblProdukcjaRuchTowar
    {
        private Kalander pozycjaMsAccess;

        #region CTOR
        public Kalander_tblProdukcjaRuchTowarAdapter(Kalander kalander)
        {
            pozycjaMsAccess = kalander;
        }
        #endregion

        public override int? IDProdukcjaZlecenieProdukcyjne => pozycjaMsAccess.ZlecenieID;
        public override string ZlecenieNazwa => pozycjaMsAccess.Zlecenie;
        public override int IDRuchStatus => (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW; 
        public override int IDProdukcjaRozliczenieStatus => (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
        public override int? IDProdukcjaRuchTowarStatus  => null; 
        public override int? IDProdukcjaGniazdoProdukcyjne => (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania;
        public override string KodKreskowy => pozycjaMsAccess.NumerMG;
        public override int NrRolki => int.MinValue;
        public override string NrRolkiPelny => pozycjaMsAccess.NumerMG;
        public override int? IDRolkaBazowa => default;
        public override string NrRolkiBazowej => pozycjaMsAccess.NrSztuki;
        public override string KodKreskowyRolkiBazowej => pozycjaMsAccess.NrSztuki;
        public override string TowarNazwaMsAccess => pozycjaMsAccess.Artykul;
        public override string TowarNazwaSubiekt => ModelHelper.GenerujNazweTowaru(this, "KL");
        public override string TowarSymbolSubiekt => ModelHelper.GenerujSymbolTowaru(this, "KL");
        public override int IDGramatura => ModelHelper.GenerujGramaturaId(pozycjaMsAccess.Artykul);
        public override int Gramatura => ModelHelper.GenerujGramature(pozycjaMsAccess.Artykul);
        public override int IDTowarGeowlokninaParametrySurowiec => ModelHelper.GenerujSurowiecId(pozycjaMsAccess.Artykul);
        public override string SurowiecSkrot => ModelHelper.GenerujSurowiecSkrot(pozycjaMsAccess.Artykul);
        public override decimal Szerokosc_m => pozycjaMsAccess.Szerokosc;
        public override decimal Dlugosc_m => pozycjaMsAccess.Dlugosc;
        public override decimal Waga_kg => pozycjaMsAccess.Waga;
        public override decimal Ilosc_m2 => pozycjaMsAccess.Szerokosc * pozycjaMsAccess.Dlugosc;
        public override decimal WagaOdpad_kg => pozycjaMsAccess.WagaOdpadu;
        public override decimal Cena_kg => default;
        public override decimal Cena_m2 => default;
        public override decimal Wartosc => default;
        public override bool CzyKalandrowana => false;
        public override bool CzyParametryZgodne => false;
        public override int NrZlecenia => pozycjaMsAccess.ZlecenieID;
        public override DateTime DataDodania => pozycjaMsAccess.Data.AddTicks(pozycjaMsAccess.Godzina.TimeOfDay.Ticks);
        public override string KierunekPrzychodu => "Linia";
        public override string NazwaRolkiBazowej => default;
        public override string SymbolRolkiBazowej => default;
        public override int? IDMsAccess => pozycjaMsAccess.Id;
        public override string NrDokumentu => default;
        public override int? IDProdukcjaRuchNaglowek => default;
        public override int IDProdukcjaRuchTowar => default;
        public override int? IDProdukcjaRuchTowarWyjsciowy => default;
        public override int? IDProdukcjaZlecenieTowar => default;
        public override int? IDTowar => default;
        public override string Uwagi => default;
        public override string UwagiParametry => default;
        public override int LP => default;

        public override int? IDZleceniePodstawowe => pozycjaMsAccess.ZlecenieID;
        public override int? NrZleceniaPodstawowego => pozycjaMsAccess.ZlecenieID;
    }
}
