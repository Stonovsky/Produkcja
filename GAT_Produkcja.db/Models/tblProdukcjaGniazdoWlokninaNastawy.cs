using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Table("Produkcja.tblProdukcjaGniazdoWlokninaNastawy")]
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public partial class tblProdukcjaGniazdoWlokninaNastawy
    {
        [Key]
        [Column("IDProdukcjaGniazdoWlokninaNastawy")]
        public int IDProdukcjaGniazdoWlokninaNastawy { get; set; }
        
        public string Uwagi { get; set; }

        public virtual tblProdukcjaGniazdoWloknina tblProdukcjaGniazdoWloknina { get; set; }
    }
}

