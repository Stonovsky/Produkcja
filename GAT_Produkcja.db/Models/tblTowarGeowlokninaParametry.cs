using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Towar.tblTowarGeowlokninaParametry")]
    public partial class tblTowarGeowlokninaParametry
    {
        [Key]
        [Column("IDTowarGeowlokninaParametry")]
        public int IDTowarGeowlokninaParametry { get; set; }

        [ForeignKey(nameof(tblTowar))]
        public int? IDTowar { get; set; }

        [ForeignKey(nameof(tblTowarGeowlokninaParametryGramatura))]
        public int? IDTowarGeowlokninaParametryGramatura { get; set; }

        [ForeignKey(nameof(tblTowarGeowlokninaParametrySurowiec))]
        public int? IDTowarGeowlokninaParametrySurowiec { get; set; }

        [ForeignKey(nameof(tblCertyfikatCE))]
        [Column("IDCertyfikatCE")]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDCertyfikatCE { get; set; }

        public decimal MasaPowierzchniowa { get; set; }
        public decimal MasaPowierzchniowa_Minimum { get; set; }
        public decimal MasaPowierzchniowa_Maksimum { get; set; }

        public decimal GruboscPrzyNacisku2kPa { get; set; }
        public decimal GruboscPrzyNacisku2kPa_Minimum { get; set; }
        public decimal GruboscPrzyNacisku2kPa_Maksimum { get; set; }

        public decimal GruboscPrzyNacisku20kPa { get; set; }
        public decimal GruboscPrzyNacisku20kPa_Minimum { get; set; }
        public decimal GruboscPrzyNacisku20kPa_Maksimum { get; set; }

        public decimal GruboscPrzyNacisku200kPa { get; set; }
        public decimal GruboscPrzyNacisku200kPa_Minimum { get; set; }
        public decimal GruboscPrzyNacisku200kPa_Maksimum { get; set; }

        public decimal WytrzymaloscNaRozciaganie_MD { get; set; }
        public decimal WytrzymaloscNaRozciaganie_MD_Minimum { get; set; }

        public decimal WytrzymaloscNaRozciaganie_CMD { get; set; }
        public decimal WytrzymaloscNaRozciaganie_CMD_Minimum { get; set; }

        public decimal WydluzeniePrzyZerwaniu_MD { get; set; }
        public decimal WydluzeniePrzyZerwaniu_MD_Minimum { get; set; }
        public decimal WydluzeniePrzyZerwaniu_MD_Maksimum { get; set; }

        public decimal WydluzeniePrzyZerwaniu_CMD { get; set; }
        public decimal WydluzeniePrzyZerwaniu_CMD_Minimum { get; set; }
        public decimal WydluzeniePrzyZerwaniu_CMD_Maksimum { get; set; }

        public decimal OdpornoscNaPrzebicieStatyczne_CBR { get; set; }
        public decimal OdpornoscNaPrzebicieStatyczne_CBR_Minimum { get; set; }

        public decimal OdpornoscNaPrzebicieDynamiczne { get; set; }
        public decimal OdpornoscNaPrzebicieDynamiczne_Maksimum { get; set; }

        public decimal CharakterystycznaWielkoscPorow { get; set; }
        public decimal CharakterystycznaWielkoscPorow_Minimum { get; set; }
        public decimal CharakterystycznaWielkoscPorow_Maksimum { get; set; }

        public decimal WodoprzepuszczalnoscProsotpadla { get; set; }
        public decimal WodoprzepuszczalnoscProsotpadla_Minimum { get; set; }
        public decimal WodoprzepuszczalnoscWPlaszczyznie_20kPa{ get; set; }
        public decimal WodoprzepuszczalnoscWPlaszczyznie_20kPa_Minimum{ get; set; }
        public decimal WodoprzepuszczalnoscWPlaszczyznie_100kPa { get; set; }
        public decimal WodoprzepuszczalnoscWPlaszczyznie_100kPa_Minimum { get; set; }
        public decimal WodoprzepuszczalnoscWPlaszczyznie_200kPa { get; set; }
        public decimal WodoprzepuszczalnoscWPlaszczyznie_200kPa_Minimum { get; set; }
        public string OdpornoscNaWarunkiAtmosferyczne { get; set; }
        public int OdpornoscNaUtlenianie { get; set; }
        public bool CzyBadanieAktualne { get; set; }
        public bool CzyUV { get; set; }

        public DateTime DataBadania { get; set; }
        public string NrDWU { get; set; }

        public virtual tblTowar tblTowar { get; set; }
        public virtual tblCertyfikatCE tblCertyfikatCE { get; set; }
        public virtual tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura { get; set; }
        public  virtual tblTowarGeowlokninaParametrySurowiec tblTowarGeowlokninaParametrySurowiec { get; set; }
    }
}
