using System;

namespace GAT_Produkcja.Utilities.ExcelParsing
{
    public class WynikiBadanGeowlokninModel
    {
        public DateTime DataBadania{ get; set; }
        public string NrRolki { get; set; }
        public string Surowiec { get; set; }
        public string KierunekBadania { get; set; }
        public string Nazwa { get; set; }
        public string Gramatura { get; set; }
        public string Technologia { get; set; }
        public string Uwagi { get; set; }
        public decimal SilaMinimalna { get; set; }
        public decimal SilaMaksymalna { get; set; }
        public decimal SilaSrednia { get; set; }
        public decimal WytrzymaloscMinimalna { get; set; }
        public decimal WytrzymaloscMaksymalna { get; set; }
        public decimal WytrzymaloscSrednia { get; set; }
        public decimal WydluzenieMinimalne { get; set; }
        public decimal WydluzenieMaksymalne { get; set; }
        public decimal WydluzenieSrednie { get; set; }
        public decimal GramaturaMinimalna { get; set; }
        public decimal GramaturaMaksymalna { get; set; }
        public decimal GramaturaSrednia { get; set; }
        public string SciezkaPliku { get; set; }
        public string NazwaPliku { get; set; }
        public DateTime DataUtworzeniaPliku { get; set; }
        public DateTime DataModyfikacjiPliku { get; set; }

    }

}
