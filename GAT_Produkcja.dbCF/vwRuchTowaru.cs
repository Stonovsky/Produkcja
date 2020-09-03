namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Magazyn.vwRuchTowaru")]
    public partial class vwRuchTowaru
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDRuchNaglowek { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime DataPrzyjecia { get; set; }

        public int? ID_PracownikGAT { get; set; }

        [StringLength(276)]
        public string ImieINazwiskoGAT { get; set; }

        public int? IDKontrahent { get; set; }

        [StringLength(255)]
        public string NazwaKontrahenta { get; set; }

        public int? IDMagazynZ { get; set; }

        public string ZMagazynu { get; set; }

        public int? IDFirmaZ { get; set; }

        public string ZFirmy { get; set; }

        public int? IDMagazynDo { get; set; }

        public string DoMagazynu { get; set; }

        public int? IDFirmaDo { get; set; }

        public string DoFirmy { get; set; }

        public int? IDTowar { get; set; }

        public string NazwaTowaru { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ilosc { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscPrzed { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscPo { get; set; }
    }
}
