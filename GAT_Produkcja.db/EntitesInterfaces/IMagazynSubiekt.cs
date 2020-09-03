using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IMagazynSubiekt
    {
        int IdMagazyn { get; set; }
        string Symbol { get; set; }
        string Nazwa { get; set; }
        int Status { get; set; }
        string Opis { get; set; }

    }
}
