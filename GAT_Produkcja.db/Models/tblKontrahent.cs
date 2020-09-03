using GAT_Produkcja.db.CustomValidationAttributes;
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
    [Table("tblKontrahent")]
    public partial class tblKontrahent : ValidationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblKontrahent()
        {
            tblRuchNaglowek = new HashSet<tblRuchNaglowek>();
            tblZamowienieHandlowe = new HashSet<tblZamowienieHandlowe>();
            tblZapotrzebowanie = new HashSet<tblZapotrzebowanie>();
        }

        [Key]
        public int ID_Kontrahent { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [StringLength(255)]
        public string Nazwa { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        //[NipValidator]
        [StringLength(20, MinimumLength = 5)]
        public string NIP { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [StringLength(100)]
        public string Ulica { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [StringLength(50)]
        public string Miasto { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [StringLength(10)]
        public string KodPocztowy { get; set; }

        [StringLength(50)]
        public string Wojewodztwo { get; set; }

        [Column(TypeName = "money")]
        public decimal? LimitKredytowy { get; set; }

        [StringLength(50)]
        public string Kraj { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(255)]
        public string InformacjeDodatkowe { get; set; }

        [StringLength(50)]
        public string StronaInternetowa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchNaglowek> tblRuchNaglowek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandlowe> tblZamowienieHandlowe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandlowe> tblZamowienieHandlowe1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZapotrzebowanie> tblZapotrzebowanie { get; set; }

        public override string ToString()
        {
            return $"{Nazwa}, Ul. {Ulica}, {KodPocztowy} {Miasto}, NIP: {NIP}";
        }
    }
}
