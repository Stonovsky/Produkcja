namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Produkcja.tblProdukcjaTechnologia")]
    public partial class tblProdukcjaTechnologia
    {
        [Key]
        public int IDProdukcjaTechnologia { get; set; }

        public int? IDProdukcjaGniazdaProdukcyjne { get; set; }

        public virtual tblProdukcjaGniazdaProdukcyjne tblProdukcjaGniazdaProdukcyjne { get; set; }
    }
}
