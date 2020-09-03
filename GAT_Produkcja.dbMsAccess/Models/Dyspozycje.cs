using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models
{
    public class Dyspozycje: IIdEntity
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string NrZlecenia { get; set; }
        public string Artykul { get; set; }
        public decimal Ilosc_m2 { get; set; }
        public bool CzyZakonczone { get; set; }
    }
}
