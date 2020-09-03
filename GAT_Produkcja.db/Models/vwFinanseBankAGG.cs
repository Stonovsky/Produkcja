using GAT_Produkcja.db.EntitesInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db
{
    [Table("AGG.vwFinanseBankAGG")]

    public class vwFinanseBankAGG : IFinanseBank
    {
        public int Id { get; set; }
        public int IdFirma { get; set; }
        public string Firma { get; set; }
        public string Nazwa { get; set; }
        public string Numer { get; set; }
        public string Bank { get; set; }
        public string Waluta { get; set; }
        public string Opis { get; set; }
        public bool CzyRachunekVAT { get; set; }
    }
}
