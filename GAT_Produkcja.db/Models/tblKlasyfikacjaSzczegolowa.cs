using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblKlasyfikacjaSzczegolowa")]
    public partial class tblKlasyfikacjaSzczegolowa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblKlasyfikacjaSzczegolowa()
        {
            tblZapotrzebowanie = new HashSet<tblZapotrzebowanie>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDKlasyfikacjaSzczegolowa { get; set; }

        [Required]
        [StringLength(255)]
        public string Nazwa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZapotrzebowanie> tblZapotrzebowanie { get; set; }
    }
}
