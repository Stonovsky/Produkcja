namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Produkcja.tblProdukcjaGniazdaProdukcyjne")]
    public partial class tblProdukcjaGniazdaProdukcyjne
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblProdukcjaGniazdaProdukcyjne()
        {
            tblProdukcjaTechnologia = new HashSet<tblProdukcjaTechnologia>();
        }

        [Key]
        public int IDProdukcjaGniazdaProdukcyjne { get; set; }

        public string GniazdoNazwa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProdukcjaTechnologia> tblProdukcjaTechnologia { get; set; }
    }
}
