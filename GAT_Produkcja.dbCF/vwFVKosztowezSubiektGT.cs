namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vwFVKosztowezSubiektGT")]
    public partial class vwFVKosztowezSubiektGT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string NrFVKlienta { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string NrWewnetrznyZobowiazaniaSGT { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(40)]
        public string Odebral { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string RodzajDok { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(500)]
        public string Uwagi { get; set; }

        public string NrZP { get; set; }

        public int? IDKontrahent { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(51)]
        public string KontrahentNazwa { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(40)]
        public string Miasto { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(82)]
        public string Ulica { get; set; }

        [StringLength(20)]
        public string NIP { get; set; }

        [Column(TypeName = "money")]
        public decimal? WartscNetto { get; set; }

        [Column(TypeName = "money")]
        public decimal? WartscBrutto { get; set; }

        [Column(TypeName = "money")]
        public decimal? KwotaDoZaplaty { get; set; }

        public DateTime? DataOtrzymania { get; set; }

        public DateTime? TerminPlantosci { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string IDFirma { get; set; }

        [StringLength(255)]
        public string FirmaNazwa { get; set; }
    }
}
