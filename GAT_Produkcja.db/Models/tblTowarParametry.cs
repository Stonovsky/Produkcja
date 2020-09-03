namespace GAT_Produkcja.db
{
    using PropertyChanged;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    [AddINotifyPropertyChangedInterface]
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
