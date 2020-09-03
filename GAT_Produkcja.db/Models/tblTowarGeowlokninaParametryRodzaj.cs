using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Towar.tblTowarGeowlokninaParametryRodzaj")]
    public partial class tblTowarGeowlokninaParametryRodzaj
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTowarGeowlokninaParametryRodzaj()
        {
        }

        [Key]
        [Column("IDTowarGeowlokninaParametryRodzaj")]
        public int IDTowarGeowlokninaParametryRodzaj { get; set; }

        [Column("Rodzaj")]
        public string Rodzaj { get; set; }

        [Column("RodzajSkrot")]
        [StringLength(20)]
        public string RodzajSkrot { get; set; }
    }
}
