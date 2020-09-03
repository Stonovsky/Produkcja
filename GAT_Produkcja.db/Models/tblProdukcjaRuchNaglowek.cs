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
    [Table("Produkcja.tblProdukcjaRuchNaglowek")]
    public partial class tblProdukcjaRuchNaglowek : ValidationBase
    {
        public tblProdukcjaRuchNaglowek()
        {
            Towary = new HashSet<tblProdukcjaRuchTowar>();
        }
        [Key]
        public int IDProdukcjaRuchNaglowek { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(1,int.MaxValue,ErrorMessage ="Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public int? IDProdukcjaZlecenieProdukcyjne { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlecenieCiecia))]
        public int? IDProdukcjaZlecenieCiecia { get; set; }
        
        [ForeignKey(nameof(tblProdukcjaZlecenieTowar))]
        public int? IDProdukcjaZlecenieTowar { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(1,int.MaxValue,ErrorMessage ="Pole wymagane")]
        [ForeignKey(nameof(tblPracownikGAT))]
        public int IDPracownikGAT { get; set; }

        public int? IDPracownikGAT1 { get; set; }

        [ForeignKey(nameof(tblMagazyn))]
        public int IDMagazyn { get; set; }

        public DateTime DataDodania { get; set; }
        
        public string Uwagi { get; set; }

        public virtual ICollection<tblProdukcjaRuchTowar> Towary { get; set; }

        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblProdukcjaZlecenieCiecia tblProdukcjaZlecenieCiecia { get; set; }
        public virtual tblProdukcjaZlecenieTowar tblProdukcjaZlecenieTowar{ get; set; }
        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
        public virtual tblMagazyn tblMagazyn { get; set; }
    }
}
