using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Zamowienia.tblZamowienieHandlowe")]
    public partial class tblZamowienieHandlowe : ValidationBase
    {
        [Key]
        public int IDZamowienieHandlowe { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int? IDPracownikGAT { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int? IDOdbiorca { get; set; }

        public string OsobaKontaktowa { get; set; }

        public string TelefonDoKontaktu { get; set; }

        public string UlicaDostawy { get; set; }

        public string MiastoDostawy { get; set; }

        public string KodPocztowyDostawy { get; set; }

        public string UwagiDoDostawy { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public string NrZamowienia { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public DateTime DataZamowienia { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public DateTime DataWysylki { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int IDZamowieniaPrzesylkaKoszt { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int? IDPrzewoznik { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public bool? CzyOdbiorWlasny { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int? IDZamowieniaWarunkiPlatnosci { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int? IDZamowieniaTerminPlatnosci { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblZamowienieHandlowePakowanie))]
        public int? IDZamowienieHandlowePakowanie { get; set; }
        public string NrWZ { get; set; }

        public string NrFV { get; set; }

        public string Uwagi { get; set; }
        public bool CzyZamowienieZakonczone { get; set; }
        public bool CzyZamowienieOplacone { get; set; }

        public virtual tblKontrahent tblKontrahent { get; set; }

        public virtual tblKontrahent tblKontrahent1 { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }

        public virtual tblZamowieniaPrzesylkaKoszt tblZamowieniaPrzesylkaKoszt { get; set; }

        public virtual tblZamowieniaTerminPlatnosci tblZamowieniaTerminPlatnosci { get; set; }

        public virtual tblZamowieniaWarunkiPlatnosci tblZamowieniaWarunkiPlatnosci { get; set; }

        public virtual tblZamowienieHandlowePakowanieRodzaj tblZamowienieHandlowePakowanie { get; set; }
    }
}
