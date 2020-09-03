using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Zamowienia.tblZamowieniaPrzesylkaKoszt")]
    public partial class tblZamowieniaPrzesylkaKoszt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblZamowieniaPrzesylkaKoszt()
        {
            tblZamowienieHandlowe = new HashSet<tblZamowienieHandlowe>();
        }

        [Key]
        public int IDZamowieniaPrzesylkaKoszt { get; set; }

        public string PrzesylkaKoszt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandlowe> tblZamowienieHandlowe { get; set; }
    }
}
