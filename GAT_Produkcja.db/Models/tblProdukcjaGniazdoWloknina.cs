using GAT_Produkcja.db.EntityValidation;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAT_Produkcja.db
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    [Table("Produkcja.tblProdukcjaGniazdoWloknina")]
    public partial class tblProdukcjaGniazdoWloknina : ValidationBase
    {
        [Key]
        [Column("IDProdukcjaGniazdoWloknina")]
        public int IDProdukcjaGniazdoWloknina { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        //[Range(1,int.MaxValue, ErrorMessage ="ID gniazda produkcyjnego musi być większe od 0")]
        [ForeignKey(nameof(tblProdukcjaGniazdoProdukcyjne))]
        public int IDProdukcjaGniazdoProdukcyjne { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        //[Range(1, int.MaxValue, ErrorMessage = "ID zlecenia produkcyjnego musi być większe od 0")]
        [ForeignKey(nameof(tblProdukcjaZlcecenieProdukcyjne))]
        public int IDProdukcjaZlecenieProdukcyjne { get; set; }

        //[Required(ErrorMessage = "Pole wymagane")]
        //[ForeignKey(nameof(tblProdukcjaGniazdoWlokninaNastawy))]
        //public int IDProdukcjaGniazdoWlokninaNastawy { get; set; }
        
        [ForeignKey(nameof(tblTowarGeowlokninaParametryGramatura))]
        public int IDGramatura { get; set; }

        public int Gramatura { get; set; }

        [Required(ErrorMessage ="Pole wymagane")]
        [Range(0.1d, 20, ErrorMessage = "Szerokość musi być większa od 10 cm")]
        public decimal Szerokosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(1d, 1000000, ErrorMessage = "Długość musi być większa od 1 m")]
        public decimal Dlugosc { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Range(0.1d, 1000000, ErrorMessage = "Waga musi być większa od 0,10 kg")] 
        public decimal Waga_Kg { get; set; }
        
        public string Uwagi { get; set; }
        
        [Required(ErrorMessage = "Pole wymagane")]
        public DateTime DataDodania { get; set; }

        [ForeignKey(nameof(tblPracownikGAT))]
        public int IDPracownikGAT { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public int NrRolki { get; set; }
        
        [Required(ErrorMessage = "Pole wymagane")]
        public string StronaRolkiWyjsciowej { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public string KodKreskowy { get; set; }

        public bool CzyParametryZgodneZTolerancjami { get; set; }
        public string UwagiDoParametrow { get; set; }

        public virtual tblProdukcjaZlecenie tblProdukcjaZlcecenieProdukcyjne { get; set; }
        public virtual tblPracownikGAT tblPracownikGAT { get; set; }
        public virtual tblProdukcjaGniazdoProdukcyjne tblProdukcjaGniazdoProdukcyjne { get; set; }
        public virtual tblTowarGeowlokninaParametryGramatura tblTowarGeowlokninaParametryGramatura { get; set; }
        //public virtual tblProdukcjaGniazdoWlokninaNastawy tblProdukcjaGniazdoWlokninaNastawy { get; set; }
    }
}
