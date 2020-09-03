namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Towar.tblTowarTyp")]
    public partial class tblTowarTyp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTowarTyp()
        {
            tblProdukcjaZlcecenieProdukcyjne = new HashSet<tblProdukcjaZlcecenieProdukcyjne>();
        }

        [Key]
        public int IDTowarTyp { get; set; }

        public string TowarTyp { get; set; }

        [StringLength(20)]
        public string TowarTypSkrot { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProdukcjaZlcecenieProdukcyjne> tblProdukcjaZlcecenieProdukcyjne { get; set; }
    }
}
