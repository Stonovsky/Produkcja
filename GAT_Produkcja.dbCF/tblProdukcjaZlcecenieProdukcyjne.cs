namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Produkcja.tblProdukcjaZlcecenieProdukcyjne")]
    public partial class tblProdukcjaZlcecenieProdukcyjne
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblProdukcjaZlcecenieProdukcyjne()
        {
            tblProdukcjaMieszanka = new HashSet<tblProdukcjaMieszanka>();
        }

        [Key]
        public int IDProdukcjaZlcecenieProdukcyjne { get; set; }

        public int? IDTowarTyp { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscKg { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscM { get; set; }

        public DateTime? DataRozpoczecia { get; set; }

        public DateTime? DataZakonczenia { get; set; }

        public string Uwagi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProdukcjaMieszanka> tblProdukcjaMieszanka { get; set; }

        public virtual tblTowarTyp tblTowarTyp { get; set; }
    }
}
