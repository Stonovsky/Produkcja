using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaGniazdoKonfekcja")]
    public partial class tblProdukcjaGniazdoKonfekcja
    {
        [Key]
        [Column("IDProdukcjaGniazdoKonfekcja")]
        public int IDProdukcjaGniazdoKonfekcja { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public int IDProdukcjaZlecenieProdukcyjne { get; set; }

        //[Required(ErrorMessage = "Pole wymagane")]
        //[ForeignKey(nameof(tblProdukcjaGniazdoKonfekcjaNastawy))]
        //public int IDProdukcjaGniazdoKonfekcjaNastawy { get; set; }

        //[Required(ErrorMessage = "Pole wymagane")]
        //[ForeignKey(nameof(tblProdukcjaTuleje))]
        //public int IDTuleja { get; set; }

        //[Required(ErrorMessage = "Pole wymagane")]
        //[ForeignKey(nameof(tblProdukcjaPalety))]
        //public int IDPaleta { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        public decimal Szerokosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public decimal Dlugosc { get; set; }
        
        [Required(ErrorMessage = "Pole wymagane")]
        public int IloscSztukNaPalecie { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public string Pakowanie { get; set; }
        public decimal Waga { get; set; }
        public string Uwagi { get; set; }

        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
        //public virtual tblProdukcjaGniazdoKonfekcjaNastawy tblProdukcjaGniazdoKonfekcjaNastawy { get; set; }
        //public virtual tblProdukcjaTuleje tblProdukcjaTuleje { get; set; }
        //public virtual tblProdukcjaPalety tblProdukcjaPalety { get; set; ]

    }
}
