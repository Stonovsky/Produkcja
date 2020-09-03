using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models
{
    public class Produkcja : IIdEntity, IGniazdoProdukcyjne
    {
        private decimal iloscM2;
        private decimal szerokosc;

        public int Id { get; set; }
        public int ZlecenieID { get; set; }
        public string Zlecenie { get; set; }
        public string NrSztuki { get; set; }
        public DateTime Data { get; set; }
        public DateTime Godzina { get; set; }
        public string Artykul { get; set; }
        public int OperatorID { get; set; }
        public int PomocOperatoraID { get; set; }
        public decimal Szerokosc
        {
            get => szerokosc / 100;
            set => szerokosc = value;
        }
        public decimal Dlugosc { get; set; }
        public decimal Waga { get; set; }
        public int Gramatura1 { get; set; }
        public int Gramatura2 { get; set; }
        public int Gramatura3 { get; set; }
        public decimal WagaOdpadu { get; set; }
        public decimal Postoj { get; set; }
        public bool Odpad { get; set; }
        public bool CzyProduktKalandowany { get; set; }
        public decimal IloscM2
        {
            get { return Szerokosc * Dlugosc; }
            set => iloscM2 = value;
        }

    }
}
