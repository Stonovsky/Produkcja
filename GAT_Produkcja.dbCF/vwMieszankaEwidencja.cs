namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mieszanka.vwMieszankaEwidencja")]
    public partial class vwMieszankaEwidencja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDMieszanka { get; set; }

        [StringLength(20)]
        public string Symbol { get; set; }

        [StringLength(100)]
        public string NazwaMieszanki { get; set; }

        public DateTime? DataUtworzenia { get; set; }

        public int? IDMagazynDocelowy { get; set; }

        public string NazwaMagazynuDocelowego { get; set; }

        public int? IDFirmyDocelowej { get; set; }

        [StringLength(255)]
        public string NazwaFirmaDocelowa { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ilosc { get; set; }

        public int? IDJm { get; set; }

        [StringLength(10)]
        public string Jm { get; set; }

        [Column(TypeName = "money")]
        public decimal? CenaJednNetto { get; set; }

        [Column(TypeName = "money")]
        public decimal? KosztNetto { get; set; }
    }
}
