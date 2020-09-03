namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblKlasyfikacja")]
    public partial class tblKlasyfikacja
    {
        [Key]
        public int IDKlasyfikacja { get; set; }

        public int IDKlasyfikacjaOgolna { get; set; }

        public int IDKlasyfikacjaSzczegolowa { get; set; }

        public int? IDUrzadzenia { get; set; }
    }
}
