using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    public partial class tblWynikiBadanGeowloknin
    {
        [NotMapped]
        public string StatusBadania { get; set; }
    }
}
