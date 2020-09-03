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

    [Table("Towar.tblTowarGeokomorkaParametryTyp")]

    public partial class tblTowarGeokomorkaParametryTyp : ValidationBase
    {
        [Key]
        [Column("IDTowarGeokomorkaParametryTyp")]
        public int IDTowarGeokomorkaParametryTyp { get; set; }

        [Required]
        public string Typ { get; set; }

        [Required]
        public decimal GruboscPasa { get; set; }
    }
}
