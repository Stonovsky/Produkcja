using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models
{
    public class NormyZuzycia : IIdEntity
    {
        public int Id { get; set; }
        public int ZlecenieID { get; set; }
        public string Zlecenie { get; set; }
        /// <summary>
        /// Jaki towar bedzie produkowany ze zlecenia - dyspozycji
        /// </summary>
        public string Artykul { get; set; }
        public int Dostawca { get; set; }
        /// <summary>
        /// Nazwa surowca
        /// </summary>
        public string Surowiec { get; set; }
        /// <summary>
        /// Udzial procentowy w mieszance
        /// </summary>
        public decimal Ilosc { get; set; }

    }
}
