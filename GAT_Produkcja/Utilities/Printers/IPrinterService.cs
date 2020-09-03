using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.Printers
{
    public interface IPrinterService
    {
        string GetPrinterIP(string printerName);
        Task<string> GetPrinterIPAsync(string printerName);
        IEnumerable<string> GetPrinters();
        Task<IEnumerable<string>> GetPrintersAsync();
    }
}