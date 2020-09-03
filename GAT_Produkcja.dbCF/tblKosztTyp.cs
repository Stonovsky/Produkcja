namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblKosztTyp")]
    public partial class tblKosztTyp
    {
        [Key]
        public int IDKosztTyp { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }
    }
}
