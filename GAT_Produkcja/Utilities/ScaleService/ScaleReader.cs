using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ScaleService

{
    public class ScaleReader
    {
        public void Read()
        {
            var scale = new USBScale();

            var device = scale.GetDevice();


            scale.Connect();

            if (scale.IsConnected)
            {

            }
        }

        public void ManagerScale()
        {

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
            {
                foreach (ManagementObject printer in searcher.Get())
                {
                    if (printer["Name"].ToString().ToLower().Contains("zdesigner") && printer["WorkOffline"].ToString().ToLower().Contains("false"))
                    {
                        //_printerName = printer["Name"].ToString();
                        break;
                    }
                    //else
                        //_printerName = string.Empty;
                }

                //return _printerName;
            }
        }

    }
}
