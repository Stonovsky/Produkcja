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
    [Table("Produkcja.tblProdukcjaGniazdoWlokninaBadania")]
    public partial class tblProdukcjaGniazdoWlokninaBadania : ValidationBase
    {
        [Key]
        public int IDProdukcjaGniazdoWlokninaBadania { get; set; }

        [ForeignKey(nameof(tblProdukcjaRuchTowar))]
        public int IDProdukcjaRuchTowar { get; set; }
        
        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }

        public int? Gramatura_1 { get; set; }
        public int? Gramatura_2 { get; set; }
        public int? Gramatura_3 { get; set; }
        public decimal GramaturaSrednia { get; set; }
        public bool CzySrenidaGramaturaWTolerancjach { get; set; }
        public string Uwagi { get; set; }

        public virtual tblProdukcjaGniazdoWloknina tblProdukcjaGniazdoWloknina { get; set; }
        public virtual tblProdukcjaRuchTowar tblProdukcjaRuchTowar { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }

    }
}
