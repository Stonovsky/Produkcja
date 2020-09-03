using System;
using System.Linq;
using System.Threading.Tasks;
using GAT_Produkcja.db.Repositories.Repositories;

namespace GAT_Produkcja.db.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ITblFirmaRepository tblFirma { get; }
        ITblJmRepository tblJm { get; }
        ITblKlasyfikacjaOgolnaRepository tblKlasyfikacjaOgolna { get; }
        ITblKlasyfikacjaSzczegolowaRepository tblKlasyfikacjaSzczegolowa { get; }
        ITblUrzadzeniaRepository tblUrzadzenia { get; }
        ITblKontrahentRepository tblKontrahent { get; }
        ITblPlikiRepository tblPliki { get; }
        ITblPracownikGATRepository tblPracownikGAT { get; }
        IVwZapotrzebowanieEwidencjaRepository vwZapotrzebowanieEwidencja { get; }
        ITblZapotrzebowanieStatusRepository tblZapotrzebowanieStatus { get; }
        ITblZapotrzebowaniePozycjeRepository tblZapotrzebowaniePozycje { get; }
        ITblZapotrzebowanieRepository tblZapotrzebowanie { get; }
        IVwFVKosztowezSubiektGTRepository vwFVKosztowezSubiektGT { get; }
        ITblWynikiBadanGeowlokninRepository tblWynikiBadanGeowloknin { get; }
        ITblWynikiBadanDlaProbekRepository tblWynikiBadanDlaProbek { get; }
        ITblVATRepository tblVAT { get; }

        #region Towar
        ITblTowarRepository tblTowar { get; }
        ITblTowarParametryRepository tblTowarParametry { get; }
        ITblTowarGrupaRepository tblTowarGrupa { get; }
        ITblTowarGeowlokninaParametryGramaturaRepository tblTowarGeowlokninaParametryGramatura { get; }
        ITblTowarGeowlokninaParametryRepository tblTowarGeowlokninaParametry { get; }
        ITblTowarGeowlokninaParametryRodzajRepository tblTowarGeowlokninaParametryRodzaj { get; }
        ITblTowarStatusRepository tblTowarStatus { get; }
        ITblTowarGeokomorkaParametryGeometryczneRepository tblTowarGeokomorkaParametryGeometryczne { get; }
        ITblTowarGeokomorkaParametryTypRepository tblTowarGeokomorkaParametryTyp { get; }
        ITblTowarGeokomorkaParametryZgrzewRepository tblTowarGeokomorkaParametryZgrzew { get; }
        ITblTowarGeokomorkaParametryRodzajRepository tblTowarGeokomorkaParametryRodzaj { get; }
        ITblTowarGeowlokninaParametrySurowiecRepository tblTowarGeowlokninaParametrySurowiec { get; }
        ITblCertyfikatCERepository tblCertyfikatCE { get; }

        ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository tblProdukcjaZlecenieProdukcyjne_Mieszanka { get; }


        #endregion

        #region Magazyn
        IVwStanTowaruRepository vwStanTowaru { get; }
        IVwRuchTowaruRepository vwRuchTowaru { get; }

        ITblMagazynRepository tblMagazyn { get; }
        ITblRuchNaglowekRepository tblRuchNaglowek { get; }
        ITblRuchStatusRepository tblRuchStatus { get; }
        ITblRuchTowarRepository tblRuchTowar { get; }
        ITblKodKreskowyTypRepository tblKodKreskowyTyp { get; }

        ITblDokumentTypRepository tblDokumentTyp { get; }
        ITblRuchTowarGeowlokninaParametryRepository tblRuchTowarGeowlokninaParametry { get; }
        #endregion

        #region Zlecenia
        #region ZamowienieOdKlienta
        ITblZamowieniaPrzesylkaKosztRepository tblZamowieniaPrzesylkaKoszt { get; }
        ITblZamowieniaTerminPlatnosciRepository tblZamowieniaTerminPlatnosci { get; }
        ITblZamowieniaWarunkiPlatnosciRepository tblZamowieniaWarunkiPlatnosci { get; }
        ITblZamowienieHandloweRepository tblZamowienieHandlowe { get; }
        ITblZamowienieHandlowePakowanieRepository tblZamowienieHandlowePakowanie { get; }
        ITblZamowienieHandlowePakowanieRodzajRepository tblZamowienieHandlowePakowanieRodzaj { get; }
        ITblZamowienieHandloweTowarGeokomorkaRepository tblZamowienieHandloweTowarGeokomorka { get; }
        ITblZamowienieHandloweTowarGeowlokninaRepository tblZamowienieHandloweTowarGeowloknina { get; }
        #endregion

        #region ZlecenieProdukcyjne
        ITblProdukcjaZlecenieRepository tblProdukcjaZlecenie { get; }
        ITblProdukcjaZlcecenieProdukcyjne_tblTowarParametryRepository tblProdukcjaZlcecenieProdukcyjne_tblTowarParametry { get; }
        ITblProdukcjaTulejeRepository tblProdukcjaTuleje { get; }
        //ITblProdukcjaPaletyRepository TblProdukcjaPalety { get; }
        ITblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowarRepository tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar { get; }
        #region GniazdaProdukcyjne
        ITblProdukcjaGniazdoProdukcyjneRepository tblProdukcjaGniazdoProdukcyjne { get; }
        ITblProdukcjaGniazdoWlokninaRepository tblProdukcjaGniazdoWloknina { get; }
        ITblProdukcjaGniazdoWlokninaBadaniaRepository tblProdukcjaGniazdoWlokninaBadania { get; }
        //ITblProdukcjaGniazdoKalanderWlokninaRepository tblProdukcjaGniazdoWlokninaNastawy { get; }
        ITblProdukcjaGniazdoKalanderNastawyRepository tblProdukcjaGniazdoKalanderNastawy { get; }
        ITblProdukcjaGniazdoKalanderRepository tblProdukcjaGniazdoKalander { get; }
        //ITblProdukcjaGniazdoKonfekcjaNastawyRepository tblProdukcjaGniazdoKonfekcjaNastawy { get; }
        //ITblProdukcjaGniazdoKonfekcjaRepository tblProdukcjaGniazdoKonfekcja { get; }
        ITblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjneRepository tblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjne { get; }
        #endregion

        #region Mieszanka
        ITblMieszankaRepository tblMieszanka { get; }
        ITblMieszankaSkladRepository tblMieszankaSklad { get; }
        #endregion

        #endregion

        #region ZlecenieCiecia
        ITblProdukcjaZlecenieCieciaRepository tblProdukcjaZlecenieCiecia { get; }
        ITblProdukcjaZlecenieTowarRepository tblProdukcjaZlecenieTowar { get; }
        #endregion

        ITblProdukcjaZlecenieStatusRepository tblProdukcjaZlecenieStatus { get; }

        #endregion

        #region GniazdaProdukcyjne
        ITblProdukcjaRuchTowarRepository tblProdukcjaRuchTowar { get; }
        ITblProdukcjaRuchTowarBadaniaRepository tblProdukcjaRuchTowarBadania { get; }
        ITblProdukcjaRuchNaglowekRepository tblProdukcjaRuchNaglowek { get; }
        #endregion

        #region ProdukcjaRozliczenie
        ITblProdukcjaRozliczenie_DaneRepository tblProdukcjaRozliczenie_Dane { get; }
        ITblProdukcjaRozliczenie_PWRepository tblProdukcjaRozliczenie_PW{ get;}
        ITblProdukcjaRozliczenie_RWRepository tblProdukcjaRozliczenie_RW{ get; }
        ITblProdukcjaRozliczenie_CenyTransferoweRepository tblProdukcjaRozliczenie_CenyTransferowe { get; }
        ITblProdukcjaRozliczenie_NaglowekRepository tblProdukcjaRozliczenie_Naglowek { get; }
        ITblProdukcjaRozliczenie_PWPodsumowanieRepository tblProdukcjaRozliczenie_PWPodsumowanie { get; }
        #endregion

        #region ProdukcjaGeokomorki
        ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository tblProdukcjaGeokomorkaPodsumowaniePrzerob { get; }
        #endregion

        #region Ceny
        IVwCenyMagazynoweAGGRepository vwCenyMagazynoweAGG { get; }
        #endregion

        #region Finanse
        ITblFinanseStanKontaRepository tblFinanseStanKonta { get; }
        #endregion

        #region Logger
        ITblLogRepository tblLog { get; }
        #endregion

        #region GTEX Subiekt
        IVwMagazynRuchGTXRepository vwMagazynRuchGTX { get; }
        IVwMagazynGTXRepository vwMagazynGTX { get; }
        IVwTowarGTXRepository vwTowarGTX { get; }
        IVwFinanseNalZobGTXRepository vwFinanseNalZobGTX { get; }
        IVwFinanseBankAGGRepository vwFinanseBankAGG { get; }
        #endregion

        #region AGG Subiekt
        IVwMagazynRuchAGGRepository vwMagazynRuchAGG { get;}
        IVwMagazynAGGRepository vwMagazynAGG { get; }
        IVwTowarAGGRepository vwTowarAGG { get; }
        IVwFinanseNalZobAGGRepository vwFinanseNalZobAGG { get; }
        IVwZamOdKlientaAGGRepository vwZamOdKlientaAGG { get; }
        IVwZestSprzedazyAGGRepository vwZestSprzedazyAGG { get; }
        IVwFinanseBankGTXRepository vwFinanseBankGTX { get; }
        #endregion

        #region GTEX2 Subiekt
        IVwMagazynGTX2Repository vwMagazynGTX2 { get; }
        IVwMagazynRuchGTX2Repository vwMagazynRuchGTX2 { get; }
        IVwFinanseNalZobGTX2Repository vwFinanseNalZobGTX2 { get; }
        IVwFinanseBankGTX2Repository vwFinanseBankGTX2 { get; }
        #endregion

        #region Konfiguracja
        ITblKonfiguracjaUrzadzenRepository tblKonfiguracjaUrzadzen { get; }
        #endregion
        
        //void Dispose();
        int Save();
        Task<int> SaveAsync();
        IQueryable<T> Query<T>();
    }
}