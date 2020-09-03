using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Extensions
{
    public static class tblProdukcjaRuchTowarExtensions
    {
        public static string PobierzKierunekPrzychodu(this tblProdukcjaRuchTowar towar)
        {
            if (towar.IDProdukcjaRozliczenieStatus == (int)ProdukcjaRozliczenieStatusEnum.NieRozliczono)
                return "Linia";
            else
                return "Magazyn";
        }
    }
}
