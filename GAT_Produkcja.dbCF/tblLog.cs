namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLog")]
    public partial class tblLog
    {
        [Key]
        public Guid ID_Log { get; set; }

        public DateTime? Czas { get; set; }

        [StringLength(255)]
        public string Uzytkownik { get; set; }

        [StringLength(255)]
        public string Aktywnosc { get; set; }
    }
}
