using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblLog")]
    public partial class tblLog
    {
        [Key]
        public Guid ID_Log { get; set; }

        public DateTime? Data { get; set; }

        [StringLength(255)]
        public string Uzytkownik { get; set; }

        [StringLength(255)]
        public string Aktywnosc { get; set; }
    }
}
