namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblVAT")]
    public partial class tblVAT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblVAT()
        {
            tblRuchTowar = new HashSet<tblRuchTowar>();
        }

        [Key]
        public int IDVAT { get; set; }

        public double? VAT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchTowar> tblRuchTowar { get; set; }
    }
}
