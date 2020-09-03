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
    [Table("GTEX.vwTowarGTX")]
    public partial class vwTowarGTX : ValidationBase
    {
        [Key]
        public int IdTowar { get; set; }
        public int Rodzaj { get; set; }
        [StringLength(20)]
        public string Symbol { get; set; }
        [StringLength(50)]
        public string Nazwa { get; set; }
        [StringLength(255)]
        public string Opis { get; set; }
        public int IdGrupa { get; set; }
        [StringLength(50)]
        public string GrupaNazwa { get; set; }
    }
}
