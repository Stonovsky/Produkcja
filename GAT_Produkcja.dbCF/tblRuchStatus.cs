namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Magazyn.tblRuchStatus")]
    public partial class tblRuchStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblRuchStatus()
        {
            tblRuchNaglowek = new HashSet<tblRuchNaglowek>();
        }

        [Key]
        public int IDRuchStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(20)]
        public string Symbol { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchNaglowek> tblRuchNaglowek { get; set; }
    }
}
