using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Zamowienia.tblZamowienieHandloweTowarInny")]
    public partial class tblZamowienieHandloweTowarInny : ValidationBase
    {
        [Column("IDZamowienieHandloweTowarInny")]
        [Key]
        public int IDZamowienieHandloweTowarInny { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [ForeignKey(nameof(tblZamowienieHandlowe))]
        public int IDZamowienieHandlowe { get; set; }

        //[Column("IDTowar")]
        [ForeignKey(nameof(tblTowar))]
        public int IDTowar { get; set; }


        [Column(TypeName = "money")]
        public decimal? IloscSumaryczna { get; set; }
        
        [ForeignKey(nameof(tblJm))]
        public int IDJm { get; set; }
        public string NazwaPelna { get; set; }
        public decimal Waga_kg { get; set; }


        public virtual tblZamowienieHandlowe tblZamowienieHandlowe { get; set; }
        public virtual tblTowar tblTowar { get; set; }
        public virtual tblJm tblJm { get; set; }

    }
}
