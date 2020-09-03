using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblUrzadzenia")]
    public partial class tblUrzadzenia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblUrzadzenia()
        {
            tblZapotrzebowanie = new HashSet<tblZapotrzebowanie>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDUrzadzenia { get; set; }

        [StringLength(100)]
        public string Nazwa { get; set; }

        [StringLength(255)]
        public string Opis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZapotrzebowanie> tblZapotrzebowanie { get; set; }
    }
}
