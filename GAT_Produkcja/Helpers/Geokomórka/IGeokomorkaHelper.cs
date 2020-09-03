using System.Collections.Generic;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.Helpers.Geokomórka
{
    public interface IGeokomorkaHelper
    {
        List<tblTowarGeokomorkaParametryGeometryczne> ListaParametrowGeometrycznych { get; set; }
        List<tblTowarGeokomorkaParametryRodzaj> ListaRodzajow { get; set; }
        List<tblTowarGeokomorkaParametryTyp> ListaTypow { get; set; }
        List<tblTowarGeokomorkaParametryZgrzew> ListaZgrzewow { get; set; }

        string GenerujNazwePelna(string nazwa, decimal ilosc_m2);
        string GenerujNazwePelna(string typ, string rodzaj, string kodZgrzewu, int zgrzew, int wysokosc, int szerokoscSekcji, int dlugoscSekcji, decimal ilosc_m2);
        decimal ObliczIloscM2ZgodnaZPowierzchniaSekcji(string nazwa, decimal ilosc_m2);
        decimal ObliczIloscM2ZgodnaZPowierzchniaSekcji(string kodZgrzewu, decimal szerokoscSekcji_m, decimal dlugoscSekcji_m, decimal ilosc_m2);
        int ObliczIloscSekcji(string nazwa, decimal ilosc_m);
        int ObliczIloscSekcji(string kodZgrzewu, decimal szerokoscSekcji_m, decimal dlugoscSekcji_m, decimal ilosc_m2);
        decimal ObliczWage(int zgrzew, string typ, int wysokosc, decimal ilosc_m2);
        decimal ObliczWage(string nazwa, decimal ilosc_m2);
        Task PobierzListy();
        tblTowarGeokomorkaParametryRodzaj PobierzRodzajZNazwy(string nazwa);
        decimal PobierzStandardowaDlugoscSekcjiZNazwy_m(string nazwa);
        decimal PobierzStandardowaSzerokoscSekcjiZNazwy_m(string nazwa);
        tblTowarGeokomorkaParametryTyp PobierzTypZNazwy(string nazwa);
        decimal PobierzWysokoscZNazwy(string nazwa);
        tblTowarGeokomorkaParametryZgrzew PobierzZgrzewZNazwy(string nazwa);
        tblTowarGeokomorkaParametryGeometryczne PobierzParametryGeometryczneZeZgrzewu(string kodZgrzewu);
        Task<tblTowar> PobierzTowarAsync(tblTowarGeokomorkaParametryRodzaj rodzaj,
                                            tblTowarGeokomorkaParametryTyp typ,
                                            tblTowarGeokomorkaParametryZgrzew zgrzew,
                                            int wysokosc);
    }
}