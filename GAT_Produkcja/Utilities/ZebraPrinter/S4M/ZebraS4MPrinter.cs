using Lextm.SharpSnmpLib;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public class ZebraS4MPrinter : IZebraS4MPrinter
    {
        public void Print(string ipAddress, string zlpMessage)
        {
            SendZplOverTcp(ipAddress, zlpMessage);
        }
        public void SendZplOverTcp(string theIpAddress, string zlpMessage)
        {
            //Guard clause checking printer status
            if (!IsPrinterReady(GetPrinterStatus(theIpAddress))) return;

            // Instantiate connection for ZPL TCP port at given address
            Connection thePrinterConn = new TcpConnection(theIpAddress, TcpConnection.DEFAULT_ZPL_TCP_PORT);

            try
            {
                // Open the connection - physical connection is established here.
                thePrinterConn.Open();

                // Send the data to printer as a byte array.
                thePrinterConn.Write(Encoding.UTF8.GetBytes(zlpMessage));
            }
            catch (ConnectionException e)
            {
                // Handle communications error here.
                throw;
            }
            finally
            {
                // Close the connection to release resources.
                thePrinterConn.Close();
            }
        }

        public PrinterStatus GetPrinterStatus(string ipAddress)
        {
            Connection connection = new TcpConnection(ipAddress, TcpConnection.DEFAULT_ZPL_TCP_PORT);
            try
            {
                connection.Open();
                var printer = ZebraPrinterFactory.GetInstance(connection);

                return printer.GetCurrentStatus();
            }
            catch (ConnectionException e)
            {
                throw;
            }
            catch (ZebraPrinterLanguageUnknownException e)
            {
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        public bool IsPrinterReady(PrinterStatus status)
        {
            if (status.isReadyToPrint)
            {
                return true;
            }
            else if (status.isPaused)
            {
                throw new ConnectionException("Błąd drukarki - drukarka wstrzymana");
            }
            else if (status.isHeadOpen)
            {
                throw new ConnectionException("Błąd drukarki - głowica otwarta");
            }
            else if (status.isPaperOut)
            {
                throw new ConnectionException("Błąd drukarki - brak papieru");
            }
            else
            {
                throw new ConnectionException("Błąd drukarki");
            }
        }
    }
}
