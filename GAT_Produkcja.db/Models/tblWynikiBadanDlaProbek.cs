using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Badania.tblWynikiBadanDlaProbek")]
    public partial class tblWynikiBadanDlaProbek
    {
        [Key]
        public int IDWynikBadaniaDlaProbki { get; set; }

        public int? IDWynikiBadanGeowloknin { get; set; }

        public decimal? Sila { get; set; }

        public decimal? Wytrzymalosc { get; set; }

        public decimal? WydluzenieCalkowite { get; set; }

        public decimal? Gramatura { get; set; }

        public string PlikZrodlowyNazwa { get; set; }

        public DateTime? DataBadania { get; set; }

        public DateTime? DataDodania { get; set; }

        public int? NrProbki { get; set; }

        public virtual tblWynikiBadanGeowloknin tblWynikiBadanGeowloknin { get; set; }
    }
}
