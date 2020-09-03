using System;
using System.Globalization;
using System.Runtime.InteropServices;
using GAT_Produkcja.db;
using Excel = Microsoft.Office.Interop.Excel;

namespace GAT_Produkcja.ViewModel.Badania.Geowlokniny
{

    public class WynikiBadanZPlikuExcel
    {
        #region Properties
        private string sciezkaPliku;
        private readonly IBadaniaGeowlokninRepository repository;
        private tblWynikiBadanGeowloknin wynikiBadan;
        

        #endregion

        #region CTOR
        public WynikiBadanZPlikuExcel(string sciezkaPliku, IBadaniaGeowlokninRepository repository)
        {
            this.sciezkaPliku = sciezkaPliku;
            this.repository = repository;
        }

        #endregion

        public tblWynikiBadanGeowloknin PobierzWynikiBadanZPlikuExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(sciezkaPliku);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            try
            {
                wynikiBadan = new tblWynikiBadanGeowloknin();
                wynikiBadan.SciezkaPliku = sciezkaPliku;

                foreach (Excel.Range cell in xlRange.Cells)
                {
                    if (cell.Value != null)
                    {
                        string cellValue = cell.Value2.ToString();
                        DodajParametrDoWynikuBadan(cellValue, cell);
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            return wynikiBadan;
        }

        private void DodajParametrDoWynikuBadan(string cellValue, dynamic cell)
        {
            if (cellValue.Contains("Nr rolki"))
            {
                string[] txtArray = cellValue.Split(':');
                string nrRolki = txtArray[1].Replace(" ", string.Empty);

                wynikiBadan.NrRolki = nrRolki;
            }
            else if (cellValue.Contains("z dnia"))
            {
                wynikiBadan.DataBadania = DateTime.FromOADate(cell.Offset[0, 1].Value2);
            }
            else if (cellValue.Contains("Gramatura:"))
            {
                wynikiBadan.Gramatura = cell.Offset[0, 1].Value2.ToString();
            }
            else if (cellValue.Contains("Surowiec"))
            {
                wynikiBadan.Surowiec = cell.Offset[0, 1].Value2;
            }
            else if (cellValue.Contains("Kierunek"))
            {
                wynikiBadan.KierunekBadania = cell.Offset[0, 1].Value2;
            }
            else if (cellValue.Contains("Technologia"))
            {
                string[] txtArray = cellValue.Split(':');
                string technologia = txtArray[1];
                wynikiBadan.Technologia = technologia;
            }
            else if (cellValue.Contains("uwagi"))
            {
                string[] txtArray = cellValue.Split(':');
                string uwagi = txtArray[1];
                wynikiBadan.Uwagi = uwagi;
            }
            else if (cellValue.Contains("Maximum"))
            {

                wynikiBadan.SilaMaksymalna = ConvertToDecimalWithCultureInfo(cell.Offset[0, 1].Value2.ToString());
                wynikiBadan.WytrzymaloscMaksymalna = ConvertToDecimalWithCultureInfo(cell.Offset[0, 2].Value2.ToString());
                wynikiBadan.WydluzenieMaksymalne = ConvertToDecimalWithCultureInfo(cell.Offset[0, 3].Value2.ToString());
                wynikiBadan.GramaturaMaksymalna = ConvertToDecimalWithCultureInfo(cell.Offset[0, 4].Value2.ToString());
            }
            else if (cellValue.Contains("Minimum"))
            {

                wynikiBadan.SilaMinimalna = ConvertToDecimalWithCultureInfo(cell.Offset[0, 1].Value2.ToString());
                wynikiBadan.WytrzymaloscMinimalna = ConvertToDecimalWithCultureInfo(cell.Offset[0, 2].Value2.ToString());
                wynikiBadan.WydluzenieMinimalne = ConvertToDecimalWithCultureInfo(cell.Offset[0, 3].Value2.ToString());
                wynikiBadan.GramaturaMinimalna = ConvertToDecimalWithCultureInfo(cell.Offset[0, 4].Value2.ToString());
            }
            else if (cellValue.Contains("Mean"))
            {

                wynikiBadan.SilaSrednia = ConvertToDecimalWithCultureInfo(cell.Offset[0, 1].Value2.ToString());
                wynikiBadan.WytrzymaloscSrednia = ConvertToDecimalWithCultureInfo(cell.Offset[0, 2].Value2.ToString());
                wynikiBadan.WydluzenieSrednie = ConvertToDecimalWithCultureInfo(cell.Offset[0, 3].Value2.ToString());
                wynikiBadan.GramaturaSrednia = ConvertToDecimalWithCultureInfo(cell.Offset[0, 4].Value2.ToString());
            }

            wynikiBadan.Nazwa = "ALTEX AT " + wynikiBadan.Surowiec + " " + wynikiBadan.Gramatura;
        }

        private decimal ConvertToDecimalWithCultureInfo(string value)
        {
            var culture = new CultureInfo("pl-PL");
            return Convert.ToDecimal(value, culture);
        }
    }


}
