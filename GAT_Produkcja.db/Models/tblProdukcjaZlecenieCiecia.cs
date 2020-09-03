using GAT_Produkcja.db.EntitesInterfaces;
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

    [Table("Produkcja.tblProdukcjaZlecenieCiecia")]
    public partial class tblProdukcjaZlecenieCiecia : ValidationBase, ItblProdukcjaZlecenieCiecia
    {

        [Key]
        public int IDProdukcjaZlecenieCiecia { get; set; }

        public int NrZleceniaCiecia { get; set; }
        public string KodKreskowy { get; set; }

        [Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblPracownikGAT_Zlecajacy))]
        public int IDZlecajacy { get; set; }

        [Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblPracownikGAT_Wykonujacy))]
        public int IDWykonujacy { get; set; }

        [Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblKontrahent))]
        public int IDKontrahent { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlecenieStatus))]
        public int IDProdukcjaZlecenieStatus { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public DateTime DataZlecenia { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public DateTime DataWykonania { get; set; }


        public string NrDokumentu { get; set; }
        public string RodzajPakowania { get; set; }
        public string Uwagi { get; set; }
        public decimal Zaawansowanie { get; set; }


        [ForeignKey(nameof(tblProdukcjaRozliczenieStatus))]
        public int? IDProdukcjaRozliczenieStatus { get; set; }
        public DateTime? DataRozliczenia { get; set; }

        public virtual tblProdukcjaRozliczenieStatus tblProdukcjaRozliczenieStatus { get; set; }
        //public virtual tblPracownikGAT tblPracownikGAT{ get; set; }
        public virtual tblPracownikGAT tblPracownikGAT_Zlecajacy { get; set; }
        public virtual tblPracownikGAT tblPracownikGAT_Wykonujacy { get; set; }
        public virtual tblKontrahent tblKontrahent { get; set; }
        public virtual tblProdukcjaZlecenieStatus tblProdukcjaZlecenieStatus { get; set; }
    }
}
