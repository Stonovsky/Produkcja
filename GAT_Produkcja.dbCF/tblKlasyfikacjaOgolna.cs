namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblKlasyfikacjaOgolna")]
    public partial class tblKlasyfikacjaOgolna
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblKlasyfikacjaOgolna()
        {
            tblZapotrzebowanie = new HashSet<tblZapotrzebowanie>();
        }

        [Key]
        public int IDKlasyfikacjaOgolna { get; set; }

        [StringLength(100)]
        public string Nazwa { get; set; }

        [StringLength(255)]
        public string Opis { get; set; }

        public int? IDFirma { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZapotrzebowanie> tblZapotrzebowanie { get; set; }
    }
}
