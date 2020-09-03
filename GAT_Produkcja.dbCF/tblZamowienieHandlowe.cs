namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zamowienia.tblZamowienieHandlowe")]
    public partial class tblZamowienieHandlowe
    {
        [Key]
        public int IDZamowienieHandlowe { get; set; }

        public int? IDPracownikGAT { get; set; }

        public int? IDOdbiorca { get; set; }

        public string OsobaKontaktowa { get; set; }

        public string TelefonDoKontaktu { get; set; }

        public string UlicaDostawy { get; set; }

        public string MiastoDostawy { get; set; }

        public string KodPocztowyDostawy { get; set; }

        public string NrZamowienia { get; set; }

        public DateTime? DataZamowienia { get; set; }

        public DateTime? DataWysylki { get; set; }

        public int? IDZamowieniaPrzesylkaKoszt { get; set; }

        public int? IDPrzewoznik { get; set; }

        public bool? CzyOdbiorWlasny { get; set; }

        public int? IDZamowieniaWarunkiPlatnosci { get; set; }

        public int? IDZamowieniaTerminPlatnosci { get; set; }

        public string NrWZ { get; set; }

        public string NrFV { get; set; }

        public string Uwagi { get; set; }

        public virtual tblKontrahent tblKontrahent { get; set; }

        public virtual tblKontrahent tblKontrahent1 { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }

        public virtual tblZamowieniaPrzesylkaKoszt tblZamowieniaPrzesylkaKoszt { get; set; }

        public virtual tblZamowieniaTerminPlatnosci tblZamowieniaTerminPlatnosci { get; set; }

        public virtual tblZamowieniaWarunkiPlatnosci tblZamowieniaWarunkiPlatnosci { get; set; }
    }
}
