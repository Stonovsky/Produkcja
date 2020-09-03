using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Towar.tblTowarGrupa")]
    public partial class tblTowarGrupa : ValidationBase
    {
        [Key]
        public int IDTowarGrupa { get; set; }
        public string Grupa { get; set; }
    }
}
