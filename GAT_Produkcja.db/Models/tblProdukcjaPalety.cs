using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaPalety")]
    public partial class tblProdukcjaPalety
    {
        [Key]
        public int IDPaleta { get; set; }

        [ForeignKey(nameof(tblProdukcjaRuchTowar))]
        public int IDProdukcjaRuchTowar { get; set; }
        public int NrPalety { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public string Uwagi { get; set; }

        public virtual tblProdukcjaRuchTowar tblProdukcjaRuchTowar { get; set; }
    }
}
