using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Towar.tblTowarGeowlokninaParametryGramatura")]
    public partial class tblTowarGeowlokninaParametryGramatura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTowarGeowlokninaParametryGramatura()
        {
            tblZamowienieHandloweTowarGeowloknina = new HashSet<tblZamowienieHandloweTowarGeowloknina>();
        }
        [Column("IDTowarGeowlokninaParametryGramatura")]
        [Key]
        public int IDTowarGeowlokninaParametryGramatura { get; set; }

        public int? Gramatura { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandloweTowarGeowloknina> tblZamowienieHandloweTowarGeowloknina { get; set; }
    }
}
