using System;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface ItblProdukcjaRuchTowar
    {
        decimal Cena_kg { get; }
        decimal Cena_m2 { get; }
        bool CzyKalandrowana { get; }
        bool CzyParametryZgodne { get; }
        DateTime DataDodania { get; }
        decimal Dlugosc_m { get; }
        int Gramatura { get; }
        int IDGramatura { get; }
        int? IDMsAccess { get; }
        int? IDProdukcjaGniazdoProdukcyjne { get; }
        int IDProdukcjaRozliczenieStatus { get; }
        int? IDProdukcjaRuchNaglowek { get; }
        int IDProdukcjaRuchTowar { get; }
        int? IDProdukcjaRuchTowarStatus { get; }
        int? IDProdukcjaRuchTowarWyjsciowy { get; }
        int? IDProdukcjaZlecenieProdukcyjne { get; }
        int? IDProdukcjaZlecenieTowar { get; }
        int? IDRolkaBazowa { get; }
        int IDRuchStatus { get; }
        int? IDTowar { get; }
        int IDTowarGeowlokninaParametrySurowiec { get; }
        decimal Ilosc_m2 { get; }
        string KierunekPrzychodu { get; }
        string KodKreskowy { get; }
        string KodKreskowyRolkiBazowej { get; }
        string NazwaRolkiBazowej { get; }
        string NrDokumentu { get; }
        int NrRolki { get; }
        string NrRolkiBazowej { get; }
        string NrRolkiPelny { get; }
        int NrZlecenia { get; }
        string SurowiecSkrot { get; }
        string SymbolRolkiBazowej { get; }
        decimal Szerokosc_m { get; }
        string TowarNazwaMsAccess { get; }
        string TowarNazwaSubiekt { get; }
        string TowarSymbolSubiekt { get; }
        string Uwagi { get; }
        string UwagiParametry { get; }
        decimal Waga_kg { get; }
        decimal WagaOdpad_kg { get; }
        decimal Wartosc { get; }
        string ZlecenieNazwa { get; }
        int LP { get; }
    }
}