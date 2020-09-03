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
    [Table("Produkcja.tblProdukcjaRozliczenie_PW")]
    public partial class tblProdukcjaRozliczenie_PW : IProdukcjaRozliczenie, IProdukcjaRozliczenie_PW
    {
        [NotMapped]
        public bool IsChecked { get; set; }
        [Key]
        public int IDProdukcjaRozliczenie_PW { get; set; }

        [ForeignKey(nameof(tblProdukcjaRozliczenie_Naglowek))]
        public int IDProdukcjaRozliczenie_Naglowek { get; set; }

        /// <summary>
        /// Id Zlecenia bazowego (linia wloknin)
        /// </summary>
        public int IDZlecenie { get; set; }
        /// <summary>
        /// Nr zlecenia bazowego (linia wloknin)
        /// </summary>
        [StringLength(20)]
        public string NrZlecenia { get; set; }
        /// <summary>
        /// ID pozycji z MsAccess jezeli wystepuje
        /// </summary>
        public int? IDMsAccess { get; set; }

        [StringLength(20)]
        public string NrWz { get; set; }

        [StringLength(20)]
        public string NrRolki { get; set; }        

        [StringLength(20)]
        public string NrRolkiBazowej { get; set; }

        [StringLength(20)]
        public string SymbolRolkiBazowej { get; set; }
        [StringLength(150)]
        public string NazwaRolkiBazowej { get; set; }

        [StringLength(100)]
        public string NazwaTowaru { get; set; }

        [StringLength(100)]
        public string NazwaTowaruSubiekt { get; set; }

        [StringLength(30)]
        public string SymbolTowaruSubiekt { get; set; }
        public decimal Szerokosc_m { get; set; }
        public decimal Dlugosc_m { get; set; }
        public decimal Ilosc_kg { get; set; }
        public decimal Odpad_kg { get; set; }
        public decimal Ilosc { get; set; }

        [ForeignKey(nameof(tblJm))]
        public int? IDJm { get; set; }

        [NotMapped]
        public int Ilosc_szt { get; set; }

        [NotMapped]
        public string Jm { get; set; }

        public decimal CenaProduktuBezNarzutow_kg { get; set; }
        public decimal CenaProduktuBezNarzutow_m2 { get; set; }
        public decimal CenaSprzedazyGtex_m2 { get; set; }
        public decimal CenaHurtowaAGG_m2 { get; set; }
        
        public decimal CenaJednostkowa { get; set; }
        public decimal Wartosc { get; set; }
        public string Przychod { get; set; }
        /// <summary>
        /// Wlasciwosc na potrzeby ProdukcjaEwidencjaViewModel
        /// 
        /// </summary>
        [NotMapped]
        public decimal WartoscOdpad { get; set; }


        public virtual tblProdukcjaRozliczenie_Naglowek tblProdukcjaRozliczenie_Naglowek { get; set; }
        public virtual tblJm tblJm { get; set; }

        public override string ToString()
        {
            return $"{NazwaTowaruSubiekt}, {CenaProduktuBezNarzutow_kg}, {Wartosc}";
        }
    }
}
