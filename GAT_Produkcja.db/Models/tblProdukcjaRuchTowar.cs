using GAT_Produkcja.db.EntitesInterfaces;
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
    [Table("Produkcja.tblProdukcjaRuchTowar")]
    public partial class tblProdukcjaRuchTowar : ValidationBase, IProdukcjaRuchTowar, ItblProdukcjaRuchTowar
    {

        public tblProdukcjaRuchTowar()
        {
            tblProdukcjaRuchTowarBadania = new HashSet<tblProdukcjaRuchTowarBadania>();
        }


        [Key]
        public virtual int IDProdukcjaRuchTowar { get; set; }

        [ForeignKey(nameof(tblProdukcjaRuchTowarWyjsciowy))]
        public virtual int? IDProdukcjaRuchTowarWyjsciowy { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public virtual int? IDProdukcjaZlecenieProdukcyjne { get; set; }

        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public virtual int? IDProdukcjaGniazdoProdukcyjne { get; set; }

        [ForeignKey(nameof(tblProdukcjaRuchNaglowek))]
        public virtual int? IDProdukcjaRuchNaglowek { get; set; }

        [ForeignKey(nameof(tblProdukcjaZlecenieTowar))]
        public virtual int? IDProdukcjaZlecenieTowar { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblRuchStatus))]
        public virtual int IDRuchStatus { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblProdukcjaRozliczenieStatus))]
        public virtual int IDProdukcjaRozliczenieStatus { get; set; }

        [ForeignKey(nameof(tblProdukcjaRuchTowarStatus))]
        public virtual int? IDProdukcjaRuchTowarStatus { get; set; }
        public virtual int? IDTowar { get; set; }

        public virtual string KodKreskowy { get; set; }
        public virtual int NrRolki { get; set; }
        public virtual string NrRolkiPelny { get; set; }

        #region RolkaBazowa

        public virtual int? IDRolkaBazowa { get; set; }
        public virtual string NrRolkiBazowej { get; set; }
        public virtual string KodKreskowyRolkiBazowej { get; set; }
        public virtual string NazwaRolkiBazowej { get; set; }
        public virtual string SymbolRolkiBazowej { get; set; }

        #endregion

        #region ZleceniePodstawowe
        public virtual int? IDZleceniePodstawowe { get; set; }
        public virtual int? NrZleceniaPodstawowego { get; set; }
        #endregion

        public virtual string TowarNazwaMsAccess { get; set; }
        public virtual string TowarNazwaSubiekt { get; set; }
        public virtual string TowarSymbolSubiekt { get; set; }

        [Range(1d, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblTowarGeowlokninaParametryGramatura))]
        public virtual int IDGramatura { get; set; }

        [Range(1d, 1000000, ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblTowarGeowlokninaParametrySurowiec))]
        public virtual int IDTowarGeowlokninaParametrySurowiec { get; set; }

        public virtual string SurowiecSkrot { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public virtual int Gramatura { get; set; }

        [Range(0.01d, 4.5d, ErrorMessage = "Szer. > 0 i < 4,5")]
        public virtual decimal Szerokosc_m { get; set; }

        [Range(0.01d, 1000000, ErrorMessage = "Pole wymagane > 0")]
        public virtual decimal Dlugosc_m { get; set; }

        [Range(0.01d, 1000000, ErrorMessage = "Pole wymagane > 0")]
        public virtual decimal Waga_kg { get; set; }

        [Range(0.01d, 1000000, ErrorMessage = "Pole wymagane > 0")]
        public virtual decimal Ilosc_m2 { get; set; }

        public virtual decimal WagaOdpad_kg { get; set; }

        [DataType("decimal(18,4)")]
        public virtual decimal Cena_kg { get; set; }
        [DataType("decimal(18,4)")]
        public virtual decimal Cena_m2 { get; set; }
        [DataType("decimal(18,4)")]
        public virtual decimal Wartosc { get; set; }
        public virtual bool CzyKalandrowana { get; set; }
        public virtual bool CzyParametryZgodne { get; set; }
        public virtual string Uwagi { get; set; }
        public virtual string UwagiParametry { get; set; }
        public virtual int NrZlecenia { get; set; }
        public virtual string ZlecenieNazwa { get; set; }
        public virtual DateTime DataDodania { get; set; }
        [StringLength(20)]
        public virtual string KierunekPrzychodu { get; set; }

        public virtual int? IDMsAccess { get; set; }

        public virtual string NrDokumentu { get; set; }
        public virtual int NrPalety{ get; set; }


        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
        public virtual tblProdukcjaRuchTowar tblProdukcjaRuchTowarWyjsciowy { get; set; }
        public virtual tblProdukcjaRuchNaglowek tblProdukcjaRuchNaglowek { get; set; }
        public virtual tblRuchStatus tblRuchStatus { get; set; }
        public virtual tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura { get; set; }
        public virtual tblTowarGeowlokninaParametrySurowiec tblTowarGeowlokninaParametrySurowiec { get; set; }
        public virtual tblProdukcjaRozliczenieStatus tblProdukcjaRozliczenieStatus { get; set; }
        public virtual tblProdukcjaRuchTowarStatus tblProdukcjaRuchTowarStatus { get; set; }
        public virtual tblProdukcjaZlecenieTowar tblProdukcjaZlecenieTowar { get; set; }
        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual ICollection<tblProdukcjaRuchTowarBadania> tblProdukcjaRuchTowarBadania { get; set; }

        [NotMapped]
        public virtual int LP { get; set; }


        public override string ToString()
        {
            return $"{TowarNazwaSubiekt}, {IDProdukcjaZlecenieTowar}" ;
        }
    }
}
