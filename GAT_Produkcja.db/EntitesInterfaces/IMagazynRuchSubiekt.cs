using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IMagazynRuchSubiekt
    {
        int IdMagazynRuch { get; set; }
        int IdTowar { get; set; }
        string TowarSymbol { get; set; }
        string TowarNazwa { get; set; }
        int IdMagazyn { get; set; }
        string MagazynSymbol { get; set; }
        string MagazynNazwa { get; set; }
        DateTime Data { get; set; }
        decimal Ilosc { get; set; }
        decimal Pozostalo { get; set; }
        string Jm { get; set; }
        decimal Cena { get; set; }
        decimal Wartosc { get; set; }
        string Firma { get; set; }

    }
}
