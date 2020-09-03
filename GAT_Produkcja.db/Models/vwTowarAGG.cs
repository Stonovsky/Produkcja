using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Table("AGG.vwTowarAGG")]
    public partial class vwTowarAGG
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
