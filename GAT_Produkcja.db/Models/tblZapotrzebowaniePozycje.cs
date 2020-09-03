using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblZapotrzebowaniePozycje")]
    public partial class tblZapotrzebowaniePozycje : ValidationBase
    {
        [Key]
        public int IDZapotrzebowaniePozycja { get; set; }

        [ForeignKey(nameof(tblJm))]
        [Required(ErrorMessage ="Pole wymagane")]
        public int? IDJm { get; set; }
        
        [NotMapped]
        public string Jm { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage ="Pole wymagane")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        public double? Ilosc { get; set; }

        [Column(TypeName = "money")]
        [Required(ErrorMessage ="Pole wymagane")]
        public decimal? Cena { get; set; }

        [Column(TypeName = "money")]
        public decimal? Koszt { get; set; }

        [StringLength(255)]
        public string Uwagi { get; set; }

        public int? IDZapotrzebowanie { get; set; }

        public virtual tblZapotrzebowanie tblZapotrzebowanie { get; set; }
        public virtual tblJm tblJm { get; set; }
    }
}
