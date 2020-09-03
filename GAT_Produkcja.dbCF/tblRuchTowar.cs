namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Magazyn.tblRuchTowar")]
    public partial class tblRuchTowar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblRuchTowar()
        {
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
        }

        [Key]
        public int IDRuchTowar { get; set; }

        public int? IDRuchNaglowek { get; set; }

        public int? IDTowar { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ilosc { get; set; }

        public int? IDJm { get; set; }

        [Column(TypeName = "money")]
        public decimal? CenaJedn { get; set; }

        [Column(TypeName = "money")]
        public decimal? KosztNetto { get; set; }

        public int? IDVat { get; set; }

        [Column(TypeName = "money")]
        public decimal? KosztBrutto { get; set; }

        public string Uwagi { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscPrzed { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscPo { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscZarezerwowana { get; set; }

        [StringLength(50)]
        public string NrParti { get; set; }

        public virtual tblJm tblJm { get; set; }

        public virtual tblVAT tblVAT { get; set; }

        public virtual tblRuchNaglowek tblRuchNaglowek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }

        public virtual tblTowar tblTowar { get; set; }
    }
}
