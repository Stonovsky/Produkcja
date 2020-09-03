using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IFinanseNaleznosciZobowiazania
    {
        int Id { get; set; }
        DateTime? DataPowstania { get; set; }
        DateTime? TerminPlatnosci { get; set; }
        int? DniSpoznienia { get; set; }
        string NrDok { get; set; }
        string Kontrahent { get; set; }
        string NIP { get; set; }
        decimal? Naleznosc { get; set; }
        decimal? Zobowiazanie { get; set; }
        int IdFirma { get; set; }

    }
}
