using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IProdukcjaRozliczenie
    {
        string NazwaTowaruSubiekt { get; set; }
        string SymbolTowaruSubiekt { get; set; }
        decimal Ilosc { get; set; }
        decimal CenaJednostkowa { get; set; }
        decimal Wartosc { get; set; }
        int? IDJm { get; set; }
        string Jm{ get; set; }
        int IDZlecenie { get; set; }
        string NrZlecenia { get; set; }
    }
}
