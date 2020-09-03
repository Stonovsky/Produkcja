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
    [Table("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob")]
    public class tblProdukcjaGeokomorkaPodsumowaniePrzerob : ValidationBase
    {
        [Key]
        [Column("IdProdukcjaGeokomorkaPodsumowaniePrzerob")]
        public int IdProdukcjaGeokomorkaPodsumowaniePrzerob { get; set; }

        public decimal IloscWyprodukowana_kg { get; set; }
        public decimal IloscNawrot_kg { get; set; }
        
        [Range(0.01d, Double.MaxValue,ErrorMessage ="Pole nie może być zerowe")]
        public decimal Ilosc_kg { get; set; }

        [Range(0.01d, Double.MaxValue,ErrorMessage ="Pole nie może być zerowe")]
        public decimal Ilosc_m2{ get; set; }
        public decimal CenaJedn_kg { get; set; }
        public decimal Wartosc { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
        public DateTime DataDodania { get; set; }
        [ForeignKey(nameof(tblPracownikGAT))]
        public int IdOperator { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
    }
}
