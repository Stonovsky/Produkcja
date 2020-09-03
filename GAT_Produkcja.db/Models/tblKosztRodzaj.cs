using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblKosztRodzaj")]
    public partial class tblKosztRodzaj
    {
        [Key]
        public int IDKosztRodzaj { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }

        public int? IDFirma { get; set; }

        [StringLength(255)]
        public string Uwagi { get; set; }
    }
}
