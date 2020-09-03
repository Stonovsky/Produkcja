using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblFirma")]
    public partial class tblFirma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblFirma()
        {
            tblMagazyn = new HashSet<tblMagazyn>();
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
            tblMieszanka = new HashSet<tblMieszanka>();
            tblRuchNaglowek = new HashSet<tblRuchNaglowek>();
            tblRuchNaglowek1 = new HashSet<tblRuchNaglowek>();
        }

        [Key]
        public int IDFirma { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }

        [StringLength(255)]
        public string Ulica { get; set; }

        [StringLength(255)]
        public string Miasto { get; set; }

        [StringLength(255)]
        public string KodPocztowy { get; set; }

        public string NIP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMagazyn> tblMagazyn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszanka> tblMieszanka { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchNaglowek> tblRuchNaglowek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchNaglowek> tblRuchNaglowek1 { get; set; }

        public override string ToString()
        {
            return $"{Nazwa}";
        }
    }
}
