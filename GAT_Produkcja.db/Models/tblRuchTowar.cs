namespace GAT_Produkcja.db
{
    using GAT_Produkcja.db.EntityValidation;
    using PropertyChanged;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Magazyn.tblRuchTowar")]
    public partial class tblRuchTowar : ValidationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblRuchTowar()
        {
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
        }

        [Key]
        public int IDRuchTowar { get; set; }

        public int? IDRuchNaglowek { get; set; }

        [ForeignKey(nameof(tblDokumentTyp))]
        public int IDDokumentTyp { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int? IDTowar { get; set; }

        [ForeignKey(nameof(tblMagazyn))]
        public int? IDMagazyn { get; set; }


        [Column(TypeName = "money")]
        [Required(ErrorMessage = "Pole wymagane")]
        //[Range(0, int.MaxValue, ErrorMessage = "Wartoœæ musi byæ wiêksza od 0")]
        public decimal Ilosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
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

        [Required(ErrorMessage = "Pole wymagane")]
        [StringLength(50)]
        public string NrParti { get; set; } //nr drukowany na kodzie kreskowym

        public string NrRolki { get; set; } //wewn. nr rolki w podziale na L i P

        public virtual tblJm tblJm { get; set; }

        public virtual tblVAT tblVAT { get; set; }

        public virtual tblRuchNaglowek tblRuchNaglowek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }
        public virtual tblMagazyn tblMagazyn { get; set; }
        public virtual tblTowar tblTowar { get; set; }
        public virtual tblDokumentTyp tblDokumentTyp { get; set; }
    }
}
