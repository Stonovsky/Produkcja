using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Zamowienia.tblZamowieniaTerminPlatnosci")]
    public partial class tblZamowieniaTerminPlatnosci
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblZamowieniaTerminPlatnosci()
        {
            tblZamowienieHandlowe = new HashSet<tblZamowienieHandlowe>();
        }

        [Key]
        public int IDZamowieniaTerminPlatnosci { get; set; }

        public string TerminPlatnosci { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandlowe> tblZamowienieHandlowe { get; set; }
    }
}
