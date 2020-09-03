using GAT_Produkcja.db.CustomValidationAttributes;
using GAT_Produkcja.db.EntitesInterfaces;
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

    [Table("Produkcja.tblProdukcjaZlecenie")]
    public partial class tblProdukcjaZlecenie : ValidationBase// , ItblProdukcjaZlecenieCiecia
    {
        public tblProdukcjaZlecenie()
        {
            TowaryZlecenia = new HashSet<tblProdukcjaZlecenieTowar>();
            MieszankaZlecenia = new HashSet<tblProdukcjaZlecenieProdukcyjne_Mieszanka>();
        }

        [Key]
        public virtual int IDProdukcjaZlecenie { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblPracownikGAT))]
        public virtual int IDZlecajacy { get; set; }

        [Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblPracownikGAT_Wykonujacy))]
        public virtual int? IDWykonujacy { get; set; }


        [ForeignKey(nameof(tblProdukcjaZlecenieStatus))]
        public virtual int IDProdukcjaZlecenieStatus { get; set; }

        [ForeignKey(nameof(tblKontrahent))]
        public virtual int? IDKontrahent { get; set; }/**/

        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public virtual int? IDProdukcjaGniazdoProdukcyjne { get; set; }

        public int? IDMsAccess { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public virtual int? NrZlecenia { get; set; }
        public virtual string NrDokumentu { get; set; }
        
        public virtual string KodKreskowy { get; set; }

        //[Required(ErrorMessage = "Pole wymagane")]
        public virtual string NazwaZlecenia { get; set; }

        public virtual DateTime DataUtworzenia { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [StartEndDateValidator("DataZakonczenia")]
        public virtual DateTime? DataRozpoczecia { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public virtual DateTime? DataZakonczenia { get; set; }

        public virtual DateTime? DataRozpoczeciaFakt { get; set; }
        public virtual DateTime? DataZakonczeniaFakt { get; set; }

        #region Podsumowanie mieszanki
        //[Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual decimal WartoscMieszanki_zl { get; set; }

        //[Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual decimal CenaMieszanki_zl { get; set; }

        //[Range(1, 1, ErrorMessage = "Pole musi wynosiæ 100%")]
        //[HudredPercentValidator(ErrorMessage = "Pole wymag")]
        public virtual decimal UdzialSurowcowWMieszance { get; set; } 
        #endregion

        #region Rozliczenie
        [ForeignKey(nameof(tblProdukcjaRozliczenieStatus))]
        public virtual int? IDProdukcjaRozliczenieStatus { get; set; }
        public virtual DateTime? DataRozliczenia { get; set; }
        #endregion
        public virtual string Uwagi { get; set; }
        public virtual decimal Zaawansowanie { get; set; }
        public virtual string RodzajPakowania { get; set; }


        public virtual ICollection<tblProdukcjaZlecenieTowar> TowaryZlecenia { get; set; }
        public virtual ICollection<tblProdukcjaZlecenieProdukcyjne_Mieszanka> MieszankaZlecenia { get; set; }

        public virtual tblProdukcjaRozliczenieStatus tblProdukcjaRozliczenieStatus { get; set; }
        public virtual tblProdukcjaZlecenieStatus tblProdukcjaZlecenieStatus { get; set; }
        public virtual tblPracownikGAT tblPracownikGAT{ get; set; }
        public virtual tblPracownikGAT tblPracownikGAT_Wykonujacy { get; set; }
        public virtual tblKontrahent tblKontrahent { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
    }
}
