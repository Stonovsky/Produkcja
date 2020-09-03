using Zebra.Sdk.Printer;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public interface IZebraS4MPrinter
    {
        PrinterStatus GetPrinterStatus(string ipAddress);
        bool IsPrinterReady(PrinterStatus status);
        void Print(string ipAddress, string zlpMessage);
        void SendZplOverTcp(string theIpAddress, string zlpMessage);
    }
}