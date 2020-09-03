using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IProdukcjaRuchTowarWymiary
    {

        string TowarNazwa { get; }

        decimal Szerokosc_m { get;  }
        decimal Dlugosc_m { get;  }
        decimal Waga_kg { get;  }
        decimal Ilosc_m2 { get;  }


    }
}
