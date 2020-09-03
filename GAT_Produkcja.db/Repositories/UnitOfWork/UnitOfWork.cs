using GAT_Produkcja.db.Repositories.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GAT_ProdukcjaModel ctx;
        public ITblFirmaRepository tblFirma { get; private set; }
        public ITblJmRepository tblJm { get; private set; }

        public ITblKlasyfikacjaOgolnaRepository tblKlasyfikacjaOgolna { get; private set; }

        public ITblKlasyfikacjaSzczegolowaRepository tblKlasyfikacjaSzczegolowa { get; private set; }

        public ITblUrzadzeniaRepository tblUrzadzenia { get; private set; }

        public ITblKontrahentRepository tblKontrahent { get; private set; }

        public ITblPlikiRepository tblPliki { get; private set; }

        public ITblPracownikGATRepository tblPracownikGAT { get; private set; }

        public IVwZapotrzebowanieEwidencjaRepository vwZapotrzebowanieEwidencja { get; private set; }

        public ITblZapotrzebowanieStatusRepository tblZapotrzebowanieStatus { get; private set; }

        public ITblZapotrzebowaniePozycjeRepository tblZapotrzebowaniePozycje { get; private set; }

        public ITblZapotrzebowanieRepository tblZapotrzebowanie { get; private set; }

        public IVwFVKosztowezSubiektGTRepository vwFVKosztowezSubiektGT { get; private set; }

        public ITblWynikiBadanGeowlokninRepository tblWynikiBadanGeowloknin { get; private set; }

        public ITblWynikiBadanDlaProbekRepository tblWynikiBadanDlaProbek { get; private set; }

        public ITblVATRepository tblVAT { get; private set; }

        public ITblCertyfikatCERepository tblCertyfikatCE { get; private set; }

        #region Magazyn
        public ITblMagazynRepository tblMagazyn { get; private set; }

        public ITblRuchNaglowekRepository tblRuchNaglowek { get; private set; }

        public ITblRuchStatusRepository tblRuchStatus { get; private set; }

        public ITblRuchTowarRepository tblRuchTowar { get; private set; }

        public ITblTowarRepository tblTowar { get; private set; }

        public ITblTowarStatusRepository tblTowarStatus { get; private set; }

        public ITblKodKreskowyTypRepository tblKodKreskowyTyp { get; private set; }

        public IVwStanTowaruRepository vwStanTowaru { get; private set; }

        public IVwRuchTowaruRepository vwRuchTowaru { get; private set; }
        public ITblDokumentTypRepository tblDokumentTyp { get; private set; }

        #endregion

        #region ZamowienieHandlowe
        public ITblZamowieniaPrzesylkaKosztRepository tblZamowieniaPrzesylkaKoszt { get; private set; }

        public ITblZamowieniaTerminPlatnosciRepository tblZamowieniaTerminPlatnosci { get; private set; }

        public ITblZamowieniaWarunkiPlatnosciRepository tblZamowieniaWarunkiPlatnosci { get; private set; }

        public ITblZamowienieHandloweRepository tblZamowienieHandlowe { get; private set; }

        public ITblZamowienieHandlowePakowanieRepository tblZamowienieHandlowePakowanie { get; private set; }


        public ITblTowarGeowlokninaParametryGramaturaRepository tblTowarGeowlokninaParametryGramatura { get; private set; }

        public ITblTowarGeowlokninaParametryRodzajRepository tblTowarGeowlokninaParametryRodzaj { get; private set; }
        public ITblTowarGeowlokninaParametryRepository tblTowarGeowlokninaParametry { get; private set; }

        public ITblZamowienieHandlowePakowanieRodzajRepository tblZamowienieHandlowePakowanieRodzaj { get; private set; }

        public ITblTowarGeokomorkaParametryGeometryczneRepository tblTowarGeokomorkaParametryGeometryczne { get; private set; }

        public ITblTowarGeokomorkaParametryTypRepository tblTowarGeokomorkaParametryTyp { get; private set; }

        public ITblTowarGeokomorkaParametryZgrzewRepository tblTowarGeokomorkaParametryZgrzew { get; private set; }

        public ITblZamowienieHandloweTowarGeokomorkaRepository tblZamowienieHandloweTowarGeokomorka { get; private set; }

        public ITblZamowienieHandloweTowarGeowlokninaRepository tblZamowienieHandloweTowarGeowloknina { get; private set; }

        public ITblTowarGeokomorkaParametryRodzajRepository tblTowarGeokomorkaParametryRodzaj { get; private set; }


        #endregion

        #region ZlecenieProdukcyjne
        public ITblMieszankaRepository tblMieszanka { get; private set; }

        public ITblMieszankaSkladRepository tblMieszankaSklad { get; private set; }

        public ITblTowarParametryRepository tblTowarParametry { get; private set; }

        public ITblProdukcjaZlcecenieProdukcyjne_tblTowarParametryRepository tblProdukcjaZlcecenieProdukcyjne_tblTowarParametry { get; private set; }

        public ITblProdukcjaZlecenieRepository tblProdukcjaZlecenie { get; private set; }

        public ITblTowarGrupaRepository tblTowarGrupa { get; private set; }

        public ITblRuchTowarGeowlokninaParametryRepository tblRuchTowarGeowlokninaParametry { get; private set; }

        public ITblProdukcjaTulejeRepository tblProdukcjaTuleje { get; private set; }
        //public ITblProdukcjaPaletyRepository tblProdukcjaPalety { get; set; }

        #region GniazdaProdukcyjne
        public ITblProdukcjaGniazdoProdukcyjneRepository tblProdukcjaGniazdoProdukcyjne { get; private set; }

        //public ITblProdukcjaGniazdoWlokninaNastawyRepository tblProdukcjaGniazdoWlokninaNastawy { get; private set; }
        public ITblProdukcjaGniazdoWlokninaRepository tblProdukcjaGniazdoWloknina { get; private set; }

        public ITblProdukcjaGniazdoKalanderNastawyRepository tblProdukcjaGniazdoKalanderNastawy { get; private set; }
        public ITblProdukcjaGniazdoKalanderRepository tblProdukcjaGniazdoKalander { get; private set; }

        //public ITblProdukcjaGniazdoKonfekcjaNastawyRepository tblProdukcjaGniazdoKonfekcjaNastawy { get; private set; }
        //public ITblProdukcjaGniazdoKonfekcjaRepository tblProdukcjaGniazdoKonfekcja { get; private set; }

        public ITblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjneRepository tblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjne { get; private set; }

        public ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository tblProdukcjaZlecenieProdukcyjne_Mieszanka { get; private set; }

        public ITblTowarGeowlokninaParametrySurowiecRepository tblTowarGeowlokninaParametrySurowiec { get; private set; }

        public ITblProdukcjaGniazdoWlokninaBadaniaRepository tblProdukcjaGniazdoWlokninaBadania { get; private set; }

        public IVwCenyMagazynoweAGGRepository vwCenyMagazynoweAGG { get; private set; }

        #endregion

        #region ProdukcjaRozliczenie
        public ITblProdukcjaRozliczenie_DaneRepository tblProdukcjaRozliczenie_Dane { get; private set; }
        public ITblProdukcjaRozliczenie_PWRepository tblProdukcjaRozliczenie_PW { get; private set; }
        public ITblProdukcjaRozliczenie_RWRepository tblProdukcjaRozliczenie_RW { get; private set; }

        public ITblProdukcjaRuchTowarRepository tblProdukcjaRuchTowar { get; private set; }

        public ITblProdukcjaRuchNaglowekRepository tblProdukcjaRuchNaglowek { get; private set; }

        public ITblLogRepository tblLog { get; private set; }

        public ITblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowarRepository tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar { get; private set; }

        #endregion

        public ITblProdukcjaZlecenieCieciaRepository tblProdukcjaZlecenieCiecia { get; private set; }

        public ITblProdukcjaZlecenieTowarRepository tblProdukcjaZlecenieTowar { get; private set; }

        public ITblProdukcjaZlecenieStatusRepository tblProdukcjaZlecenieStatus { get; private set; }

        public ITblProdukcjaRozliczenie_CenyTransferoweRepository tblProdukcjaRozliczenie_CenyTransferowe { get; private set; }

        public ITblProdukcjaRozliczenie_NaglowekRepository tblProdukcjaRozliczenie_Naglowek { get; private set; }

        public ITblProdukcjaRozliczenie_PWPodsumowanieRepository tblProdukcjaRozliczenie_PWPodsumowanie { get; private set; }

        #region Finanse
        public ITblFinanseStanKontaRepository tblFinanseStanKonta { get; private set; }

        #endregion

        #endregion

        #region GTEX Subiekt
        public IVwMagazynRuchGTXRepository vwMagazynRuchGTX { get; private set; }

        public IVwMagazynGTXRepository vwMagazynGTX { get; private set; }

        public IVwTowarGTXRepository vwTowarGTX { get; private set; }


        public IVwFinanseNalZobGTXRepository vwFinanseNalZobGTX { get; private set; }
        public IVwFinanseBankGTXRepository vwFinanseBankGTX { get; private set; }

        #endregion

        #region AGG Subiekt

        public IVwMagazynRuchAGGRepository vwMagazynRuchAGG { get; private set; }

        public IVwMagazynAGGRepository vwMagazynAGG { get; private set; }

        public IVwTowarAGGRepository vwTowarAGG { get; private set; }

        public IVwFinanseNalZobAGGRepository vwFinanseNalZobAGG { get; private set; }

        public IVwZamOdKlientaAGGRepository vwZamOdKlientaAGG { get; private set; }

        public IVwZestSprzedazyAGGRepository vwZestSprzedazyAGG { get; private set; }

        public IVwFinanseBankAGGRepository vwFinanseBankAGG { get; private set; }
        #endregion

        #region GTEX2 Subiekt
        public IVwMagazynRuchGTX2Repository vwMagazynRuchGTX2 { get; private set; }
        public IVwFinanseNalZobGTX2Repository vwFinanseNalZobGTX2 { get; private set; }
        public IVwFinanseBankGTX2Repository vwFinanseBankGTX2 { get; private set; }
        public ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository tblProdukcjaGeokomorkaPodsumowaniePrzerob { get; private set; }
        public IVwMagazynGTX2Repository vwMagazynGTX2 => new VwMagazynGTX2Repository(ctx);

        public ITblProdukcjaRuchTowarBadaniaRepository tblProdukcjaRuchTowarBadania { get; private set; }

        #endregion
        #region Konfiguracja
        public ITblKonfiguracjaUrzadzenRepository tblKonfiguracjaUrzadzen  {get; private set; }

    #endregion
    public UnitOfWork(GAT_ProdukcjaModel context)
        {
            ctx = context;

            tblFirma = new TblFirmaRepository(ctx);
            tblCertyfikatCE = new TblCertyfikatCERepository(ctx);
            tblJm = new TblJmRepository(ctx);
            tblKlasyfikacjaOgolna = new TblKlasyfikacjaOgolnaRepository(ctx);
            tblKlasyfikacjaSzczegolowa = new TblKlasyfikacjaSzczegolowaRepository(ctx);
            tblUrzadzenia = new TblUrzadzeniaRepository(ctx);
            tblKontrahent = new TblKontrahentRepository(ctx);
            tblPliki = new TblPlikiRepository(ctx);
            tblPracownikGAT = new TblPracownikGATRepository(ctx);
            tblZapotrzebowanie = new TblZapotrzebowanieRepository(ctx);
            tblZapotrzebowanieStatus = new TblZapotrzebowanieStatusRepository(ctx);
            tblZapotrzebowaniePozycje = new TblZapotrzebowaniePozycjeRepository(ctx);
            vwZapotrzebowanieEwidencja = new VwZapotrzebowanieEwidencjaRepository(ctx);
            vwFVKosztowezSubiektGT = new VwFVKosztowezSubiektGTRepository(ctx);
            tblWynikiBadanGeowloknin = new TblWynikiBadanGeowlokninRepository(ctx);
            tblWynikiBadanDlaProbek = new TblWynikiBadanDlaProbekRepository(ctx);
            tblVAT = new TblVATRepository(ctx);
            tblMagazyn = new TblMagazynRepository(ctx);
            tblRuchNaglowek = new TblRuchNaglowekRepository(ctx);
            tblRuchStatus = new TblRuchStatusRepository(ctx);
            tblRuchTowar = new TblRuchTowarRepository(ctx);
            tblTowar = new TblTowarRepository(ctx);
            tblTowarStatus = new TblTowarStatusRepository(ctx);
            tblKodKreskowyTyp = new TblKodKreskowyTypRepository(ctx);
            vwStanTowaru = new VwStanTowaruRepository(ctx);
            vwRuchTowaru = new VwRuchTowaruRepository(ctx);
            tblZamowieniaPrzesylkaKoszt = new TblZamowieniaPrzesylkaKosztRepository(ctx);
            tblZamowieniaTerminPlatnosci = new TblZamowieniaTerminPlatnosciRepository(ctx);
            tblZamowieniaWarunkiPlatnosci = new TblZamowieniaWarunkiPlatnosciRepository(ctx);
            tblZamowienieHandlowe = new TblZamowienieHandloweRepository(ctx);
            tblZamowienieHandlowePakowanie = new TblZamowienieHandlowePakowanieRepository(ctx);
            tblTowarGeowlokninaParametryGramatura = new TblTowarGeowlokninaParametryGramaturaRepository(ctx);
            tblTowarGeowlokninaParametryRodzaj = new TblTowarGeowlokninaParametryRodzajRepository(ctx);
            tblTowarGeowlokninaParametry = new TblTowarGeowlokninaParametryRepository(ctx);
            tblZamowienieHandlowePakowanieRodzaj = new TblZamowienieHandlowePakowanieRodzajRepository(ctx);
            tblZamowienieHandloweTowarGeowloknina = new TblZamowienieHandloweTowarGeowlokninaRepository(ctx);
            tblTowarGeokomorkaParametryRodzaj = new TblTowarGeokomorkaParametryRodzajRepository(ctx);
            tblTowarGeokomorkaParametryZgrzew = new TblTowarGeokomorkaParametryZgrzewRepository(ctx);
            tblTowarGeokomorkaParametryTyp = new TblTowarGeokomorkaParametryTypRepository(ctx);
            tblTowarGeokomorkaParametryGeometryczne = new TblTowarGeokomorkaParametryGeometryczneRepository(ctx);
            tblMieszanka = new TblMieszankaRepository(ctx);
            tblMieszankaSklad = new TblMieszankaSkladRepository(ctx);
            tblTowarParametry = new TblTowarParametryRepository(ctx);
            tblProdukcjaZlcecenieProdukcyjne_tblTowarParametry = new TblProdukcjaZlcecenieProdukcyjne_tblTowarParametryRepository(ctx);
            tblDokumentTyp = new TblDokumentTypRepository(ctx);
            tblProdukcjaZlecenie = new TblProdukcjaZlecenieRepository(ctx);
            tblTowarGrupa = new TblTowarGrupaRepository(ctx);
            tblRuchTowarGeowlokninaParametry = new TblRuchTowarGeowlokninaParametryRepository(ctx);
            tblProdukcjaGniazdoProdukcyjne = new TblProdukcjaGniazdoProdukcyjneRepository(ctx);
            tblProdukcjaTuleje = new TblProdukcjaTulejeRepository(ctx);
            tblProdukcjaGniazdoWloknina = new TblProdukcjaGniazdoWlokninaRepository(ctx);
            //tblProdukcjaGniazdoWlokninaNastawy = new TblProdukcjaGniazdoWlokninaNastawyRepository(ctx);
            tblProdukcjaGniazdoKalander = new TblProdukcjaGniazdoKalanderRepository(ctx);
            tblProdukcjaGniazdoKalanderNastawy = new TblProdukcjaGniazdoKalanderNastawyRepository(ctx);
            //tblProdukcjaGniazdoKonfekcja = new TblProdukcjaGniazdoKonfekcjaRepository(ctx);
            //tblProdukcjaGniazdoKonfekcjaNastawy = new TblProdukcjaGniazdoKonfekcjaNastawyRepository(ctx);

            tblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjne = new TblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjneRepository(ctx);
            tblProdukcjaZlecenieProdukcyjne_Mieszanka = new TblProdukcjaZlecenieProdukcyjne_MieszankaRepository(ctx);
            tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar = new TblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowarRepository(ctx);

            tblTowarGeowlokninaParametrySurowiec = new TblTowarGeowlokninaParametrySurowiecRepository(ctx);
            tblProdukcjaGniazdoWlokninaBadania = new TblProdukcjaGniazdoWlokninaBadaniaRepository(ctx);
            vwCenyMagazynoweAGG = new VwCenyMagazynoweAGGRepository(ctx);
            tblProdukcjaRozliczenie_Dane = new TblProdukcjaRozliczenie_DaneRepository(ctx);
            tblProdukcjaRozliczenie_PW = new TblProdukcjaRozliczenie_PWRepository(ctx);
            tblProdukcjaRozliczenie_RW = new TblProdukcjaRozliczenie_RWRepository(ctx);
            tblProdukcjaRozliczenie_CenyTransferowe = new TblProdukcjaRozliczenie_CenyTransferoweRepository(ctx);
            tblProdukcjaRuchTowar = new TblProdukcjaRuchTowarRepository(ctx);
            tblProdukcjaRuchTowarBadania = new TblProdukcjaRuchTowarBadaniaRepository(ctx);
            tblProdukcjaRuchNaglowek = new TblProdukcjaRuchNaglowekRepository(ctx);
            tblLog = new TblLogRepository(ctx);
            tblProdukcjaZlecenieCiecia = new TblProdukcjaZlecenieCieciaRepository(ctx);
            tblProdukcjaZlecenieTowar = new TblProdukcjaZlecenieTowarRepository(ctx);
            tblProdukcjaZlecenieStatus = new TblProdukcjaZlecenieStatusRepository(ctx);
            tblProdukcjaRozliczenie_Naglowek = new TblProdukcjaRozliczenie_NaglowekRepository(ctx);
            tblProdukcjaRozliczenie_PWPodsumowanie = new TblProdukcjaRozliczenie_PWPodsumowanieRepository(ctx);
            
            tblProdukcjaGeokomorkaPodsumowaniePrzerob = new TblProdukcjaGeokomorkaPodsumowaniePrzerobRepository(ctx);

            vwMagazynRuchGTX = new VwMagazynRuchGTXRepository(ctx);
            vwMagazynGTX = new VwMagazynGTXRepository(ctx);
            vwTowarGTX = new VwTowarGTXRepository(ctx);
            vwFinanseNalZobGTX = new VwFinanseNalZobGTXRepository(ctx);

            vwMagazynAGG = new VwMagazynAGGRepository(ctx);
            vwMagazynRuchAGG = new VwMagazynRuchAGGRepository(ctx);
            vwTowarAGG = new VwTowarAGGRepository(ctx);
            vwFinanseNalZobAGG = new VwFinanseNalZobAGGRepository(ctx);
            vwZamOdKlientaAGG = new VwZamOdKlientaAGGRepository(ctx);
            vwZestSprzedazyAGG = new VwZestSprzedazyAGGRepository(ctx);
            
            vwFinanseNalZobGTX2 = new VwFinanseNalZobGTX2Repository(ctx);
            vwMagazynRuchGTX2 = new VwMagazynRuchGTX2Repository(ctx);
            vwFinanseBankAGG = new VwFinanseBankAGGRepository(ctx);
            vwFinanseBankGTX = new VwFinanseBankGTXRepository(ctx);
            vwFinanseBankGTX2 = new VwFinanseBankGTX2Repository(ctx);
            tblFinanseStanKonta = new TblFinanseStanKontaRepository(ctx);

            #region Konfiguracja
            tblKonfiguracjaUrzadzen = new TblKonfiguracjaUrzadzenRepository(ctx);
            #endregion
        }

        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }

        public int Save()
        {
            return ctx.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await ctx.SaveChangesAsync();
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }

}
