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
    [AddINotifyPropertyChangedInterface]
    [Serializable]
    [Table("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka")]

    public partial class tblProdukcjaZlecenieProdukcyjne_Mieszanka : ValidationBase
    {
        [Key]
        public virtual int IDZlecenieProdukcyjneMieszanka { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public virtual int? IDProdukcjaZlecenieProdukcyjne { get; set; }
        
        //[ForeignKey(nameof(vwTowarGTX))]
        public virtual int? IDTowar { get; set; }

        public virtual int? IDMsAccess { get; set; }

        [Range(0.001d, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual decimal ZawartoscProcentowa { get; set; }

        public virtual decimal IloscKg { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblJm))]
        public virtual int IDJm { get; set; }

        public virtual string Uwagi { get; set; }

        public virtual decimal Cena_kg { get; set; }

        public virtual decimal Wartosc_kg { get; set; }

        [Range(1d, 1000000, ErrorMessage = "Pole wymagane")]
        public virtual decimal IloscMieszanki_kg { get; set; }
        public virtual decimal CenaMieszanki_kg { get; set; }
        public virtual decimal WartoscMieszanki { get; set; }

        [NotMapped]
        public virtual string NazwaTowaru { get; set; }
        [NotMapped]
        public virtual string JmNazwa { get; set; }

        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblJm tblJm { get; set; }
        //public virtual vwTowarGTX vwTowarGTX { get; set; }
    }
}
