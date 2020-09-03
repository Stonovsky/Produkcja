namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Magazyn.vwStanTowaru")]
    public partial class vwStanTowaru
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDTowar { get; set; }

        [StringLength(20)]
        public string Symbol { get; set; }

        public string Nazwa { get; set; }

        public string Opis { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscCalkowita { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscZarezerwowana { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscDostepna { get; set; }

        public int? IDMagazyn { get; set; }

        public string Magazyn { get; set; }

        public int? IDFirma { get; set; }

        [StringLength(255)]
        public string Firma { get; set; }
    }
}
