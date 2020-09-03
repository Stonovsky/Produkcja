using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Magazyn.tblDokumentTyp")]

    public partial class tblDokumentTyp
    {
        [Key]
        public int IDDokumentTyp { get; set; }
        public string DokumentTyp { get; set; }
        [StringLength(20)]
        public string DokumentTypSkrot { get; set; }
    }
}
