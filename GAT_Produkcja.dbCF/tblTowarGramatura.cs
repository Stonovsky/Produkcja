namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Towar.tblTowarGramatura")]
    public partial class tblTowarGramatura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTowarGramatura()
        {
            tblZamowienieHandloweTowar = new HashSet<tblZamowienieHandloweTowar>();
        }

        [Key]
        public int IDTowarGramatura { get; set; }

        public int? Gramatura { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandloweTowar> tblZamowienieHandloweTowar { get; set; }
    }
}
