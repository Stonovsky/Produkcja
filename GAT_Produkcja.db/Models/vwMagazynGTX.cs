using GAT_Produkcja.db.EntitesInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Table("GTEX.vwMagazynGTX")]
    public class vwMagazynGTX : IMagazynSubiekt
    {
        [Key]
        public int IdMagazyn { get; set; }
        public string Symbol { get; set; }
        public string Nazwa { get; set; }
        public int Status { get; set; }
        public string Opis { get; set; }
    }
}
