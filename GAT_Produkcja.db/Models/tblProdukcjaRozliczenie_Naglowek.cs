
using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaRozliczenie_Naglowek")]
    public partial class tblProdukcjaRozliczenie_Naglowek : ValidationBase
    {
        [Key]
        [Column("IDProdukcjaRozliczenie_Naglowek")]
        public int IDProdukcjaRozliczenie_Naglowek { get; set; }

        public int IDTowarAccess { get; set; }

        public int IDZlecenie { get; set; }

        [StringLength(20)]
        public string NrZlecenia { get; set; }

        [StringLength(100)]
        public string TowarNazwa { get; set; }
        public DateTime DataDodania { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(0.01d, 1000000, ErrorMessage = "Pole musi mieć wartość większą od 0")]
        public decimal Szerokosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(1d, 1000000, ErrorMessage = "Pole musi mieć wartość większą od 0")]
        public decimal Dlugosc { get; set; }



        [ForeignKey(nameof(tblPracownikGAT))]
        public int IDPracownikGAT { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
    }
}
