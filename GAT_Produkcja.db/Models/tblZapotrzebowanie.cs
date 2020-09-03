using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    
    [AddINotifyPropertyChangedInterface]
    [Table("tblZapotrzebowanie")]
    public partial class tblZapotrzebowanie: ValidationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblZapotrzebowanie()
        {
            tblPliki = new HashSet<tblPliki>();
            tblZapotrzebowaniePozycje = new HashSet<tblZapotrzebowaniePozycje>();
        }

        [Key]
        public int IDZapotrzebowanie { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int Nr { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int IDKontrahent { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int IDPracownikGAT { get; set; }

        [ForeignKey(nameof(PracownikOdpZaZakup))]
        public int? IDPracownikOdpZaZakup { get; set; }
        
        public DateTime? DataZgloszenia { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public DateTime DataZapotrzebowania { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [StringLength(255)]
        public string Opis { get; set; }

        public int? IDKlasyfikacjaOgolna { get; set; }

        public int? IDKlasyfikacjaSzczegolowa { get; set; }

        public int? IDUrzadzenia { get; set; }

        [StringLength(255)]
        public string Uwagi { get; set; }
        [ForeignKey(nameof(tblZapotrzebowanieStatus))]
        public int? IDZapotrzebowanieStatus { get; set; }

        public int? IDFirma { get; set; }
        public bool CzyZweryfikowano { get; set; }

        public virtual tblKlasyfikacjaOgolna tblKlasyfikacjaOgolna { get; set; }

        public virtual tblKlasyfikacjaSzczegolowa tblKlasyfikacjaSzczegolowa { get; set; }

        public virtual tblKontrahent tblKontrahent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPliki> tblPliki { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
        public virtual tblPracownikGAT PracownikOdpZaZakup { get; set; }

        public virtual tblUrzadzenia tblUrzadzenia { get; set; }

        public virtual tblZapotrzebowanieStatus tblZapotrzebowanieStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZapotrzebowaniePozycje> tblZapotrzebowaniePozycje { get; set; }
    }
}
