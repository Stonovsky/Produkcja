using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters
{
    public class RozliczeniePWAdapter : IProdukcjaRozliczenie_PW

    {
        private readonly Konfekcja konfekcja;

        public RozliczeniePWAdapter(Konfekcja konfekcja)
        {
            this.konfekcja = konfekcja;
        }
        public string NazwaTowaruSubiekt => null;
        public string SymbolTowaruSubiekt => null;
        public decimal Ilosc => konfekcja.Waga;
        public decimal CenaJednostkowa => 0;
        public decimal Wartosc => 0;
        public int? IDJm => (int)JmEnum.kg;
        public string Jm => "kg";
        public int IDZlecenie => konfekcja.ZlecenieID;
        public string NrZlecenia => konfekcja.Zlecenie;

        public string NazwaTowaru => konfekcja.Artykul;

        public string NrWz => konfekcja.NrWz;

        public string NrRolki => konfekcja.NumerMG;

        public decimal Odpad_kg => konfekcja.WagaOdpadu;
    }
}
