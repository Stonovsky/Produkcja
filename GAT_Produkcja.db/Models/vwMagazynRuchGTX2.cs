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
    [Table("GTEX2.vwMagazynRuchGTX2")]

    public partial class vwMagazynRuchGTX2 : IMagazynRuchSubiekt
    {
        [Key]
        public int IdMagazynRuch { get; set; }
        public int IdTowar { get; set; }
        public string TowarSymbol{ get; set; }
        public string TowarNazwa { get; set; }
        public int IdMagazyn { get; set; }
        public string MagazynSymbol{ get; set; }
        public string MagazynNazwa { get; set; }
        public DateTime Data { get; set; }
        public decimal Ilosc { get; set; }
        public decimal Pozostalo { get; set; }
        public string Jm { get; set; }
        public decimal Cena { get; set; }
        public decimal Wartosc { get; set; }
        [NotMapped]
        public string Firma { get ; set ; }

        public override string ToString()
        {
            return $"{TowarSymbol}, Ilosc={Pozostalo}";
        }
    }
}
