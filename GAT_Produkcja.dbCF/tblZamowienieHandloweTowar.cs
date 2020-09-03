namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zamowienia.tblZamowienieHandloweTowar")]
    public partial class tblZamowienieHandloweTowar
    {
        [Key]
        public int IDZamowienieHandloweTowar { get; set; }

        public int? IDTowar { get; set; }

        public int? IDTowarGramatura { get; set; }

        [Column(TypeName = "money")]
        public decimal? SzerokoscRolki { get; set; }

        [Column(TypeName = "money")]
        public decimal? DlugoscNawoju { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscRolek { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscSumaryczna { get; set; }

        public virtual tblTowar tblTowar { get; set; }

        public virtual tblTowarGramatura tblTowarGramatura { get; set; }
    }
}
