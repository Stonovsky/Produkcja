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
    [Table("Konfiguracja.tblKonfiguracjaUrzadzen")]
    public class tblKonfiguracjaUrzadzen : ValidationBase
    {
        [Key]
        public int IDKonfiguracjaUrzadzen { get; set; }
        public string NazwaKomputera { get; set; }
        public string DrukarkaNazwa { get; set; }
        public string DrukarkaIP { get; set; }
        public string WagaComPort { get; set; }
        public DateTime DataDodania { get; set; }

        [ForeignKey(nameof(tblPracownikGAT))]
        public int? IDOperator { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
    }
}
