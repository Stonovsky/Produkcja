using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("RozliczanieFaktur.tblFakturaZapotrzebowanie")]
    public partial class tblFakturaZapotrzebowanie
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string NrZapotrzebowania { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string NrWewnetrznyZobowiazaniaSGT { get; set; }
    }
}
