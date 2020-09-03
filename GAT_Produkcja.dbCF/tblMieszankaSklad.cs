namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mieszanka.tblMieszankaSklad")]
    public partial class tblMieszankaSklad
    {
        [Key]
        public int IDMieszankaSklad { get; set; }

        public int? IDTowar { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ilosc { get; set; }

        [Column(TypeName = "money")]
        public decimal? CenaJednNetto { get; set; }

        [Column(TypeName = "money")]
        public decimal? KosztNetto { get; set; }

        [Column(TypeName = "money")]
        public decimal? Udzial { get; set; }

        public int? IDRuchTowar { get; set; }

        public int? IDFirma { get; set; }

        public int? IDMagazyn { get; set; }

        public int? IDMieszanka { get; set; }

        public int? IDJm { get; set; }

        public virtual tblFirma tblFirma { get; set; }

        public virtual tblJm tblJm { get; set; }

        public virtual tblMagazyn tblMagazyn { get; set; }

        public virtual tblRuchTowar tblRuchTowar { get; set; }

        public virtual tblMieszanka tblMieszanka { get; set; }

        public virtual tblTowar tblTowar { get; set; }
    }
}
