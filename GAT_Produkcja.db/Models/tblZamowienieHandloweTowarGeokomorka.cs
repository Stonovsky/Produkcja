using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Zamowienia.tblZamowienieHandloweTowarGeokomorka")]
    public partial class tblZamowienieHandloweTowarGeokomorka : ValidationBase
    {
        [Key]
        public int IDZamowienieHandloweTowarGeokomorka { get; set; }

        [ForeignKey(nameof(tblTowar))]
        public int IDTowar { get; set; }

        [ForeignKey(nameof(tblZamowienieHandlowe))]
        public int IDZamowienieHandlowe { get; set; }

        [Column("IDTowarGeokomorkaParametryRodzaj")]
        [ForeignKey(nameof(tblTowarGeokomorkaParametryRodzaj))]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDTowarGeokomorkaParametryRodzaj { get; set; }

        [Column("IDTowarGeokomorkaParametryTyp")]
        [ForeignKey(nameof(tblTowarGeokomorkaParametryTyp))]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDTowarGeokomorkaParametryTyp { get; set; }

        [ForeignKey(nameof(tblTowarGeokomorkaParametryZgrzew))]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDTowarGeokomorkaParametryZgrzew { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        //[Range(1, int.MaxValue, ErrorMessage = "Wartoœæ nieprawid³owa, wprowadŸ wiêksz¹ wartoœæ")]
        public int Wysokosc_mm { get; set; }
        public int SzerokoscSekcji_mm { get; set; }
        public int DlugoscSekcji_mm { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(1d, 1000000, ErrorMessage = "Pole musi mieæ wartoœæ wiêksz¹ od 0")]
        public decimal? Ilosc_m2 { get; set; }
        public int? IloscSekcji_szt { get; set; }
        public decimal Waga_kg { get; set; }
        public string NazwaPelna { get; set; }
        public bool CzyTowarNiestandardowy { get; set; }
        public virtual tblTowarGeokomorkaParametryRodzaj tblTowarGeokomorkaParametryRodzaj { get; set; }
        public virtual tblTowarGeokomorkaParametryTyp tblTowarGeokomorkaParametryTyp { get; set; }
        public virtual tblTowarGeokomorkaParametryZgrzew tblTowarGeokomorkaParametryZgrzew { get; set; }
        public virtual tblZamowienieHandlowe tblZamowienieHandlowe { get; set; }
        public virtual tblTowar tblTowar{ get; set; }
    }
}
