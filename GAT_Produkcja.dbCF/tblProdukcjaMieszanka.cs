namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Produkcja.tblProdukcjaMieszanka")]
    public partial class tblProdukcjaMieszanka
    {
        [Key]
        public int IDMieszanka { get; set; }

        public int? IDZlecenieProdukcyjne { get; set; }

        public int? IDTowar { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ilosc { get; set; }

        [Column(TypeName = "money")]
        public decimal? ZawartoscProcentowa { get; set; }

        public virtual tblTowar tblTowar { get; set; }

        public virtual tblProdukcjaZlcecenieProdukcyjne tblProdukcjaZlcecenieProdukcyjne { get; set; }
    }
}
