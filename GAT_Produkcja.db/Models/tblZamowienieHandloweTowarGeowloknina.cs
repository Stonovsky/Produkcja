using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Zamowienia.tblZamowienieHandloweTowarGeowloknina")]
    public partial class tblZamowienieHandloweTowarGeowloknina : ValidationBase
    {
        [Column("IDZamowienieHandloweTowarGeowloknina")]
        [Key]
        public int IDZamowienieHandloweTowarGeowloknina { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblZamowienieHandlowe))]
        public int IDZamowienieHandlowe { get; set; }

        //[Column("IDTowar")]
        [ForeignKey(nameof(tblTowar))]
        public int IDTowar { get; set; }

        [ForeignKey(nameof(tblTowarGeowlokninaParametryRodzaj))]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDTowarGeowlokninaParametryRodzaj { get; set; }

        [Column("IDTowarGeowlokninaParametryGramatura")]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDTowarGeowlokninaParametryGramatura { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Column(TypeName = "money")]
        public decimal? SzerokoscRolki { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Column(TypeName = "money")]
        public decimal? DlugoscNawoju { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Column(TypeName = "money")]
        public decimal? IloscRolek { get; set; }

        [Column(TypeName = "money")]
        public decimal? IloscSumaryczna { get; set; }
        public string NazwaPelna { get; set; }
        public bool CzyTowarNiestandardowy { get; set; }
        public decimal Waga_kg { get; set; }


        //public virtual tblTowarRodzaj tblTowarRodzaj { get; set; }
        public virtual tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura { get; set; }
        public virtual tblZamowienieHandlowe tblZamowienieHandlowe { get; set; }
        public virtual tblTowarGeowlokninaParametryRodzaj tblTowarGeowlokninaParametryRodzaj { get; set; }
        public virtual tblTowar tblTowar { get; set; }

    }
}
