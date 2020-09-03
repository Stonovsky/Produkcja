namespace GAT_Produkcja.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zapotrzebowanie.vwZapotrzebowanieEwidencjaRejestracjaFV")]
    public partial class vwZapotrzebowanieEwidencjaRejestracjaFV
    {
        [StringLength(50)]
        public string StatusZapotrzebowania { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDZapotrzebowanie { get; set; }

        public int? Nr { get; set; }

        [StringLength(276)]
        public string ImieINazwiskoGAT { get; set; }

        [StringLength(255)]
        public string Opis { get; set; }

        [StringLength(255)]
        public string ZakupOd { get; set; }

        [Column(TypeName = "money")]
        public decimal? SumaOfKoszt { get; set; }

        [StringLength(255)]
        public string KlasyfikacjaOgolna { get; set; }

        [StringLength(100)]
        public string KlasyfikacjaSzczegolowa { get; set; }

        [StringLength(100)]
        public string Urzadzenie { get; set; }

        [StringLength(255)]
        public string NazwaFirmy { get; set; }

        public string StatusFV { get; set; }
    }
}
