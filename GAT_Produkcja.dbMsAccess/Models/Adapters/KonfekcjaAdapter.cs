using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.dbMsAccess.Helpers;
using GAT_Produkcja.dbMsAccess.Models.Adapters.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class KonfekcjaAdapter : MsAccessAdapterBase<Konfekcja>, IProdukcjaRuchTowar
    {
        private readonly Konfekcja pozycjaMsAccess;
        private readonly IProdukcjaZlecenieIdHelper produkcjaZlecenieIdHelper;
        private int zlecenieId;
        private int nrPalety;

        public KonfekcjaAdapter(Konfekcja konfekcja)
        {
            this.pozycjaMsAccess = konfekcja;

            PobierzNrPalety();
        }

        private void PobierzNrPalety()
        {
            nrPalety = 0;

            int.TryParse(pozycjaMsAccess.NrWz, out nrPalety);
        }

        public int? IDProdukcjaZlecenieProdukcyjne => pozycjaMsAccess.ZlecenieID;
        public string ZlecenieNazwa => pozycjaMsAccess.Zlecenie;

        public int IDRuchStatus { get => (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW; }
        public int IDProdukcjaRozliczenieStatus
        {
            get
            {
                if (pozycjaMsAccess.CzyZaksiegowano)
                    return (int)ProdukcjaRozliczenieStatusEnum.Rozliczono;
                else
                    return (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono;
            }
        }
        public int? IDProdukcjaRuchTowarStatus { get => null; }
        public int? IDProdukcjaGniazdoProdukcyjne => (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji;

        public string KodKreskowy { get => pozycjaMsAccess.NumerMG; }
        public int NrRolki { get => int.MinValue; }
        public string NrRolkiPelny { get => pozycjaMsAccess.NumerMG; }
        public int? IDRolkaBazowa { get => null; }
        public string NrRolkiBazowej { get => pozycjaMsAccess.NrSztuki; }
        public string KodKreskowyRolkiBazowej { get => pozycjaMsAccess.NrSztuki; }

        public string TowarNazwaMsAccess => pozycjaMsAccess.Artykul;
        public string TowarNazwaSubiekt => GenerujNazweTowaru(this);
        public string TowarSymbolSubiekt => GenerujSymbolTowaru(this);

        public int IDGramatura { get => 0; }
        public int Gramatura => this.GenerujGramature(pozycjaMsAccess.Artykul);
        public int IDTowarGeowlokninaParametrySurowiec { get => 0; }
        public string SurowiecSkrot => this.GenerujSurowiecSkrot(pozycjaMsAccess.Artykul);
        public decimal Szerokosc_m { get => pozycjaMsAccess.Szerokosc; }
        public decimal Dlugosc_m { get => pozycjaMsAccess.Dlugosc; }
        public decimal Waga_kg { get => pozycjaMsAccess.Waga; }
        public decimal Ilosc_m2 { get => pozycjaMsAccess.Szerokosc * pozycjaMsAccess.Dlugosc; }
        public decimal WagaOdpad_kg { get => pozycjaMsAccess.WagaOdpadu; }
        public decimal Cena_kg { get => 0; }
        public decimal Cena_m2 { get => 0; }
        public decimal Wartosc { get => 0; }
        public bool CzyKalandrowana { get => true; }
        public bool CzyParametryZgodne { get => false; }
        public int NrZlecenia { get => pozycjaMsAccess.ZlecenieID; }
        public DateTime DataDodania { get => pozycjaMsAccess.Data.Add(pozycjaMsAccess.Godzina.TimeOfDay);}

        public string KierunekPrzychodu => pozycjaMsAccess.Przychody;
        public string NazwaRolkiBazowej => pozycjaMsAccess.NrSztuki;
        public string SymbolRolkiBazowej => pozycjaMsAccess.NrSztuki;

        public int? IDMsAccess => pozycjaMsAccess.Id;

        public string NrDokumentu => pozycjaMsAccess.NrWz;

        public int? IDZleceniePodstawowe => pozycjaMsAccess.ZlecenieID;
        public int? NrZleceniaPodstawowego => pozycjaMsAccess.ZlecenieID;

        public tblProdukcjaRuchTowar Generuj()
        {
            return new tblProdukcjaRuchTowar
            {
                IDProdukcjaZlecenieProdukcyjne = zlecenieId,
                ZlecenieNazwa = $"{pozycjaMsAccess.Zlecenie} - {pozycjaMsAccess.ZlecenieID}",
                IDRuchStatus = (int)StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW,
                IDProdukcjaRozliczenieStatus = ModelHelper.CzyRozliczono(pozycjaMsAccess.CzyZaksiegowano),
                IDProdukcjaRuchTowarStatus = null,
                IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji,
                IDProdukcjaRuchNaglowek = 11,
                IDProdukcjaZlecenieTowar = default,
                IDMsAccess = pozycjaMsAccess.Id,
                IDProdukcjaRuchTowar = default,
                IDProdukcjaRuchTowarWyjsciowy = default,
                IDTowar = default,
                KodKreskowy = pozycjaMsAccess.NumerMG,
                NrRolki = default, //TODO sprawdzic czy jest sam nr
                NrRolkiPelny = pozycjaMsAccess.NumerMG,
                TowarNazwaMsAccess = pozycjaMsAccess.Artykul,
                TowarNazwaSubiekt = ModelHelper.GenerujNazweTowaru(this, null),
                TowarSymbolSubiekt = ModelHelper.GenerujSymbolTowaru(this, null),
                IDGramatura = ModelHelper.GenerujGramaturaId(pozycjaMsAccess.Artykul),
                Gramatura = ModelHelper.GenerujGramature(pozycjaMsAccess.Artykul),
                IDTowarGeowlokninaParametrySurowiec = ModelHelper.GenerujSurowiecId(pozycjaMsAccess.Artykul),
                SurowiecSkrot = ModelHelper.GenerujSurowiecSkrot(pozycjaMsAccess.Artykul),
                Szerokosc_m = pozycjaMsAccess.Szerokosc,
                Dlugosc_m = pozycjaMsAccess.Dlugosc,
                Waga_kg = pozycjaMsAccess.Waga,
                Ilosc_m2 = pozycjaMsAccess.Szerokosc * pozycjaMsAccess.Dlugosc,
                WagaOdpad_kg = pozycjaMsAccess.WagaOdpadu,
                CzyKalandrowana = true,
                CzyParametryZgodne = false,
                NrZlecenia = pozycjaMsAccess.ZlecenieID,
                DataDodania = pozycjaMsAccess.Data.AddTicks(pozycjaMsAccess.Godzina.TimeOfDay.Ticks),
                KierunekPrzychodu = pozycjaMsAccess.Przychody,
                NrDokumentu=pozycjaMsAccess.NrWz,
                NazwaRolkiBazowej=pozycjaMsAccess.NrSztuki,
                NrRolkiBazowej = pozycjaMsAccess.NrSztuki,
                KodKreskowyRolkiBazowej = pozycjaMsAccess.NrSztuki,
                IDRolkaBazowa = default,
                NrPalety=nrPalety,
                IDZleceniePodstawowe = pozycjaMsAccess.ZlecenieID,
                NrZleceniaPodstawowego = pozycjaMsAccess.ZlecenieID,
            };
        }
    }
}
