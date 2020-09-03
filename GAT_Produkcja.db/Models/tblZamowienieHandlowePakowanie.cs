using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]

    [Table("Zamowienia.tblZamowienieHandlowePakowanie")]
    public partial class tblZamowienieHandlowePakowanie: ValidationBase
    {
        [Key]
        [Column("IDZamowienieHandlowePakowanie")]
        public int IDZamowienieHandlowePakowanie { get; set; }


        [ForeignKey(nameof(tblZamowienieHandlowePakowanieRodzaj))]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDZamowienieHandlowePakowanieRodzaj { get; set; }


        [ForeignKey(nameof(tblZamowienieHandlowe))]
        [Required(ErrorMessage = "Pole wymagane")]
        public int IDZamowienieHandlowe { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public decimal Ilosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public decimal Dlugosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public decimal Szerokosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public decimal Wysokosc { get; set; }

        public string Uwagi { get; set; }


        public virtual tblZamowienieHandlowe tblZamowienieHandlowe { get; set; }
        public virtual tblZamowienieHandlowePakowanieRodzaj tblZamowienieHandlowePakowanieRodzaj { get; set; }
    }
}
