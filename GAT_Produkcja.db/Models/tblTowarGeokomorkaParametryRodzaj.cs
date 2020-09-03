using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Towar.tblTowarGeokomorkaParametryRodzaj")]

    public partial class tblTowarGeokomorkaParametryRodzaj
    {
        [Key]
        public int IDTowarGeokomorkaParametryRodzaj { get; set; }
        public string Rodzaj { get; set; }
        public string RodzajSkrot { get; set; }
    }
}
