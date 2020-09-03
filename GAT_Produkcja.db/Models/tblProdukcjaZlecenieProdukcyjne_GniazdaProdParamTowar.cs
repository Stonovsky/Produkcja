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
    [Table("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar")]
    public partial class tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar : ValidationBase
    {
        [Key]
        public int IDProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar { get; set; }
        
        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public int IDProdukcjaZlcecenieProdukcyjne { get; set; }

        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }

        [ForeignKey(nameof(tblTowarGeowlokninaParametryGramatura))]
        public int? IDTowarGeowlokninaParametryGramatura { get; set; }

        [ForeignKey(nameof(tblTowarGeowlokninaParametrySurowiec))]
        public int? IDTowarGeowlokninaParametrySurowiec { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(1d, 1000000, ErrorMessage = "Pole wymagane")]
        public decimal Szerokosc_m { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(1d, 1000000, ErrorMessage = "Pole wymagane")]
        public decimal Dlugosc_m { get; set; }

        public decimal Ilosc_kg { get; set; }

        public decimal Ilosc_m2 { get; set; }

        public decimal Ilosc_szt { get; set; }

        public bool CzyKalandorwana { get; set; }
        public bool CzyUV { get; set; }
        public string Uwagi { get; set; }


        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblTowarGeowlokninaParametrySurowiec tblTowarGeowlokninaParametrySurowiec { get; set; }
        public virtual tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
    }
}
