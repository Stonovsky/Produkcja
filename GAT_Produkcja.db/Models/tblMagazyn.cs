using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Magazyn.tblMagazyn")]
    public partial class tblMagazyn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMagazyn()
        {
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
            tblMieszanka = new HashSet<tblMieszanka>();
            tblRuchNaglowek = new HashSet<tblRuchNaglowek>();
            tblRuchNaglowek1 = new HashSet<tblRuchNaglowek>();
        }

        [Key]
        public int IDMagazyn { get; set; }

        [StringLength(20)]
        public string Symol { get; set; }

        public string Nazwa { get; set; }

        public string Opis { get; set; }

        public int? IDFirma { get; set; }

        public virtual tblFirma tblFirma { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszanka> tblMieszanka { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchNaglowek> tblRuchNaglowek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchNaglowek> tblRuchNaglowek1 { get; set; }
    }
}
