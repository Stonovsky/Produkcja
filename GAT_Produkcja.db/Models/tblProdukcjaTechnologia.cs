using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaTechnologia")]
    public partial class tblProdukcjaTechnologia
    {
        [Key]
        public int IDProdukcjaTechnologia { get; set; }

        [Column("IDProdukcjaGniazdoProdukcyjne")]
        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public int? IDProdukcjaGniazdaProdukcyjne { get; set; }

        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
    }
}
