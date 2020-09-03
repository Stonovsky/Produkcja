using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    public partial class tblKlasyfikacjaSzczegolowa
    {
        [NotMapped]
        public decimal SumaZapotrzebowania { get; set; }
    }
}
