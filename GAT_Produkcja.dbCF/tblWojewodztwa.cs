namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblWojewodztwa")]
    public partial class tblWojewodztwa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Identyfikator { get; set; }

        [StringLength(255)]
        public string Województwo { get; set; }
    }
}
