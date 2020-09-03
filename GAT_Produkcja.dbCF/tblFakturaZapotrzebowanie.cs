namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RozliczanieFaktur.tblFakturaZapotrzebowanie")]
    public partial class tblFakturaZapotrzebowanie
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string NrZapotrzebowania { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string NrWewnetrznyZobowiazaniaSGT { get; set; }
    }
}
