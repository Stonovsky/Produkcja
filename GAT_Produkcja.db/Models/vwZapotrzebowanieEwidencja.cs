using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [AddINotifyPropertyChangedInterface]
    [Table("Zapotrzebowanie.vwZapotrzebowanieEwidencja")]
    public partial class vwZapotrzebowanieEwidencja
    {
        [StringLength(50)]
        public string StatusZapotrzebowania { get; set; }
        public int? IDZapotrzebowanieStatus { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDZapotrzebowanie { get; set; }

        public int? Nr { get; set; }

        public DateTime? DataZgloszenia { get; set; }

        public DateTime? DataZapotrzebowania { get; set; }

        [StringLength(276)]
        public string OsobaZglZap { get; set; }
        public string OsobaOdpZaZap { get; set; }

        [StringLength(255)]
        public string Opis { get; set; }

        [StringLength(255)]
        public string ZakupOd { get; set; }

        [Column(TypeName = "money")]
        public decimal? SumaOfKoszt { get; set; }

        [StringLength(255)]
        public string KlasyfikacjaSzczegolowa { get; set; }

        public int? IDKlasyfikacjaSzczegolowa { get; set; }

        [StringLength(100)]
        public string KlasyfikacjaOgolna { get; set; }

        public int? IDKlasyfikacjaOgolna { get; set; }

        [StringLength(100)]
        public string Urzadzenie { get; set; }

        public int? IDUrzadzenia { get; set; }

        [StringLength(255)]
        public string NazwaFirmy { get; set; }
        public bool CzyZweryfikowano { get; set; }

    }
}
