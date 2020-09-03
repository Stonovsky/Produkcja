using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.RozliczenieMsAccess.Modele
{
    public class PodsumowanieRozliczenieMsAccessModel
    {
        public decimal WagaKg { get; set; }
        public decimal OdpadKg { get; set; }
        public decimal IloscM2{ get; set; }
        public int IloscPozycji { get; set; }
    }
}
