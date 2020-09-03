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
    [Table("Produkcja.tblProdukcjaRuchTowarStatus")]

    public partial class tblProdukcjaRuchTowarStatus
    {
        [Key]
        public int IDProdukcjaRuchTowarStatus { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
    }
}
