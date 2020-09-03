using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.EntitesInterfaces
{
    public interface IFinanseBank
    {
        int Id { get; set; }
        int IdFirma { get; set; }
        string Firma { get; set; }
        string Nazwa { get; set; }
        string Numer { get; set; }
        string Bank { get; set; }
        string Waluta { get; set; }
        string Opis { get; set; }
        bool CzyRachunekVAT { get; set; }
    }
}
