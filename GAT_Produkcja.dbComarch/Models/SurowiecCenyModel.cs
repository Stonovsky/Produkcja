using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbComarch.Models
{
    public class SurowiecCenyModel
    {
        public int Id { get; set; }
        public int MagazynID { get; set; }
        public string MagazynNazwa { get; set; }
        public string Kod { get; set; }
        public string Nazwa { get; set; }
        public decimal Ilosc { get; set; }
        public string Jm { get; set; }
        public decimal CenaJedn { get; set; }
        public decimal Wartosc { get; set; }
        public string Operator { get; set; }
        public DateTime Data { get; set; }
    }
}
