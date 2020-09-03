using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Enums
{
    public enum StatusRuchuTowarowEnum
    {
        PrzyjecieZewnetrne_PZ = 1,
        PrzesuniecieMiedzymagazynowe_MM = 2,
        WydanieZewnetrzne_WZ = 3,
        WydanieWewnetrzne_WW=4,
        RozchodWewnetrzny_RW = 5,
        Zablokowano_Z = 6,
        PrzyjecieWewnetrzne_PW = 7,
    }
}
