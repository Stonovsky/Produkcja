using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models
{
    public class Surowiec : IIdEntity
    {
        public int Id { get; set; }
        public int LP { get; set; }
        public string NazwaSurowca { get; set; }
    }
}
