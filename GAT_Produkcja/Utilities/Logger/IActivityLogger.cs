using GAT_Produkcja.db;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.Logger
{
    public interface IActivityLogger
    {
        Task LogUserActivityAsync([CallerMemberName] string aktywnosc = "");
    }
}