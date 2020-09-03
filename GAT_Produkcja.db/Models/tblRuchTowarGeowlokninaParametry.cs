using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Magazyn.tblRuchTowarGeowlokninaParametry")]
    public partial class tblRuchTowarGeowlokninaParametry : ValidationBase
    {
        [Key]
        [Column("IDRuchTowarGeowlokninaParametry")]
        public int IDRuchTowarParametry { get; set; }
        
        [ForeignKey(nameof(tblRuchTowar))]
        public int IDRuchTowar { get; set; }
        
        [ForeignKey(nameof(tblTowarGeowlokninaParametryRodzaj))]
        public int IDTowarGeowlokninaParametryRodzaj { get; set; }
        
        [ForeignKey(nameof(tblTowarGeowlokninaParametryGramatura))]
        public int IDTowarGeowlokninaParametryGramatura { get; set; }
        
        public decimal Szerokosc { get; set; }
        public decimal Dlugosc { get; set; }
        public decimal Waga { get; set; }
        

        public virtual tblRuchTowar tblRuchTowar { get; set; }
        public virtual tblTowarGeowlokninaParametryRodzaj tblTowarGeowlokninaParametryRodzaj { get; set; }
        public virtual tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura { get; set; }
        
    }
}
