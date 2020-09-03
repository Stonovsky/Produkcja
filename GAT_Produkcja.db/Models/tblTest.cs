using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GAT_Produkcja.db
{
    [Serializable]
    
    [AddINotifyPropertyChangedInterface]

    [Table("tblTest")]
    public partial class tblTest
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string txt { get; set; }

        [StringLength(50)]
        public string txt2 { get; set; }

        [StringLength(50)]
        public string txt3 { get; set; }
    }
}
