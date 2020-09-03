using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaGniazdoProdukcyjne")]
    public partial class tblProdukcjaGniazdoProdukcyjne : ValidationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblProdukcjaGniazdoProdukcyjne()
        {
            tblProdukcjaTechnologia = new HashSet<tblProdukcjaTechnologia>();
        }

        [Key]
        [Column("IDProdukcjaGniazdoProdukcyjne")]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }


        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblTowarGrupa))]
        public int? IDTowarGrupa { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        public string GniazdoNazwa { get; set; }
        public string GniazdoKodKreskowy { get; set; }

        /// <summary>
        /// Wlasciwosc uzywana w rozchodzie rolki do zmniejszenia masy koncowej z uwagi na odparowanie wody na gniezdzie
        /// </summary>
        public decimal WspZmniejszeniaMasy { get; set; }
        public string Opis { get; set; }
        public string Uwagi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProdukcjaTechnologia> tblProdukcjaTechnologia { get; set; }
        public virtual tblTowarGrupa tblTowarGrupa { get; set; }
    }
}
