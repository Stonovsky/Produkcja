using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaTuleje")]
    public partial class tblProdukcjaTuleje
    {
        [Key]
        [Column("IDTuleja")]
        public int IDTuleja { get; set; }
        public decimal Srednica { get; set; }
        public decimal GruboscScianki { get; set; }
        public string Opis { get; set; }
        public string Uwagi { get; set; }

    }
}
