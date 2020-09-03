using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaRuchTowarBadania")]
    public partial class tblProdukcjaRuchTowarBadania : ValidationBase
    {
        [Key]
        public int IDProdukcjaRuchTowarBadania { get; set; }

        [ForeignKey(nameof(tblProdukcjaRuchTowar))]
        public int IDProdukcjaRuchTowar { get; set; }

        public int Gramatura_1 { get; set; }
        public int Gramatura_2 { get; set; }
        public int Gramatura_3 { get; set; }
        public decimal GramaturaSrednia { get; set; }
        public bool CzySredniaGramaturaWTolerancjach { get; set; }
        public string UwagiGramatura { get; set; }

        public tblProdukcjaRuchTowar tblProdukcjaRuchTowar { get; set; }

    }
}
