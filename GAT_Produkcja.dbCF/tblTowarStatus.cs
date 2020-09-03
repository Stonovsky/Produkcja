namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Towar.tblTowarStatus")]
    public partial class tblTowarStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTowarStatus()
        {
            tblCenyTransferowe = new HashSet<tblCenyTransferowe>();
            tblCenyTransferowe1 = new HashSet<tblCenyTransferowe>();
        }

        [Key]
        public int IDStatus { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCenyTransferowe> tblCenyTransferowe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCenyTransferowe> tblCenyTransferowe1 { get; set; }
    }
}
