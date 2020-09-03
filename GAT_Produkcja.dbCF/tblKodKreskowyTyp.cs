namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Towar.tblKodKreskowyTyp")]
    public partial class tblKodKreskowyTyp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblKodKreskowyTyp()
        {
            tblTowar = new HashSet<tblTowar>();
        }

        [Key]
        public int IDKodKreskowyTyp { get; set; }

        [StringLength(20)]
        public string KodKreskowyTyp { get; set; }

        public string Opis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblTowar> tblTowar { get; set; }
    }
}
