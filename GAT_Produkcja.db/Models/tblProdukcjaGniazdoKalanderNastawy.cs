using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaGniazdoKalanderNastawy")]
    public partial class tblProdukcjaGniazdoKalanderNastawy
    {
        [Key]
        [Column("IDProdukcjaGniazdoKalanderNastawy")]
        public int IDProdukcjaGniazdoKalanderNastawy { get; set; }

        public decimal TemperaturaGora { get; set; }
        public decimal TemperaturaDol { get; set; }
        public decimal PredkoscKalandra { get; set; }
        public decimal PredkoscChlodzenia { get; set; }
        public decimal TemperaturaChlodzenia { get; set; }
        public decimal DociskL { get; set; }
        public decimal DociskP { get; set; }
        public int IloscObciaznikow { get; set; }
        public decimal PredkoscOdwijania { get; set; }
        public decimal PredkoscZawijania { get; set; }
        public string Uwagi { get; set; }
    }
}

