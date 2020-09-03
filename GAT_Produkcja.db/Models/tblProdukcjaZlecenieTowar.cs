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
    [Table("Produkcja.tblProdukcjaZlecenieTowar")]

    public partial class tblProdukcjaZlecenieTowar : ValidationBase
    {
        [Key]
        [Column("IDProdukcjaZlecenieTowar")]
        public virtual int IDProdukcjaZlecenieTowar{ get; set; }

        [ForeignKey(nameof(tblProdukcjaZlecenieCiecia))]
        public virtual int? IDProdukcjaZlecenieCiecia { get; set; }

        [Column("IDProdukcjaZlecenie")]
        [ForeignKey(nameof(tblProdukcjaZlecenie))]
        public virtual int? IDProdukcjaZlecenie { get; set; }

        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public virtual int? IDProdukcjaGniazdoProdukcyjne { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlecenieStatus))]
        public virtual int? IDProdukcjaZlecenieStatus { get; set; }

        //[ForeignKey(nameof(tblTowar))]
        public virtual int? IDTowar { get; set; }

        [NotMapped]
        public virtual string TowarNazwa { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblTowarGeowlokninaParametryGramatura))]
        public virtual int IDTowarGeowlokninaParametryGramatura { get; set; }

        [NotMapped]
        public virtual int? Gramatura { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblTowarGeowlokninaParametrySurowiec))]
        public virtual int IDTowarGeowlokninaParametrySurowiec { get; set; }

        [NotMapped]
        public virtual string Surowiec { get; set; }

        [Range(0.01d, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual decimal Szerokosc_m { get; set; }

        [Range(0.01d, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual decimal Dlugosc_m { get; set; }
        public virtual bool CzyWielokrotnoscDlugosci{ get; set; } 

        //[Range(0.01d, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual int Ilosc_szt { get; set; }

        [Range(0.01d, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual decimal Ilosc_m2 { get; set; }

        public virtual decimal? Ilosc_kg { get; set; }
        public virtual decimal? IloscZmian { get; set; }

        public virtual bool CzyUv { get; set; }
        
        public virtual bool CzyKalandrowana { get; set; } 

        public virtual string RodzajPakowania { get; set; }
        public virtual string Uwagi { get; set; }
        public virtual decimal Zaawansowanie { get; set; }

        public virtual tblProdukcjaZlecenieCiecia tblProdukcjaZlecenieCiecia { get; set; }
        public virtual tblProdukcjaZlecenie tblProdukcjaZlecenie { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }

        //public virtual tblTowar tblTowar { get; set; }
        public virtual tblTowarGeowlokninaParametrySurowiec tblTowarGeowlokninaParametrySurowiec { get; set; }
        public virtual tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura { get; set; }
        public virtual tblProdukcjaZlecenieStatus tblProdukcjaZlecenieStatus { get; set; }
    }
}
