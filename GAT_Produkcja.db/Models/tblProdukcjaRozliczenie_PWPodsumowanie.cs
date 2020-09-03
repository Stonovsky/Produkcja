using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.EntityValidation;
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
    [Table("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie")]
    public partial class tblProdukcjaRozliczenie_PWPodsumowanie : ValidationBase, IProdukcjaRozliczenie
    {
        [Key]
        public int IDProdukcjaRozliczenie_PWPodsumowanie { get; set; }

        [ForeignKey(nameof(tblProdukcjaRozliczenie_Naglowek))]
        public int IDProdukcjaRozliczenie_Naglowek { get; set; }

        public int IDZlecenie { get; set; }
        [StringLength(20)]
        public string NrZlecenia { get; set; }
        [StringLength(30)]
        public string SymbolTowaruSubiekt { get; set; }
        [StringLength(100)]
        public string NazwaTowaruSubiekt { get; set; }

        public decimal Szerokosc_m { get; set; }
        public decimal Dlugosc_m { get; set; }
        public decimal Ilosc { get; set; }
        
        public decimal Ilosc_kg { get; set; }
        [ForeignKey(nameof(tblJm))]
        public int? IDJm { get; set; }
        [NotMapped]
        public string Jm { get; set; }
        public decimal CenaProduktuBezNarzutow_kg { get; set; }
        public decimal CenaProduktuBezNarzutow_m2 { get; set; }
        public decimal CenaSprzedazyGtex_m2 { get; set; }
        public decimal CenaHurtowaAGG_m2 { get; set; }
        
        public decimal CenaJednostkowa { get; set; }
        public decimal Wartosc { get; set; }

        public virtual tblProdukcjaRozliczenie_Naglowek tblProdukcjaRozliczenie_Naglowek { get; set; }
        public virtual tblJm tblJm { get; set; }
    }
}
