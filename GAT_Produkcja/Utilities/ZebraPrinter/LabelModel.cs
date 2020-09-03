using GAT_Produkcja.db.CustomValidationAttributes;
using GAT_Produkcja.db.EntityValidation;
using GAT_Produkcja.db.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ZebraPrinter
{
    [AddINotifyPropertyChangedInterface]
    public class LabelModel : ValidationBase
    {
        public string Kontrahent { get; set; }

        public string NazwaTowaru { get; set; }

        public string TypTowaru { get; set; }

        public decimal Ilosc { get; set; }

        [Required]
        public decimal Waga_kg { get; set; }
        
        [Required]
        public string NrZP { get; set; }
        public TypKoduKreskowegoEnum TypKoduKreskowego { get; set; }

        [Required]
        public string KodKreskowy { get; set; }

        [Required]
        public int IDTowarGeowlokninaParametryRodzaj { get; set; }
        [Required]
        public string RodzajSurowca { get; set; }

        [Required]
        public int IDTowarGeowlokninaParametryGramatura { get; set; }
        
        [Required]
        public int? Gramatura { get; set; }
        
        [Required]
        public decimal DlugoscNawoju { get; set; }

        [Required]
        public decimal SzerokoscRolki { get; set; }

        [Required]
        public bool Kalandrowana { get; set; }
        
        [Required]
        public int IloscEtykietDoDruku { get; set; }
        
        [Required]
        public int IloscEtykietNaJednaSztuke { get; set; }
        public string Uwagi { get; set; }

        public GniazdaProdukcyjneEnum GniazdoProdukcyjne{ get; set; }
    }
}