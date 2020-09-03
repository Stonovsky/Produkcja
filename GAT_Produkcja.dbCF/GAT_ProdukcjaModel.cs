namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GAT_ProdukcjaEntities : DbContext
    {
        public GAT_ProdukcjaEntities()
            : base("name=GAT_ProdukcjaEntities")
        {
        }

        public virtual DbSet<tblWynikiBadanDlaProbek> tblWynikiBadanDlaProbek { get; set; }
        public virtual DbSet<tblWynikiBadanGeowloknin> tblWynikiBadanGeowloknin { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tblFirma> tblFirma { get; set; }
        public virtual DbSet<tblJm> tblJm { get; set; }
        public virtual DbSet<tblKlasyfikacja> tblKlasyfikacja { get; set; }
        public virtual DbSet<tblKlasyfikacjaOgolna> tblKlasyfikacjaOgolna { get; set; }
        public virtual DbSet<tblKlasyfikacjaOgolna_SzczegolowaMM> tblKlasyfikacjaOgolna_SzczegolowaMM { get; set; }
        public virtual DbSet<tblKlasyfikacjaProdukcji_UrzadzeniaMM> tblKlasyfikacjaProdukcji_UrzadzeniaMM { get; set; }
        public virtual DbSet<tblKlasyfikacjaSzczegolowa> tblKlasyfikacjaSzczegolowa { get; set; }
        public virtual DbSet<tblKlasyfikacjaSzczegolowa_UrzadzeniaMM> tblKlasyfikacjaSzczegolowa_UrzadzeniaMM { get; set; }
        public virtual DbSet<tblKontrahent> tblKontrahent { get; set; }
        public virtual DbSet<tblKosztRodzaj> tblKosztRodzaj { get; set; }
        public virtual DbSet<tblKosztTyp> tblKosztTyp { get; set; }
        public virtual DbSet<tblLog> tblLog { get; set; }
        public virtual DbSet<tblPliki> tblPliki { get; set; }
        public virtual DbSet<tblPracownikGAT> tblPracownikGAT { get; set; }
        public virtual DbSet<tblPracownikGATDostep> tblPracownikGATDostep { get; set; }
        public virtual DbSet<tblUrzadzenia> tblUrzadzenia { get; set; }
        public virtual DbSet<tblVAT> tblVAT { get; set; }
        public virtual DbSet<tblWojewodztwa> tblWojewodztwa { get; set; }
        public virtual DbSet<tblZapotrzebowanie> tblZapotrzebowanie { get; set; }
        public virtual DbSet<tblZapotrzebowaniePozycje> tblZapotrzebowaniePozycje { get; set; }
        public virtual DbSet<tblZapotrzebowaniePozycjeMM> tblZapotrzebowaniePozycjeMM { get; set; }
        public virtual DbSet<tblZapotrzebowanieStatus> tblZapotrzebowanieStatus { get; set; }
        public virtual DbSet<tblCenyTransferowe> tblCenyTransferowe { get; set; }
        public virtual DbSet<tblMagazyn> tblMagazyn { get; set; }
        public virtual DbSet<tblRuchNaglowek> tblRuchNaglowek { get; set; }
        public virtual DbSet<tblRuchStatus> tblRuchStatus { get; set; }
        public virtual DbSet<tblRuchTowar> tblRuchTowar { get; set; }
        public virtual DbSet<tblMieszanka> tblMieszanka { get; set; }
        public virtual DbSet<tblMieszankaSklad> tblMieszankaSklad { get; set; }
        public virtual DbSet<tblProdukcjaGniazdaProdukcyjne> tblProdukcjaGniazdaProdukcyjne { get; set; }
        public virtual DbSet<tblProdukcjaMieszanka> tblProdukcjaMieszanka { get; set; }
        public virtual DbSet<tblProdukcjaTechnologia> tblProdukcjaTechnologia { get; set; }
        public virtual DbSet<tblProdukcjaZlcecenieProdukcyjne> tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual DbSet<tblFakturaZapotrzebowanie> tblFakturaZapotrzebowanie { get; set; }
        public virtual DbSet<tblKodKreskowyTyp> tblKodKreskowyTyp { get; set; }
        public virtual DbSet<tblTowar> tblTowar { get; set; }
        public virtual DbSet<tblTowarGramatura> tblTowarGramatura { get; set; }
        public virtual DbSet<tblTowarParametry> tblTowarParametry { get; set; }
        public virtual DbSet<tblTowarStatus> tblTowarStatus { get; set; }
        public virtual DbSet<tblTowarTyp> tblTowarTyp { get; set; }
        public virtual DbSet<tblZamowieniaPrzesylkaKoszt> tblZamowieniaPrzesylkaKoszt { get; set; }
        public virtual DbSet<tblZamowieniaTerminPlatnosci> tblZamowieniaTerminPlatnosci { get; set; }
        public virtual DbSet<tblZamowieniaWarunkiPlatnosci> tblZamowieniaWarunkiPlatnosci { get; set; }
        public virtual DbSet<tblZamowienieHandlowe> tblZamowienieHandlowe { get; set; }
        public virtual DbSet<tblZamowienieHandlowePakowanie> tblZamowienieHandlowePakowanie { get; set; }
        public virtual DbSet<tblZamowienieHandloweTowar> tblZamowienieHandloweTowar { get; set; }
        public virtual DbSet<tblTest> tblTest { get; set; }
        public virtual DbSet<tblTowar_Kontrahent> tblTowar_Kontrahent { get; set; }
        public virtual DbSet<vwFVKosztowezSubiektGT> vwFVKosztowezSubiektGT { get; set; }
        public virtual DbSet<vwFVZarejestrowane> vwFVZarejestrowane { get; set; }
        public virtual DbSet<vwRuchTowaru> vwRuchTowaru { get; set; }
        public virtual DbSet<vwStanTowaru> vwStanTowaru { get; set; }
        public virtual DbSet<vwMieszankaEwidencja> vwMieszankaEwidencja { get; set; }
        public virtual DbSet<vwEwidencja> vwEwidencja { get; set; }
        public virtual DbSet<vwEwidencjaRejestracjiFV> vwEwidencjaRejestracjiFV { get; set; }
        public virtual DbSet<vwZapotrzebowanieEwidencja> vwZapotrzebowanieEwidencja { get; set; }
        public virtual DbSet<vwZapotrzebowanieEwidencjaRejestracjaFV> vwZapotrzebowanieEwidencjaRejestracjaFV { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblWynikiBadanDlaProbek>()
                .Property(e => e.Sila)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanDlaProbek>()
                .Property(e => e.Wytrzymalosc)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanDlaProbek>()
                .Property(e => e.WydluzenieCalkowite)
                .HasPrecision(10, 4);

            modelBuilder.Entity<tblWynikiBadanDlaProbek>()
                .Property(e => e.Gramatura)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.SilaMinimalna)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.SilaMaksymalna)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.SilaSrednia)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.WytrzymaloscMinimalna)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.WytrzymaloscMaksymalna)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.WytrzymaloscSrednia)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.WydluzenieMinimalne)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.WydluzenieMaksymalne)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.WydluzenieSrednie)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.GramaturaMinimalna)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.GramaturaMaksymalna)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblWynikiBadanGeowloknin>()
                .Property(e => e.GramaturaSrednia)
                .HasPrecision(10, 3);

            modelBuilder.Entity<tblFirma>()
                .HasMany(e => e.tblRuchNaglowek)
                .WithOptional(e => e.tblFirma)
                .HasForeignKey(e => e.IDFirmaZ);

            modelBuilder.Entity<tblFirma>()
                .HasMany(e => e.tblRuchNaglowek1)
                .WithOptional(e => e.tblFirma1)
                .HasForeignKey(e => e.IDFirmaDo);

            modelBuilder.Entity<tblKontrahent>()
                .Property(e => e.LimitKredytowy)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblKontrahent>()
                .HasMany(e => e.tblRuchNaglowek)
                .WithOptional(e => e.tblKontrahent)
                .HasForeignKey(e => e.IDKontrahent);

            modelBuilder.Entity<tblKontrahent>()
                .HasMany(e => e.tblZamowienieHandlowe)
                .WithOptional(e => e.tblKontrahent)
                .HasForeignKey(e => e.IDOdbiorca);

            modelBuilder.Entity<tblKontrahent>()
                .HasMany(e => e.tblZamowienieHandlowe1)
                .WithOptional(e => e.tblKontrahent1)
                .HasForeignKey(e => e.IDPrzewoznik);

            modelBuilder.Entity<tblKontrahent>()
                .HasMany(e => e.tblZapotrzebowanie)
                .WithOptional(e => e.tblKontrahent)
                .HasForeignKey(e => e.IDKontrahent);

            modelBuilder.Entity<tblPracownikGAT>()
                .HasMany(e => e.tblWynikiBadanGeowloknin)
                .WithOptional(e => e.tblPracownikGAT)
                .HasForeignKey(e => e.IDPracownikGAT);

            modelBuilder.Entity<tblPracownikGAT>()
                .HasMany(e => e.tblZamowienieHandlowe)
                .WithOptional(e => e.tblPracownikGAT)
                .HasForeignKey(e => e.IDPracownikGAT);

            modelBuilder.Entity<tblPracownikGAT>()
                .HasMany(e => e.tblZapotrzebowanie)
                .WithOptional(e => e.tblPracownikGAT)
                .HasForeignKey(e => e.IDPracownikGAT);

            modelBuilder.Entity<tblZapotrzebowaniePozycje>()
                .Property(e => e.Cena)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblZapotrzebowaniePozycje>()
                .Property(e => e.Koszt)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblCenyTransferowe>()
                .Property(e => e.Cena)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMagazyn>()
                .HasMany(e => e.tblRuchNaglowek)
                .WithOptional(e => e.tblMagazyn)
                .HasForeignKey(e => e.IDMagazynZ);

            modelBuilder.Entity<tblMagazyn>()
                .HasMany(e => e.tblRuchNaglowek1)
                .WithOptional(e => e.tblMagazyn1)
                .HasForeignKey(e => e.IDMagazynDo);

            modelBuilder.Entity<tblRuchStatus>()
                .Property(e => e.Symbol)
                .IsFixedLength();

            modelBuilder.Entity<tblRuchStatus>()
                .HasMany(e => e.tblRuchNaglowek)
                .WithRequired(e => e.tblRuchStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblRuchTowar>()
                .Property(e => e.Ilosc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblRuchTowar>()
                .Property(e => e.CenaJedn)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblRuchTowar>()
                .Property(e => e.KosztNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblRuchTowar>()
                .Property(e => e.KosztBrutto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblRuchTowar>()
                .Property(e => e.IloscPrzed)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblRuchTowar>()
                .Property(e => e.IloscPo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblRuchTowar>()
                .Property(e => e.IloscZarezerwowana)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMieszanka>()
                .Property(e => e.Ilosc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMieszanka>()
                .Property(e => e.CenaJednNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMieszanka>()
                .Property(e => e.KosztNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMieszankaSklad>()
                .Property(e => e.Ilosc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMieszankaSklad>()
                .Property(e => e.CenaJednNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMieszankaSklad>()
                .Property(e => e.KosztNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMieszankaSklad>()
                .Property(e => e.Udzial)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProdukcjaMieszanka>()
                .Property(e => e.Ilosc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProdukcjaMieszanka>()
                .Property(e => e.ZawartoscProcentowa)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProdukcjaZlcecenieProdukcyjne>()
                .Property(e => e.IloscKg)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProdukcjaZlcecenieProdukcyjne>()
                .Property(e => e.IloscM)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProdukcjaZlcecenieProdukcyjne>()
                .HasMany(e => e.tblProdukcjaMieszanka)
                .WithOptional(e => e.tblProdukcjaZlcecenieProdukcyjne)
                .HasForeignKey(e => e.IDZlecenieProdukcyjne);

            modelBuilder.Entity<tblFakturaZapotrzebowanie>()
                .Property(e => e.NrZapotrzebowania)
                .IsUnicode(false);

            modelBuilder.Entity<tblFakturaZapotrzebowanie>()
                .Property(e => e.NrWewnetrznyZobowiazaniaSGT)
                .IsUnicode(false);

            modelBuilder.Entity<tblTowarStatus>()
                .HasMany(e => e.tblCenyTransferowe)
                .WithRequired(e => e.tblTowarStatus)
                .HasForeignKey(e => e.IDTowarStatusZ)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblTowarStatus>()
                .HasMany(e => e.tblCenyTransferowe1)
                .WithRequired(e => e.tblTowarStatus1)
                .HasForeignKey(e => e.IDTowarStatusDo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblZamowienieHandloweTowar>()
                .Property(e => e.SzerokoscRolki)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblZamowienieHandloweTowar>()
                .Property(e => e.DlugoscNawoju)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblZamowienieHandloweTowar>()
                .Property(e => e.IloscRolek)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblZamowienieHandloweTowar>()
                .Property(e => e.IloscSumaryczna)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.NrFVKlienta)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.NrWewnetrznyZobowiazaniaSGT)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.Odebral)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.RodzajDok)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.Uwagi)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.KontrahentNazwa)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.Miasto)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.Ulica)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.NIP)
                .IsUnicode(false);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.WartscNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.WartscBrutto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.KwotaDoZaplaty)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwFVKosztowezSubiektGT>()
                .Property(e => e.IDFirma)
                .IsUnicode(false);

            modelBuilder.Entity<vwRuchTowaru>()
                .Property(e => e.ZMagazynu)
                .IsUnicode(false);

            modelBuilder.Entity<vwRuchTowaru>()
                .Property(e => e.ZFirmy)
                .IsUnicode(false);

            modelBuilder.Entity<vwRuchTowaru>()
                .Property(e => e.DoMagazynu)
                .IsUnicode(false);

            modelBuilder.Entity<vwRuchTowaru>()
                .Property(e => e.DoFirmy)
                .IsUnicode(false);

            modelBuilder.Entity<vwRuchTowaru>()
                .Property(e => e.Ilosc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwRuchTowaru>()
                .Property(e => e.IloscPrzed)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwRuchTowaru>()
                .Property(e => e.IloscPo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwStanTowaru>()
                .Property(e => e.IloscCalkowita)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwStanTowaru>()
                .Property(e => e.IloscZarezerwowana)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwStanTowaru>()
                .Property(e => e.IloscDostepna)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwMieszankaEwidencja>()
                .Property(e => e.Ilosc)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwMieszankaEwidencja>()
                .Property(e => e.CenaJednNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwMieszankaEwidencja>()
                .Property(e => e.KosztNetto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwEwidencja>()
                .Property(e => e.SumaOfKoszt)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwEwidencjaRejestracjiFV>()
                .Property(e => e.SumaOfKoszt)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwZapotrzebowanieEwidencja>()
                .Property(e => e.SumaOfKoszt)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwZapotrzebowanieEwidencjaRejestracjaFV>()
                .Property(e => e.SumaOfKoszt)
                .HasPrecision(19, 4);
        }
    }
}
