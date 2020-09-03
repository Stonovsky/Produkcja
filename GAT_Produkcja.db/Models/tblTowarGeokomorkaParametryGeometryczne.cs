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

    [Table("Towar.tblTowarGeokomorkaParametryGeometryczne")]

    public partial class tblTowarGeokomorkaParametryGeometryczne : ValidationBase
    {
        [Key]
        [Column("IDTowarGeokomorkaParametryGeometryczne")]
        public int IDTowarGeokomorkaParametryGeometryczne { get; set; }

        [ForeignKey(nameof(tblTowarParametryGeokomorkaZgrzew))]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDTowarParametryGeokomorkaZgrzew { get; set; }
        public int Przekatna1PoRozlozeniu_mm { get; set; }
        public int Przekatna2PoRozlozeniu_mm { get; set; }
        public int WymiarBokuKomorki_mm { get; set; }
        public decimal PowierzchniaKomorki_cm2 { get; set; }
        public decimal SzerokoscStandardowaSekcji_m { get; set; }
        public decimal DlugoscStandardowaSekcji_m { get; set; }
        public int IloscKomorekPoSzerokosciSekcji_szt { get; set; }
        public int IloscKomorekPoDlugosciSekcji_szt { get; set; }
        public decimal PowierzchniaSekcji_m2 { get; set; }
        public string NazwaZwyczajowa { get; set; }

        public virtual tblTowarGeokomorkaParametryZgrzew tblTowarParametryGeokomorkaZgrzew { get; set; }

    }
}
