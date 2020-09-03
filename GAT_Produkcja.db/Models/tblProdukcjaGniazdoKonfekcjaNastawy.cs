using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaGniazdoKonfekcjaNastawy")]
    public partial class tblProdukcjaGniazdoKonfekcjaNastawy
    {
        [Key]
        [Column("IDProdukcjaGniazdoKonfekcjaNastawy")]
        public int IDProdukcjaGniazdoKonfekcjaNastawy { get; set; }
        
        public decimal Szerokosc { get; set; }
        public decimal DlugoscNawoju { get; set; }
        public decimal PredkoscOdwijarki { get; set; }
        public decimal PredkoscNawijarki { get; set; }
        public string Uwagi { get; set; }

        public virtual tblProdukcjaGniazdoKonfekcja tblProdukcjaGniazdoKonfekcja { get; set; }
    }
}

