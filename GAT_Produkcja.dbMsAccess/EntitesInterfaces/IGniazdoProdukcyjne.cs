using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.EntitesInterfaces
{
    public interface IGniazdoProdukcyjne
    {
        int ZlecenieID { get; set; }
        string Zlecenie { get; set; }
        string NrSztuki { get; set; }
        DateTime Data { get; set; }
        DateTime Godzina { get; set; }
        string Artykul { get; set; }
        int OperatorID { get; set; }
        decimal Szerokosc { get; set; }
        decimal Dlugosc { get; set; }
        decimal Waga { get; set; }
        decimal WagaOdpadu { get; set; }
        decimal IloscM2  {get; set; }

    }
}
