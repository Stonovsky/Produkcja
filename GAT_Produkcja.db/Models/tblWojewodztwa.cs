using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
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
