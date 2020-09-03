using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class Produkcja_tblProdukcjaRuchTowarAdapter : tblProdukcjaRuchTowar, ItblProdukcjaRuchTowar
    {
        private readonly Produkcja pozycjaMsAccess;
        #region CTOR
        public Produkcja_tblProdukcjaRuchTowarAdapter(Produkcja produkcja)
        {
            this.pozycjaMsAccess = produkcja;
        }
        #endregion


        public override int? IDProdukcjaZlecenieProdukcyjne => pozycjaMsAccess.ZlecenieID;
        public override string ZlecenieNazwa => pozycjaMsAccess.Zlecenie;
        public override int IDRuchStatus { get => (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW; }
        public override int IDProdukcjaRozliczenieStatus => (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
        public override int? IDProdukcjaRuchTowarStatus { get => null; }
        public override int? IDProdukcjaGniazdoProdukcyjne => (int)GniazdaProdukcyjneEnum.LiniaWloknin;
        public override string KodKreskowy => pozycjaMsAccess.NrSztuki;
        public override int NrRolki => default; //TODO sprawdzic czy jest sam nr
        public override string NrRolkiPelny => pozycjaMsAccess.NrSztuki;
        public override int? IDRolkaBazowa => default;
        public override string NrRolkiBazowej => pozycjaMsAccess.NrSztuki;
        public override string KodKreskowyRolkiBazowej => pozycjaMsAccess.NrSztuki;
        public override string TowarNazwaMsAccess => pozycjaMsAccess.Artykul;
        public override string TowarNazwaSubiekt => ModelHelper.GenerujNazweTowaru(this, "WL");
        public override string TowarSymbolSubiekt => ModelHelper.GenerujSymbolTowaru(this, "WL");
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
        public override tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne => default;
        public override tblProdukcjaRozliczenieStatus tblProdukcjaRozliczenieStatus => default;
        public override tblProdukcjaRuchNaglowek tblProdukcjaRuchNaglowek => default;
        public override tblProdukcjaRuchTowarStatus tblProdukcjaRuchTowarStatus => default;
        public override tblProdukcjaRuchTowar tblProdukcjaRuchTowarWyjsciowy => default;
        public override tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne => default;
        public override tblProdukcjaZlecenieTowar tblProdukcjaZlecenieTowar => default;
        public override tblRuchStatus tblRuchStatus => default;
        public override tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura => default;
        public override tblTowarGeowlokninaParametrySurowiec tblTowarGeowlokninaParametrySurowiec => default;


        public tblProdukcjaRuchTowar Generuj()
        {
            return new tblProdukcjaRuchTowar
            {
                IDProdukcjaZlecenieProdukcyjne = pozycjaMsAccess.ZlecenieID,
                ZlecenieNazwa = pozycjaMsAccess.Zlecenie,
                IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW,
                IDProdukcjaRozliczenieStatus = (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono,
                IDProdukcjaRuchTowarStatus = null,
                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin,
                KodKreskowy = pozycjaMsAccess.NrSztuki,
                NrRolki = default, //TODO sprawdzic czy jest sam nr
                NrRolkiPelny = pozycjaMsAccess.NrSztuki,
                IDRolkaBazowa = default,
                NrRolkiBazowej = pozycjaMsAccess.NrSztuki,
                KodKreskowyRolkiBazowej = pozycjaMsAccess.NrSztuki,
                TowarNazwaMsAccess = pozycjaMsAccess.Artykul,
                TowarNazwaSubiekt = ModelHelper.GenerujNazweTowaru(this, "WL"),
                TowarSymbolSubiekt = ModelHelper.GenerujSymbolTowaru(this, "WL"),
                IDGramatura = ModelHelper.GenerujGramaturaId(pozycjaMsAccess.Artykul),
                Gramatura = ModelHelper.GenerujGramature(pozycjaMsAccess.Artykul),
                IDTowarGeowlokninaParametrySurowiec = ModelHelper.GenerujSurowiecId(pozycjaMsAccess.Artykul),
                SurowiecSkrot = ModelHelper.GenerujSurowiecSkrot(pozycjaMsAccess.Artykul),
                Szerokosc_m = pozycjaMsAccess.Szerokosc,
                Dlugosc_m = pozycjaMsAccess.Dlugosc,
                Waga_kg = pozycjaMsAccess.Waga,
                Ilosc_m2 = pozycjaMsAccess.Szerokosc * pozycjaMsAccess.Dlugosc,
                WagaOdpad_kg = pozycjaMsAccess.WagaOdpadu,
                CzyKalandrowana = false,
                CzyParametryZgodne = false,
                NrZlecenia = pozycjaMsAccess.ZlecenieID,
                DataDodania = pozycjaMsAccess.Data.AddTicks(pozycjaMsAccess.Godzina.TimeOfDay.Ticks),
                KierunekPrzychodu = "Linia",

            };
        }
    }
}
