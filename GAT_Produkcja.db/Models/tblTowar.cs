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
    [Table("Towar.tblTowar")]
    public partial class tblTowar: ValidationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTowar()
        {
            tblRuchTowar = new HashSet<tblRuchTowar>();
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
            tblProdukcjaMieszanka = new HashSet<tblProdukcjaZlecenieProdukcyjneMieszanka>();
            tblZamowienieHandloweTowar = new HashSet<tblZamowienieHandloweTowarGeowloknina>();
        }

        [Key]
        public int IDTowar { get; set; }

        [ForeignKey (nameof(tblTowarGrupa))]
        [Column("IDTowarGrupa")]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDTowarGrupa { get; set; }

        [StringLength(20)]
        public string Symbol { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public string Nazwa { get; set; }
        public string NazwaWlasna { get; set; }
        public string Opis { get; set; }

        public int? IloscMinimalna { get; set; }

        [StringLength(20)]
        public string NrKoduKreskowego { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int? IDJm { get; set; }

        public int? IDKodKreskowyTyp { get; set; }

        public virtual tblJm tblJm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchTowar> tblRuchTowar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProdukcjaZlecenieProdukcyjneMieszanka> tblProdukcjaMieszanka { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandloweTowarGeowloknina> tblZamowienieHandloweTowar { get; set; }
        
        public virtual tblKodKreskowyTyp tblKodKreskowyTyp { get; set; }
        public virtual tblTowarGrupa tblTowarGrupa { get; set; }
    }
}
