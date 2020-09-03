using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Mieszanka.tblMieszanka")]
    public partial class tblMieszanka : ValidationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMieszanka()
        {
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
        }

        [Key]
        public int IDMieszanka { get; set; }

        [ForeignKey(nameof(tblPracownikGAT))]
        public int IDPracownikGAT { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        public DateTime? DataUtworzenia { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [StringLength(20)]
        public string Symbol { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [StringLength(100)]
        public string NazwaMieszanki { get; set; }

        public int? IDFirma { get; set; }

        public int? IDMagazyn { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [Column(TypeName = "money")]
        public decimal? Ilosc { get; set; }

        [Column(TypeName = "money")]
        public decimal? CenaJednNetto { get; set; }

        [Column(TypeName = "money")]
        public decimal? KosztNetto { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        public int? IDJm { get; set; }
        public string Opis { get; set; }
        public string Uwagi { get; set; }

        public virtual tblFirma tblFirma { get; set; }

        public virtual tblJm tblJm { get; set; }

        public virtual tblMagazyn tblMagazyn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }
        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
    }
}
