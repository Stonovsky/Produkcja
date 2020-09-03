using GAT_Produkcja.db.EntityValidation;
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
    [Table("Finanse.tblFinanseStanKonta")]
    public class tblFinanseStanKonta : ValidationBase
    {
        [Key]
        public int IDFinanseStanKonta { get; set; }

        [ForeignKey(nameof(tblFirma))]
        public int IdFirma { get; set; }
        [StringLength(100)]

        public string Firma { get; set; }
        public int IdBank { get; set; }
        public string BankNazwa { get; set; }
        public string NrKonta { get; set; }
        public decimal Stan { get; set; }
        public DateTime DataStanu { get; set; }
        [StringLength(10)]
        public string Waluta { get; set; }
        public DateTime DataDodania { get; set; }
        
        [ForeignKey(nameof(tblPracownikGAT))]
        public int IdOperator { get; set; }


        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
        public virtual tblFirma tblFirma { get; set; }


        public override string ToString()
        {
            return $"{BankNazwa}, {Stan} {Waluta}";
        }
    }
}
