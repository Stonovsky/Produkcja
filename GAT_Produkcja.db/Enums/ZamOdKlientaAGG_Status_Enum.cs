using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Enums
{
    public enum ZamOdKlientaAGG_Status_Enum
    {
        Wycofany = 0,
        Wykonany = 1,
        Uniewazniony = 2,
        Odlozony = 3,
        MM_wydany = 4,
        Niezrealizowane = 5,
        Niezrealizowane_bezRezerwacji = 6,
        Niezrealizowane_zRezewacja = 7,
        Zrealizowane = 8,
    }
}
