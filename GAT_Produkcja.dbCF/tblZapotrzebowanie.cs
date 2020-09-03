namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblZapotrzebowanie")]
    public partial class tblZapotrzebowanie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblZapotrzebowanie()
        {
            tblPliki = new HashSet<tblPliki>();
            tblZapotrzebowaniePozycje = new HashSet<tblZapotrzebowaniePozycje>();
        }

        [Key]
        public int IDZapotrzebowanie { get; set; }

        public int? Nr { get; set; }

        public int? IDKontrahent { get; set; }

        public int? IDPracownikGAT { get; set; }

        public DateTime? DataZgloszenia { get; set; }

        public DateTime? DataZapotrzebowania { get; set; }

        [StringLength(255)]
        public string Opis { get; set; }

        public int? IDKlasyfikacjaOgolna { get; set; }

        public int? IDKlasyfikacjaSzczegolowa { get; set; }

        public int? IDUrzadzenia { get; set; }

        [StringLength(255)]
        public string Uwagi { get; set; }

        public int? IDZapotrzebowanieStatus { get; set; }

        public int? IDFirma { get; set; }

        public virtual tblKlasyfikacjaOgolna tblKlasyfikacjaOgolna { get; set; }

        public virtual tblKlasyfikacjaSzczegolowa tblKlasyfikacjaSzczegolowa { get; set; }

        public virtual tblKontrahent tblKontrahent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPliki> tblPliki { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }

        public virtual tblUrzadzenia tblUrzadzenia { get; set; }

        public virtual tblZapotrzebowanieStatus tblZapotrzebowanieStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZapotrzebowaniePozycje> tblZapotrzebowaniePozycje { get; set; }
    }
}
