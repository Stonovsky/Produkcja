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
    [Table("Towar.tblCertyfikatCE")]
    public partial class tblCertyfikatCE
    {
        [Key]
        public int IDCertyfikatCE { get; set; }

        [ForeignKey(nameof(tblFirma))]
        [Column("IDFirma")]
        public int IDFirma { get; set; }
        public DateTime DataWydaniaCertyfikatu { get; set; }
        public string NumerCertyfikatu { get; set; }
        public string JednostkaCertyfikujaca { get; set; }
        public string DotyczyWyrobuBudowlanego { get; set; }
        public bool CzyAktualny { get; set; }
        public virtual tblFirma tblFirma { get; set; }

    }
}