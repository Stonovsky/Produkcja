using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblPracownikGAT")]
    public partial class tblPracownikGAT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPracownikGAT()
        {
            tblWynikiBadanGeowloknin = new HashSet<tblWynikiBadanGeowloknin>();
            tblRuchNaglowek = new HashSet<tblRuchNaglowek>();
            tblZamowienieHandlowe = new HashSet<tblZamowienieHandlowe>();
            tblZapotrzebowanie = new HashSet<tblZapotrzebowanie>();
        }

        [Key]
        public int ID_PracownikGAT { get; set; }

        [Required]
        [StringLength(20)]
        public string Imie { get; set; }

        [Required]
        [StringLength(255)]
        public string Nazwisko { get; set; }

        [StringLength(255)]
        public string Stanowisko { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string TelefonKomorkowy { get; set; }

        [StringLength(255)]
        public string TelefonStacjonarny { get; set; }

        [StringLength(255)]
        public string Plec { get; set; }

        [StringLength(20)]
        public string HasloPracownika { get; set; }

        public bool CzyPracuje { get; set; }

        public int? ID_Dostep { get; set; }
        
        [DefaultValue(1)]
        public int MotywKoloru { get; set; }
        public string KodKreskowy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        [StringLength(276)]
        public string ImieINazwiskoGAT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblWynikiBadanGeowloknin> tblWynikiBadanGeowloknin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchNaglowek> tblRuchNaglowek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandlowe> tblZamowienieHandlowe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZapotrzebowanie> tblZapotrzebowanie { get; set; }
    }
}
