using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Zamowienia.tblZamowieniaWarunkiPlatnosci")]
    public partial class tblZamowieniaWarunkiPlatnosci
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblZamowieniaWarunkiPlatnosci()
        {
            tblZamowienieHandlowe = new HashSet<tblZamowienieHandlowe>();
        }

        [Key]
        public int IDZamowieniaWarunkiPlatnosci { get; set; }

        public string WarunkiPlatnosci { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandlowe> tblZamowienieHandlowe { get; set; }
    }
}
