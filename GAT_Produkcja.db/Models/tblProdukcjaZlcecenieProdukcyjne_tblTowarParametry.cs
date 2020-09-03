using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaZlcecenieProdukcyjne_tblTowarParametry")]

    public partial class tblProdukcjaZlcecenieProdukcyjne_tblTowarParametry : ValidationBase
    {
        [Key, Column(Order = 1)]
        public int IDProdukcjaZlcecenieProdukcyjne { get; set; }

        [Key, Column(Order = 2)]
        public int IDTowarParametry { get; set; }

        public decimal Wartosc { get; set; }

        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblTowarParametry tblTowarParametry { get; set; }
    }
}
