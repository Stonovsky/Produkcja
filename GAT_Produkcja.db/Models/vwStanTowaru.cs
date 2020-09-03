namespace GAT_Produkcja.db
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
        public long RowNumber { get; set; }
        public int IDTowar { get; set; }
        public int IDTowarGrupa { get; set; }

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
        public int? IDJm{ get; set; }
        public string Jm { get; set; }
    }
}
