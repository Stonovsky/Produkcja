using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public interface IZebraZPLCELabelGenerator
    {
        string GetLabelCE();
        Task<string> GetLabelCE(tblProdukcjaRuchTowar towar, int ilosc, bool czyUV=true);
    }
}