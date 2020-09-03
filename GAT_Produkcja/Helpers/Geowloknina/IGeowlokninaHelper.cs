using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.Helpers.Geowloknina
{
    public interface IGeowlokninaHelper
    {
        decimal ObliczWageZNazwy(string nazwa, decimal ilosc);
        decimal PobierzDlugoscNawojuZNazwy_m(string nazwa);
        int PobierzGramatureZNazwy(string nazwa);
        Task PobierzListy();
        tblTowarGeowlokninaParametryRodzaj PobierzRodzajZNazwy(string nazwa);
        decimal PobierzSzerokoscNawojuZNazwy_m(string nazwa);
        Task<tblTowar> PobierzTowarZGramaturyIRodzajuSurowca(int gramatura, string rodzajSurowca);

    }
}