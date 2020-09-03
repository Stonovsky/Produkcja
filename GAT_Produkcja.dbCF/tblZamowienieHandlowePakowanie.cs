namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zamowienia.tblZamowienieHandlowePakowanie")]
    public partial class tblZamowienieHandlowePakowanie
    {
        [Key]
        public int IDZamowienieHandlowePakowanie { get; set; }

        public string RodzajPakowania { get; set; }
    }
}
