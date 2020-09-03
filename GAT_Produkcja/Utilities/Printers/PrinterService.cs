using DocumentFormat.OpenXml.Office2010.CustomUI;
using GalaSoft.MvvmLight.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.Printers
{
    public class PrinterService : IPrinterService
    {
        private ManagementObjectSearcher printerQuery;
        private ManagementObjectCollection printerCollection;

        #region CTOR
        public PrinterService()
        {
            printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            printerCollection = printerQuery.Get();
        }
        #endregion


        public IEnumerable<string> GetPrinters()
        {
            foreach (var printer in printerCollection)
            {
                yield return printer.GetPropertyValue("Name").ToString();
            }
        }

        public async Task<IEnumerable<string>> GetPrintersAsync()
        {
            return await Task.Run(() => GetPrinters());
        }


        public string GetPrinterIP(string printerName)
        {
            string portName = string.Empty;
            foreach (var printer in printerCollection)
            {
                if (printer.GetPropertyValue("Name").ToString().ToLower().Contains(printerName.ToLower()))
                    portName = printer.Properties["PortName"].Value.ToString();
            }

            if (string.IsNullOrEmpty(portName))
                throw new ArgumentException("Brak IP dla podanej drukarki. Sprawdź czy wybrana drukarka jest drukarką sieciową.");

            return portName;
        }

        public async Task<string> GetPrinterIPAsync(string printerName)
        {
            return await Task.Run(() => GetPrinterIP(printerName));
        }
    }
}
