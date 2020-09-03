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

    [Table("Towar.tblTowarGeokomorkaParametryZgrzew")]

    public partial class tblTowarGeokomorkaParametryZgrzew : ValidationBase
    {
        [Key]
        [Column("IDTowarGeokomorkaParametryZgrzew")]
        public int IDTowarGeokomorkaParametryZgrzew { get; set; }

        [Required]
        public string KodZgrzewu { get; set; }

        [Required]
        public int Zgrzew { get; set; }
    }
}
