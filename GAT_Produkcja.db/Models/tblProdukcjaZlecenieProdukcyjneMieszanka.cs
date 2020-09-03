using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [AddINotifyPropertyChangedInterface]
    [Serializable]

    [Table("Produkcja.tblProdukcjaZlecenieProdukcyjneMieszanka")]
    public partial class tblProdukcjaZlecenieProdukcyjneMieszanka : ValidationBase
    {
        [Key]
        public int IDMieszanka { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public int? IDZlecenieProdukcyjne { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblTowar))]
        public int? IDTowar { get; set; }
        
        [Required(ErrorMessage = "Pole wymagane")]
        public decimal? ZawartoscProcentowa { get; set; }

        public decimal? IloscKg { get; set; }
        
        [Required(ErrorMessage = "Pole wymagane")]
        public decimal? IloscSumarycznaKg { get; set; }
        
        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblJm))]
        public int IDJm { get; set; }

        public virtual tblTowar tblTowar { get; set; }
        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblJm tblJm { get; set; }
    }
}
