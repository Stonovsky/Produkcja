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
    [AddINotifyPropertyChangedInterface]
    [Serializable]
    [Table("Produkcja.tblProdukcjaRozliczenieStatus")]
    public partial class tblProdukcjaRozliczenieStatus
    {
        [Key]
        public int IDProdukcjaRozliczenieStatus { get; set; }
        public string Status { get; set; }
    }
}
