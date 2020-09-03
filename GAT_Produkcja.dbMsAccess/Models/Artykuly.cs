using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models
{
    public class Artykuly : IIdEntity
    {
        public int Id { get; set; }
        public string NazwaArtykulu { get; set; }
        public string Opis { get; set; }
    }
}
