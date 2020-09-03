using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Table("Ceny.vwCenyMagazynoweAGG")]

    public partial class vwCenyMagazynoweAGG
    {
        [Key]
        public int IDProdukt { get; set; }
        public string Nazwa { get; set; }
        public string NazwaWlasna { get; set; }
        
        [Column(TypeName = "money")]
        public decimal CenaMagazynowa { get; set; }

        public int IDJm { get; set; }
        public string Jm { get; set; }

    }
}
