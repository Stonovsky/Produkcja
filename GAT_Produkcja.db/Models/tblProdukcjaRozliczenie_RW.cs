using GAT_Produkcja.db.EntitesInterfaces;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaRozliczenie_RW")]
    public partial class tblProdukcjaRozliczenie_RW : IProdukcjaRozliczenie
    {
        [Key]
        public int IDProdukcjaRozliczenie_RW { get; set; }

        [ForeignKey(nameof(tblProdukcjaRozliczenie_Naglowek))]
        public int IDProdukcjaRozliczenie_Naglowek { get; set; }
        [StringLength(20)]
        public string NrZlecenia { get; set; }
        public int IDZlecenie { get; set; }
        public int IDSurowiecMsAccess { get; set; }
        public int IDSurowiecSubiekt { get; set; }
        public int IDNormaZuzyciaMsAccess { get; set; }

        [StringLength(100)]
        public string NazwaSurowcaMsAccess { get; set; }
        public string NazwaTowaruSubiekt { get; set; }
        [StringLength(30)]
        public string SymbolTowaruSubiekt { get; set; }
        
        public decimal Udzial { get; set; }

        public decimal Ilosc { get; set; }
        [ForeignKey(nameof(tblJm))]
        public int? IDJm { get; set; }

        [NotMapped]
        public string Jm { get; set; }

        public decimal CenaJednostkowa { get; set; }
        public decimal Wartosc { get; set; }
        public DateTime DataDodania { get; set; }

        public virtual tblProdukcjaRozliczenie_Naglowek tblProdukcjaRozliczenie_Naglowek { get; set; }
        public virtual tblJm tblJm { get; set; }

    }

}
