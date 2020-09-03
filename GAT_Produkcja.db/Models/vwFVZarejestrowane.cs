namespace GAT_Produkcja.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vwFVZarejestrowane")]
    public partial class vwFVZarejestrowane
    {
        [Key]
        public int IDZapotrzebowanie { get; set; }

        public int? NrZapotrzebowania { get; set; }

        public string StatusFV { get; set; }
    }
}
