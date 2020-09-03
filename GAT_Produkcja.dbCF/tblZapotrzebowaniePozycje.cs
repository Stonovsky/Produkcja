namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblZapotrzebowaniePozycje")]
    public partial class tblZapotrzebowaniePozycje
    {
        [Key]
        public int IDZapotrzebowaniePozycja { get; set; }

        public int? IDJm { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }

        public double? Ilosc { get; set; }

        [Column(TypeName = "money")]
        public decimal? Cena { get; set; }

        [Column(TypeName = "money")]
        public decimal? Koszt { get; set; }

        [StringLength(255)]
        public string Uwagi { get; set; }

        public int? IDZapotrzebowanie { get; set; }

        public virtual tblZapotrzebowanie tblZapotrzebowanie { get; set; }
    }
}
