namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblKosztRodzaj")]
    public partial class tblKosztRodzaj
    {
        [Key]
        public int IDKosztRodzaj { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }

        public int? IDFirma { get; set; }

        [StringLength(255)]
        public string Uwagi { get; set; }
    }
}
