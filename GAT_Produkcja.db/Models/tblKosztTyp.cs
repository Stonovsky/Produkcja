using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblKosztTyp")]
    public partial class tblKosztTyp
    {
        [Key]
        public int IDKosztTyp { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }
    }
}
