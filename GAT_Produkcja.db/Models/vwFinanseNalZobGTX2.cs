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
    [Table("GTEX2.vwFinanseNalZobGTX2")]

    public partial class vwFinanseNalZobGTX2 : IFinanseNaleznosciZobowiazania
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DataPowstania { get; set; }
        public DateTime? TerminPlatnosci { get; set; }
        public int? DniSpoznienia { get; set; }
        public string NrDok { get; set; }
        public string Kontrahent { get; set; }
        public string NIP { get; set; }
        public decimal? Naleznosc { get; set; }
        public decimal? Zobowiazanie { get; set; }
        [NotMapped]
        public string Firma { get; set; }
        [NotMapped]
        public int IdFirma { get; set; }
    }
}
