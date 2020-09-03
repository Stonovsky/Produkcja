namespace GAT_Produkcja.dbCF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Towar.tblTowar")]
    public partial class tblTowar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblTowar()
        {
            tblRuchTowar = new HashSet<tblRuchTowar>();
            tblMieszankaSklad = new HashSet<tblMieszankaSklad>();
            tblProdukcjaMieszanka = new HashSet<tblProdukcjaMieszanka>();
            tblZamowienieHandloweTowar = new HashSet<tblZamowienieHandloweTowar>();
        }

        [Key]
        public int IDTowar { get; set; }

        [StringLength(20)]
        public string Symbol { get; set; }

        public string Nazwa { get; set; }

        public string Opis { get; set; }

        public int? IloscMinimalna { get; set; }

        [StringLength(20)]
        public string NrKoduKreskowego { get; set; }

        public int? IDJm { get; set; }

        public int? IDKodKreskowyTyp { get; set; }

        public virtual tblJm tblJm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRuchTowar> tblRuchTowar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMieszankaSklad> tblMieszankaSklad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProdukcjaMieszanka> tblProdukcjaMieszanka { get; set; }

        public virtual tblKodKreskowyTyp tblKodKreskowyTyp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblZamowienieHandloweTowar> tblZamowienieHandloweTowar { get; set; }
    }
}
