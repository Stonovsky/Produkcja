using GAT_Produkcja.db;
using GAT_Produkcja.db.Dictionaries;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Helpers;
using GAT_Produkcja.dbMsAccess.Models.Adapters.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class ProdukcjaAdapter : MsAccessAdapterBase<Produkcja>, IProdukcjaRuchTowar, ItblProdukcjaRuchTowar
    {
        private readonly Produkcja pozycjaMsAccess;
        private readonly IProdukcjaZlecenieIdHelper produkcjaZlecenieIdHelper;
        private int zlecenieId;

        public ProdukcjaAdapter(Produkcja produkcja)
        {
            this.pozycjaMsAccess = produkcja;
        }

        public int? IDProdukcjaZlecenieProdukcyjne => zlecenieId;
        public string ZlecenieNazwa => pozycjaMsAccess.Zlecenie;
        public int IDRuchStatus { get => (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW; }
        public int IDProdukcjaRozliczenieStatus => (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
        public int? IDProdukcjaRuchTowarStatus { get => null; }
        public int? IDProdukcjaGniazdoProdukcyjne => (int)GniazdaProdukcyjneEnum.LiniaWloknin;
        public string KodKreskowy => pozycjaMsAccess.NrSztuki;
        public int NrRolki => int.MinValue;
        public string NrRolkiPelny => pozycjaMsAccess.NrSztuki;
        public int? IDRolkaBazowa => default;
        public string NrRolkiBazowej => pozycjaMsAccess.NrSztuki;
        public string KodKreskowyRolkiBazowej => pozycjaMsAccess.NrSztuki;

        public string TowarNazwaMsAccess => pozycjaMsAccess.Artykul;
        public string TowarNazwaSubiekt => GenerujNazweTowaru(this, "WL");
        public string TowarSymbolSubiekt => GenerujSymbolTowaru(this, "WL");

        public int IDGramatura => GenerujGramaturaId(pozycjaMsAccess.Artykul);
        public int Gramatura => GenerujGramature(pozycjaMsAccess.Artykul);
        public int IDTowarGeowlokninaParametrySurowiec => default;
        public string SurowiecSkrot => GenerujSurowiecSkrot(pozycjaMsAccess.Artykul);
        public decimal Szerokosc_m => pozycjaMsAccess.Szerokosc;
        public decimal Dlugosc_m => pozycjaMsAccess.Dlugosc;
        public decimal Waga_kg => pozycjaMsAccess.Waga;
        public decimal Ilosc_m2 => pozycjaMsAccess.Szerokosc * pozycjaMsAccess.Dlugosc;
        public decimal WagaOdpad_kg => pozycjaMsAccess.WagaOdpadu;
        public decimal Cena_kg => default;
        public decimal Cena_m2 => default;
        public decimal Wartosc => default;
        public bool CzyKalandrowana => false;
        public bool CzyParametryZgodne => false;
        public int NrZlecenia => pozycjaMsAccess.ZlecenieID;
        public DateTime DataDodania => pozycjaMsAccess.Data.AddTicks(pozycjaMsAccess.Godzina.TimeOfDay.Ticks);
        public string KierunekPrzychodu => "Linia";
        public string NazwaRolkiBazowej => default;
        public string SymbolRolkiBazowej => default;
        public int? IDMsAccess => pozycjaMsAccess.Id;
        public string NrDokumentu => default;
        public int? IDProdukcjaRuchNaglowek => default;
        public int IDProdukcjaRuchTowar => default;
        public int? IDProdukcjaRuchTowarWyjsciowy => default;
        public int? IDProdukcjaZlecenieTowar => default;
        public int? IDTowar => default;
        public string Uwagi => default;
        public string UwagiParametry => default;

        public int LP => default;
        public int? IDZleceniePodstawowe => pozycjaMsAccess.ZlecenieID;
        public int? NrZleceniaPodstawowego => pozycjaMsAccess.ZlecenieID;

        public tblProdukcjaRuchTowar Generuj()
        {
            return new tblProdukcjaRuchTowar
            {
                IDProdukcjaZlecenieProdukcyjne = default,
                ZlecenieNazwa =$"{pozycjaMsAccess.Zlecenie} - {pozycjaMsAccess.ZlecenieID}",
                IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW,
                IDProdukcjaRozliczenieStatus = (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono,
                IDProdukcjaRuchTowarStatus = null,
                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin,
                IDProdukcjaRuchNaglowek=9,
                IDProdukcjaZlecenieTowar=default,
                IDMsAccess=pozycjaMsAccess.Id,
                IDProdukcjaRuchTowar=default,
                IDProdukcjaRuchTowarWyjsciowy=default,
                IDTowar=default,
                KodKreskowy = pozycjaMsAccess.NrSztuki,
                NrRolki = default, //TODO sprawdzic czy jest sam nr
                NrRolkiPelny = pozycjaMsAccess.NrSztuki,
                IDRolkaBazowa = default,
                NrRolkiBazowej = pozycjaMsAccess.NrSztuki,
                KodKreskowyRolkiBazowej = pozycjaMsAccess.NrSztuki,
                TowarNazwaMsAccess = pozycjaMsAccess.Artykul,
                TowarNazwaSubiekt = ModelHelper.GenerujNazweTowaru(this),
                TowarSymbolSubiekt = ModelHelper.GenerujSymbolTowaru(this),
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
                IDZleceniePodstawowe= pozycjaMsAccess.ZlecenieID,
                NrZleceniaPodstawowego= pozycjaMsAccess.ZlecenieID,
            };
        }
    }
}