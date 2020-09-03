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
    [Table("Magazyn.tblRuchNaglowek")]
    public partial class tblRuchNaglowek : ValidationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblRuchNaglowek()
        {
            tblRuchTowar = new HashSet<tblRuchTowar>();
        }

        [Key]
        public int IDRuchNaglowek { get; set; }

        public DateTime DataPrzyjecia { get; set; }

        public int IDRuchStatus { get; set; }


        public int? IDMagazynZ { get; set; }

        public int? IDFirmaZ { get; set; }

        public int? IDMagazynDo { get; set; }

        public int? IDFirmaDo { get; set; }

        [ForeignKey (nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public int? IDProdukcjaZlecenieProdukcyjne { get; set; }

        [ForeignKey(nameof(tblProdukcjaGniazdaProdukcyjne))]
        public int? IDProdukcjaGniazdaProdukcyjne { get; set; }

        public int? IDKontrahent { get; set; }

        [StringLength(50)]
        public string NrDokumentuKontrahenta { get; set; }
        
        [Column("NrDokumentuPelny")]
        public string NrDokumentuPelny { get; set; } 

        [Column("NrDokumentu")]
        public int? NrDokumentu { get; set; }

        public int? ID_PracownikGAT { get; set; }

        public virtual tblFirma tblFirma { get; set; }

        public virtual tblFirma tblFirma1 { get; set; }

        public virtual tblKontrahent tblKontrahent { get; set; }

        public virtual tblPracownikGAT tblPracownikGAT { get; set; }

        public virtual tblMagazyn tblMagazyn { get; set; }

        public virtual tblMagazyn tblMagazyn1 { get; set; }

        public virtual tblRuchStatus tblRuchStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchTowar> tblRuchTowar { get; set; }
        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdaProdukcyjne { get; set; }

    }
}
