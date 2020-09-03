using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbComarch
{
    public static class ConnectionStringHelper
    {
        public static string GetConnectionString()
        {
            return @"Server=192.168.34.57\optima;Database=CDN_GTEX;User ID=sa;Password=Comarch!2011";
        }

    }
}
