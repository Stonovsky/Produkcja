using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Towar.tblTowarGeowlokninaParametrySurowiec")]
    public partial class tblTowarGeowlokninaParametrySurowiec
    {
        public tblTowarGeowlokninaParametrySurowiec()
        {
        }

        [Key]
        [Column("IDTowarGeowlokninaParametrySurowiec")]
        public int IDTowarGeowlokninaParametrySurowiec { get; set; }

        public string Nazwa{ get; set; }

        [StringLength(20)]
        public string Skrot{ get; set; }

    }
}
