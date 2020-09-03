namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Towar.tblTowarParametry")]
    public partial class tblTowarParametry
    {
        [Key]
        public int IDTowarParametry { get; set; }

        [Required]
        public string Nazwa { get; set; }

        public int? IDJm { get; set; }

        public virtual tblJm tblJm { get; set; }
    }
}
