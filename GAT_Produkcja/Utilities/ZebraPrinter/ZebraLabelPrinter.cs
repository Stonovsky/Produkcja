using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using GalaSoft.MvvmLight;
using GAT_Produkcja.Utilities.ZebraPrinter;
using System.Management;
using GAT_Produkcja.UI.Services;
using System.Windows;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;

namespace GAT_Produkcja.Utilities.ZebraPrinter
{
    public class ZebraLabelPrinter : IZebraLabelPrinter
    {
        #region Properties

        private readonly ZebraLabelGenerator _zebraLabelGenerator;
        private string _printerName;

        #endregion

        #region CTOR
        public ZebraLabelPrinter()
        {
            _zebraLabelGenerator = new ZebraLabelGenerator();
        }
        #endregion

        #region Commands
        public async Task PrintAsync(LabelModel labelModel)
        {
            _printerName = GetPrinterName();

            if (string.IsNullOrWhiteSpace(_printerName))
                return;

            if (!labelModel.IsValid)
                return;

            string label = _zebraLabelGenerator.EtykietaProdukcja(labelModel);

            await Task.Run(() => RawPrinterHelper.SendStringToPrinter(_printerName, label));

        }
        public async Task PrintLabelCE(LabelModel labelModel, tblTowarGeowlokninaParametry geowlokninaParametry)
        {
            _printerName = GetPrinterName();

            if (string.IsNullOrWhiteSpace(_printerName))
                return;

            if (!labelModel.IsValid)
                return;

            await Task.Run(() => RawPrinterHelper.SendStringToPrinter(_printerName, _zebraLabelGenerator.EtykietaCE_PL(labelModel, geowlokninaParametry)));

        }

        public async Task PrintAsync(string label)
        {
            _printerName = GetPrinterName();

            if (string.IsNullOrWhiteSpace(_printerName))
                return;

            if (string.IsNullOrEmpty(label))
                return;

            await Task.Run(() => RawPrinterHelper.SendStringToPrinter(_printerName, label));

        }

        public string GetPrinterName()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
            {
                foreach (ManagementObject printer in searcher.Get())
                {
                    if (printer["Name"].ToString().ToLower().Contains("zdesigner") && printer["WorkOffline"].ToString().ToLower().Contains("false"))
                    {
                        _printerName = printer["Name"].ToString();
                        break;
                    }
                    else
                        _printerName = string.Empty;
                }

                return _printerName;
            }
        }

        #endregion

        #region EPL language comment
        // tekst str 41
        // barcode str 50
        // barcode 
        //0. Sending an initial newline guarantees that any previous borked
        //      command is submitted.
        //1. [N] Clear the image buffer.This is an important step and
        //      generally should be the first command in any EPL document;
        //      who knows what state the previous job left the printer in.
        //2. [q] Set the label width to 609 dots(3 inch label x 203 dpi
        //      = 609 dots wide).
        //3. [Q] Set the label height to 203 dots(1 inch label) with a 26
        //      dot gap between the labels. (The printer will probably auto-
        //      sense, but this doesn't hurt.)
        //4. [B] Draw a UPC-A barcode with value "603679025109" at
        //      x = 26 dots (1/8 in), y = 26 dots (1/8 in) with a narrow bar
        //      width of 2 dots and make it 152 dots (3/4 in) high. (The
        //      origin of the label coordinate system is the top left corner
        //      of the label.)
        //5. [A]
        //Draw the text "SKU 6205518 MFG 6354" at
        //      x = 253 dots (3/4 in), y = 26 dots (1/8 in) in
        //      printer font "3", normal horizontal and vertical scaling,
        //      and no fancy white-on-black effect.
        //(6 through 9 are similar to line 4.)
        //10. [P]
        //Print one copy of one label. 
        #endregion

        #region UsingZebraPrinter
        //var label = new ZebraLabelPrinter("000122072019");
        //label.Print("ZDesigner LP 2844 (Kopia 1)");
        #endregion
    }
}
