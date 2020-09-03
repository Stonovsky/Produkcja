namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mieszanka.tblMieszanka")]
    public partial class tblMieszanka
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMieszanka()
        {
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
        }

        [Key]
        public int IDMieszanka { get; set; }

        public DateTime? DataUtworzenia { get; set; }

        [StringLength(20)]
        public string Symbol { get; set; }

        [StringLength(100)]
        public string NazwaMieszanki { get; set; }

        public int? IDFirma { get; set; }

        public int? IDMagazyn { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ilosc { get; set; }

        [Column(TypeName = "money")]
        public decimal? CenaJednNetto { get; set; }

        [Column(TypeName = "money")]
        public decimal? KosztNetto { get; set; }

        public int? IDJm { get; set; }

        public virtual tblFirma tblFirma { get; set; }

        public virtual tblJm tblJm { get; set; }

        public virtual tblMagazyn tblMagazyn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }
    }
}
