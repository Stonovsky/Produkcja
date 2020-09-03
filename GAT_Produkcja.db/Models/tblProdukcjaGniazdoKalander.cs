using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaGniazdoKalander")]
    public partial class tblProdukcjaGniazdoKalander
    {
        [Key]
        [Column("IDProdukcjaGniazdoKalander")]
        public int IDProdukcjaGniazdoKalander { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public int IDProdukcjaZlecenieProdukcyjne { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaGniazdoKalanderNastawy))]
        public int IDProdukcjaGniazdoKalanderNastawy { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaTuleje))]
        public int IDTuleja { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        public decimal Szerokosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public decimal Dlugosc { get; set; }

        public string Uwagi { get; set; }

        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
        public virtual tblProdukcjaGniazdoKalanderNastawy tblProdukcjaGniazdoKalanderNastawy { get; set; }
        public virtual tblProdukcjaTuleje tblProdukcjaTuleje { get; set; }
    }
}
