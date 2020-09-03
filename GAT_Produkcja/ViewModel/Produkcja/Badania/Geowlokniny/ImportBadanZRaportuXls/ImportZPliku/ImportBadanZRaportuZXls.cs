using GAT_Produkcja.db;
using GAT_Produkcja.UI.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls.ImportZPliku
{
    public class ImportBadanZRaportuZXls : IImportBadanZRaportuZXls
    {
        private tblWynikiBadanGeowloknin wynikiBadan;
        private tblWynikiBadanDlaProbek wynikBadanProbka;
        private ImportZPlikuModel wyniki;
        private Excel.Range poczatekZakresuTabeliSzczegolowejLP;
        private Excel.Range koniecZakresuTabeliSzczegolowejLP;
        private Excel.Range zakresTabeliSzczegolowejLP;
        private Excel.Application xlApp;
        private Excel.Workbook xlWorkbook;
        private Excel._Worksheet xlWorksheet;
        private Excel.Range xlRange;
        private OpenFileDialog plikExcel;
        private readonly IDialogService dialogService;

        public ImportBadanZRaportuZXls(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            wynikiBadan = new tblWynikiBadanGeowloknin();
            wynikBadanProbka = new tblWynikiBadanDlaProbek();

            wyniki = new ImportZPlikuModel();
        }

        public async Task<ImportZPlikuModel> PobierzWynikiBadan()
        {
            try
            {
                PobierzOgolnyZakresZPliku();
                PobierzZakresDlaTabeliSzczegolowej(xlRange);

                if (zakresTabeliSzczegolowejLP == null)
                {
                    dialogService.ShowInfo_BtnOK("Błędny plik z raportem lub raport nie odpowiada przyjętemu schematowi.\n\r\"Wybierz inny plik.");
                    return null;
                }

                wyniki.WynikiOgolne = await PobierzWynikiBadanZPlikuExcelAsync();
                DodajPozostaleParametryDoWynikuBadania();
                wyniki.WynikiSzczegoloweDlaProbek = await PobierzListeBadanSzczgolowychDlaProbekAsync();

                return wyniki;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (xlWorkbook != null)
                {
                    ZamknijIWyczyscExcela();
                }
            }

            return null;
        }

        private void DodajPozostaleParametryDoWynikuBadania()
        {
            FileInfo fileInfo = new FileInfo(plikExcel.FileName);

            wyniki.WynikiOgolne.DataUtworzeniaPliku = fileInfo.CreationTime;
            wyniki.WynikiOgolne.NazwaPliku = fileInfo.Name;
            wyniki.WynikiOgolne.SciezkaPliku = fileInfo.FullName;
            wyniki.WynikiOgolne.DataModyfikacjiPliku = fileInfo.LastWriteTime;
            wyniki.WynikiOgolne.StatusBadania = "Dodano";
        }

        private void ZamknijIWyczyscExcela()
        {
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            xlWorkbook.Close(false);
            Marshal.ReleaseComObject(xlWorkbook);

            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
        private void PobierzOgolnyZakresZPliku()
        {
            plikExcel = new OpenFileDialog();
            plikExcel.Filter = "Pliki Excel|*.xls";
            plikExcel.ShowDialog();

            if (string.IsNullOrEmpty(plikExcel.FileName))
                return;

            xlApp = new Excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(plikExcel.FileName);
            xlWorksheet = xlWorkbook.Sheets[1];
            xlRange = xlWorksheet.UsedRange;
        }

        #region WynikiBadanDaneOgolne
        public async Task<tblWynikiBadanGeowloknin> PobierzWynikiBadanZPlikuExcelAsync()
        {
            return await Task.Run(() => PobierzWynikiBadanZPlikuExcel());
        }
        public tblWynikiBadanGeowloknin PobierzWynikiBadanZPlikuExcel()
        {
            //PobierzOgolnyZakresZPliku();

            foreach (Excel.Range cell in xlRange.Cells)
            {
                if (cell.Value != null)
                {
                    string cellValue = cell.Value2.ToString();
                    DodajParametrDoWynikuBadan(cellValue, cell);
                }
            }

            
            return wynikiBadan;
        }
        private void DodajParametrDoWynikuBadan(string cellValue, dynamic cell)
        {
            if (cellValue.Contains("Nr rolki"))
            {
                if (cell.Offset[0, 1].Value2 is null)
                    throw new ArgumentNullException("Błąd: brak nr rolki!");
                wynikiBadan.NrRolki = cell.Offset[0, 1]?.Value2.ToString();
            }
            else if (cellValue.Contains("z dnia:"))
            {
                if (cell.Offset[0, 1].Value2 is null)
                    throw new ArgumentNullException("Błąd: brak daty!");
                wynikiBadan.DataBadania = DateTime.FromOADate(cell.Offset[0, 1].Value2);
            }
            else if (cellValue.Contains("Gramatura:"))
            {
                if (cell.Offset[0, 1].Value2 is null)
                    throw new ArgumentNullException("Błąd: brak gramatury!");

                wynikiBadan.Gramatura = cell.Offset[0, 1]?.Value2.ToString();
            }
            else if (cellValue.Contains("Surowiec:"))
            {
                if (cell.Offset[0, 1].Value2 is null)
                    throw new ArgumentNullException("Błąd: brak surowca!");
                wynikiBadan.Surowiec = cell.Offset[0, 1]?.Value2;
            }
            else if (cellValue.Contains("Kierunek"))
            {
                if (cell.Offset[0, 1].Value2 is null)
                    throw new ArgumentNullException("Błąd: brak kierunku!");
                wynikiBadan.KierunekBadania = cell.Offset[0, 1]?.Value2;
            }
            else if (cellValue =="Kalander")
            {
                if (cell.Offset[0, 1].Value2 == "TAK")
                {
                    wynikiBadan.CzyKalandrowana = true;
                }
                else if (cell.Offset[0, 1].Value2 == "NIE")
                {
                    wynikiBadan.CzyKalandrowana = false;
                }
                else
                {
                    wynikiBadan.CzyKalandrowana = null;
                }
            }
            else if (cellValue.Contains("Technologia"))
            {
                wynikiBadan.Technologia = cell.Offset[0, 1].Value2;
            }
            else if (cellValue.Contains("uwagi"))
            {
                wynikiBadan.Uwagi = cell.Offset[0, 1].Value2;
            }
            else if (cellValue.Contains("Kod kreskowy"))
            {
                wynikiBadan.KodKreskowy = cell.Offset[0, 1].Value2 == null ? string.Empty : cell.Offset[0, 1].Value2.ToString();
            }
            else if (cellValue.Contains("Rodzaj badania"))
            {
                wynikiBadan.BadanyWyrob = cell.Offset[0, 1].Value2;
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

        #endregion
        #region WynikiBadanSzczegolowychDlaProbek
        public async Task<List<tblWynikiBadanDlaProbek>> PobierzListeBadanSzczgolowychDlaProbekAsync()
        {
            return await Task.Run(() => PobierzListeBadanSzczegolowychDlaProbek());
        }
        public List<tblWynikiBadanDlaProbek> PobierzListeBadanSzczegolowychDlaProbek()
        {
            var listaWynikowBadanDlaProbek = new List<tblWynikiBadanDlaProbek>();
            foreach (Excel.Range cell in zakresTabeliSzczegolowejLP)
            {
                var wynikBadaniaProbki = new tblWynikiBadanDlaProbek();
                wynikBadaniaProbki.NrProbki = (int)cell.Value2;
                wynikBadaniaProbki.Sila = ConvertToDecimalWithCultureInfo(cell.Offset[0, 1].Value2.ToString());
                wynikBadaniaProbki.Wytrzymalosc = ConvertToDecimalWithCultureInfo(cell.Offset[0, 2].Value2.ToString());
                wynikBadaniaProbki.WydluzenieCalkowite = ConvertToDecimalWithCultureInfo(cell.Offset[0, 3].Value2.ToString());
                wynikBadaniaProbki.Gramatura = ConvertToDecimalWithCultureInfo(cell.Offset[0, 4].Value2.ToString());
                wynikBadaniaProbki.PlikZrodlowyNazwa = cell.Offset[0, 5].Value2;
                wynikBadaniaProbki.DataBadania = DateTime.FromOADate(cell.Offset[0, 6].Value2);
                wynikBadaniaProbki.DataDodania = DateTime.Now;

                listaWynikowBadanDlaProbek.Add(wynikBadaniaProbki);
            }

            return listaWynikowBadanDlaProbek;
        }
        private void PobierzZakresDlaTabeliSzczegolowej(Excel.Range range)
        {
            foreach (Excel.Range cell in range)
            {
                if (cell.Value != null)
                {
                    string cellValue = cell.Value2.ToString();
                    if (cellValue.Contains("No."))
                    {
                        poczatekZakresuTabeliSzczegolowejLP = cell.Offset[1, 0];
                    }
                    else if (cellValue.Contains("Maximum"))
                    {
                        koniecZakresuTabeliSzczegolowejLP = cell.Offset[-1, 0];
                    }
                }
            }

            if (poczatekZakresuTabeliSzczegolowejLP != null &&
                    koniecZakresuTabeliSzczegolowejLP != null)
            {
                //var padd = poczatekZakresuTabeliSzczegolowejLP.Address;
                //var kadd = koniecZakresuTabeliSzczegolowejLP.Address;

                zakresTabeliSzczegolowejLP = xlWorksheet.Range[poczatekZakresuTabeliSzczegolowejLP, koniecZakresuTabeliSzczegolowejLP].Cells;
            }
        }


        #endregion

    }
}
