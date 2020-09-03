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
    [Table("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjne")]

    public partial class tblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjne : ValidationBase
    {

        [Key]
        [Column(Order = 0)]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDProdukcjaZlecenieProdukcyjne { get; set; }
        
        public DateTime DataRozpoczecia { get; set; }
        public DateTime DataZakonczenia { get; set; }
        public string Uwagi { get; set; }


    }
}
