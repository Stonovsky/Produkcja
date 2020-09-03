namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPliki")]
    public partial class tblPliki
    {
        [Key]
        public int IDPlik { get; set; }

        [StringLength(100)]
        public string NazwaPliku { get; set; }

        [StringLength(256)]
        public string SciezkaPliku { get; set; }

        public int? IDZapotrzebowanie { get; set; }

        public string NrZP { get; set; }

        [StringLength(100)]
        public string NrFvKlienta { get; set; }

        public string SciezkaLokalnaPliku { get; set; }

        public virtual tblZapotrzebowanie tblZapotrzebowanie { get; set; }
    }
}
