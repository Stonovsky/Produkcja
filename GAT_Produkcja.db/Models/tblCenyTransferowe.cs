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
    [Table("Magazyn.tblCenyTransferowe")]
    public partial class tblCenyTransferowe
    {
        [Key]
        public int IDCenaTransferowa { get; set; }

        public int IDTowarStatusZ { get; set; }

        public int IDTowarStatusDo { get; set; }

        [Column(TypeName = "money")]
        public decimal Cena { get; set; }

        [StringLength(100)]
        public string Uwagi { get; set; }

        public virtual tblTowarStatus tblTowarStatus { get; set; }

        public virtual tblTowarStatus tblTowarStatus1 { get; set; }
    }
}
