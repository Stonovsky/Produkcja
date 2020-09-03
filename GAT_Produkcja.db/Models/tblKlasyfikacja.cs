using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("tblKlasyfikacja")]
    public partial class tblKlasyfikacja
    {
        [Key]
        public int IDKlasyfikacja { get; set; }

        public int IDKlasyfikacjaOgolna { get; set; }

        public int IDKlasyfikacjaSzczegolowa { get; set; }

        public int? IDUrzadzenia { get; set; }
    }
}
