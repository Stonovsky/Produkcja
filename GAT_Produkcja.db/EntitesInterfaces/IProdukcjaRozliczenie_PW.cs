using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IProdukcjaRozliczenie_PW
    {
        string NazwaTowaruSubiekt { get;  }
        string SymbolTowaruSubiekt { get;  }
        decimal Ilosc { get;  }
        decimal CenaJednostkowa { get;  }
        decimal Wartosc { get;  }
        int? IDJm { get;  }
        string Jm { get;  }
        /// <summary>
        /// Id Zlecenia bazowego (linia wloknin)
        /// </summary>
        int IDZlecenie { get;  }

        /// <summary>
        /// Nr Zlecenia bazowego (linia wloknin)
        /// </summary>
        string NrZlecenia { get;  }
        string NrWz { get;  }
        string NrRolki { get;  }
        decimal Odpad_kg { get;  }
    }
}
