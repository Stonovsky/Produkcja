using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    
    [AddINotifyPropertyChangedInterface]
    [Table("Badania.tblWynikiBadanGeowloknin")]
    public partial class tblWynikiBadanGeowloknin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblWynikiBadanGeowloknin()
        {
            tblWynikiBadanDlaProbek = new HashSet<tblWynikiBadanDlaProbek>();
        }

        [Key]
        public int IDWynikiBadanGeowloknin { get; set; }

        [StringLength(20)]
        public string NrRolki { get; set; }

        [StringLength(20)]
        public string Surowiec { get; set; }

        [StringLength(20)]
        public string KierunekBadania { get; set; }

        public string Nazwa { get; set; }

        [StringLength(20)]
        public string Gramatura { get; set; }

        public string Technologia { get; set; }

        public string Uwagi { get; set; }

        public decimal? SilaMinimalna { get; set; }

        public decimal? SilaMaksymalna { get; set; }

        public decimal? SilaSrednia { get; set; }

        public decimal? WytrzymaloscMinimalna { get; set; }

        public decimal? WytrzymaloscMaksymalna { get; set; }

        public decimal? WytrzymaloscSrednia { get; set; }

        public decimal? WydluzenieMinimalne { get; set; }

        public decimal? WydluzenieMaksymalne { get; set; }

        public decimal? WydluzenieSrednie { get; set; }

        public decimal? GramaturaMinimalna { get; set; }

        public decimal? GramaturaMaksymalna { get; set; }

        public decimal? GramaturaSrednia { get; set; }

        public string SciezkaPliku { get; set; }

        public string NazwaPliku { get; set; }

        public DateTime? DataUtworzeniaPliku { get; set; }

        public DateTime? DataModyfikacjiPliku { get; set; }

        public DateTime? DataBadania { get; set; }

        public string KodKreskowy { get; set; }

        public string BadanyWyrob { get; set; }

        public int? IDPracownikGAT { get; set; }

        public bool? CzyKalandrowana { get; set; }
        public bool? CzyBadanieZgodne { get; set; }
        public string UwagiDotyczaceWyniku { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblWynikiBadanDlaProbek> tblWynikiBadanDlaProbek { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
    }
}
