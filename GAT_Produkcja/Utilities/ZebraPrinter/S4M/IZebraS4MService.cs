using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public interface IZebraS4MService
    {
        void PrintInternalLabel(tblProdukcjaRuchTowar towar, int iloscEtykiet);
        Task PrintInternalLabelAsync(tblProdukcjaRuchTowar towar, int iloscEtykiet);
        Task PrintCELabelAsync(tblProdukcjaRuchTowar towar, int iloscEtykiet,bool czyUV);
        bool CanPrint();
        Task LoadAsync();
    }
}