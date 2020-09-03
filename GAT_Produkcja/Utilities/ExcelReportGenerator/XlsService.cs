using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System;
using GAT_Produkcja.Services;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.Utilities.ExcelReport
{
    public class XlsService : IXlsService
    {
        private string excelPath;
        private readonly IOpenSaveDialogService openSaveDialogService;

        private XLWorkbook wb;
        private IXLWorksheet ws;

        public XlsService(IOpenSaveDialogService openSaveDialogService)
        {
            this.openSaveDialogService = openSaveDialogService;
        }


        public async Task CreateExcelReportAsync<T>(IEnumerable<T> listsOfData,
                                                     string worksheetName,
                                                     IEnumerable<string> headers = null,
                                                     string excelPath = null)
        {

            if (listsOfData is null || !listsOfData.Any())
                throw new ArgumentException("Brak listy do exportu do pliku xlsx.");

            this.excelPath = GetSaveFilePath(excelPath);
            headers = CreateHeaders(headers, listsOfData.First());

            if (this.excelPath is null)
                throw new ArgumentException("Brak sciezki pliku do zapisu.");

            if (headers is null || !headers.Any())
                throw new ArgumentException("Brak nagłówków tabeli.");

            AddToNewSheet(listsOfData, headers, worksheetName);
            DeleteUnnecessaryColumns(headers);

            await Task.Run(() => wb.SaveAs(this.excelPath));

        }

        public string CreateExcelReport<T>(IEnumerable<T> listsOfData,
                                 string worksheetName,
                                 IEnumerable<string> headers = null,
                                 string excelPath = null)
        {

            if (listsOfData is null || !listsOfData.Any())
                throw new ArgumentException("Brak listy do exportu do pliku xlsx.");

            this.excelPath = GetSaveFilePath(excelPath);
            headers = CreateHeaders(headers, listsOfData.First());

            if (this.excelPath is null)
                throw new ArgumentException("Brak sciezki pliku do zapisu.");

            if (headers is null || !headers.Any())
                throw new ArgumentException("Brak nagłówków tabeli.");

            AddToNewSheet(listsOfData, headers, worksheetName);
            DeleteUnnecessaryColumns(headers);

            wb.SaveAs(this.excelPath);

            return excelPath;
        }

        /// <summary>
        /// Usuwa kolumny ktorych naglowki rozpoczynaja sie na "tbl" lub "ID"
        /// </summary>
        /// <param name="headers"></param>
        private void DeleteUnnecessaryColumns(IEnumerable<string> headers)
        {
            var headerList = headers.ToList();
            for (int i = headerList.Count() - 1; i >= 0; i--)
            {
                if (headerList[i].Contains("tbl") ||
                    headerList[i].Contains("ID")
                    )
                {
                    ws.Column(i + 1).Delete();
                }
            }
        }

        /// <summary>
        /// Wskazuje sciezke pliku do zapisu
        /// </summary>
        /// <param name="excelPath"></param>
        /// <returns></returns>
        private string GetSaveFilePath(string excelPath)
        {
            if (string.IsNullOrEmpty(excelPath))
                return openSaveDialogService.SaveFile();
            else
                return excelPath;
        }
        /// <summary>
        /// Wskazuje sciezke pliku
        /// </summary>
        /// <param name="excelPath"></param>
        /// <returns></returns>
        private string GetOpenFilePath(string excelPath)
        {
            if (string.IsNullOrEmpty(excelPath))
                return openSaveDialogService.OpenFile();
            else
                return excelPath;
        }

        /// <summary>
        /// Generuje nglowki
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="headersList"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IEnumerable<string> CreateHeaders<T>(IEnumerable<string> headersList, T entity)
        {
            if (headersList != null) return headersList;

            Type type = entity.GetType();
            var properties = type.GetProperties();

            var headers = properties.Select(p => p.Name).ToList();

            return headers;
        }

        /// <summary>
        /// Dodaje naglowek do arkusza Excel jako pierwszy wiersz w arkuszu
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="headers"></param>
        private void AddHeadersToWorksheet(IXLWorksheet ws, IEnumerable<string> headers)
        {
            var headersList = headers.ToList();

            for (int i = 0; i < headersList.Count(); i++)
            {
                ws.Cell(1, i + 1).Value = headersList[i];
            }
        }

        /// <summary>
        /// Dodaje liste do arkusza Excel
        /// </summary>
        /// <typeparam name="T">Typ generyczny listy</typeparam>
        /// <param name="listOfEntities">lista encji</param>
        /// <param name="headers">lista naglowkow</param>
        /// <param name="worksheetName">nazwa arkusza Excel</param>
        private void AddToNewSheet<T>(IEnumerable<T> listOfEntities,
                                      IEnumerable<string> headers,
                                      string worksheetName)
        {
            wb = new XLWorkbook();
            ws = wb.Worksheets.Add(worksheetName);

            AddHeadersToWorksheet(ws, headers);
            try
            {
                ws.Cell(2, 1).InsertData(listOfEntities);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //public IEnumerable<T> GetDataFromFile<T>(string filePath, string worksheetName)
        //{
        //    var list = new List<T>();
        //    var wb = new XLWorkbook(filePath);
        //    var ws = wb.Worksheet(worksheetName);

        //    // Look for the first row used
        //    var firstRowUsed = ws.FirstRowUsed();

        //    // Narrow down the row so that it only includes the used part
        //    var categoryRow = firstRowUsed.RowUsed();

        //    // Move to the next row (it now has the titles)
        //    categoryRow = categoryRow.RowBelow();

        //    // Get all categories
        //    while (!categoryRow.Cell(coCategoryId).IsEmpty())
        //    {
        //        String categoryName = categoryRow.Cell(coCategoryName).GetString();
        //        categories.Add(categoryName);

        //        categoryRow = categoryRow.RowBelow();
        //    }
        //}

        public IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> GetTranferPrices(string worksheetName, string filePath = null)
        {
            filePath = GetOpenFilePath(filePath);

            var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(worksheetName);

            var tables = ws.Tables;
            var table = tables.First();

            var headerShortcutList = new List<string> { "Nazwa", "Cena SPRZEDAŻY GTEX", "Cena HURTOWA" };

            if (!FileHasHeaders(headerShortcutList, table))
                throw new ArgumentException("Brak odpowiednich nagłówków w pliku\rPlik niewłaściwy");

            var cenyTransferowe = new List<tblProdukcjaRozliczenie_CenyTransferowe>();

            foreach (var row in table.DataRange.Rows())
            {
                var cenaTransferowa = new tblProdukcjaRozliczenie_CenyTransferowe();
                cenaTransferowa.TowarNazwa = row.Field(GetFullHeaderName("Nazwa", table)).GetString();
                cenaTransferowa.CenaTransferowa = row.Field(GetFullHeaderName("Cena SPRZEDAŻY GTEX", table)).GetValue<decimal>();
                cenaTransferowa.CenaHurtowa = row.Field(GetFullHeaderName("Cena HURTOWA", table)).GetValue<decimal>();

                cenyTransferowe.Add(cenaTransferowa);
                //yield return cenaTransferowa;
            }
            return cenyTransferowe;
        }

        private bool FileHasHeaders(List<string> headerShortcutList, IXLTable table)
        {
            foreach (var headerShortcut in headerShortcutList)
            {
                if (GetFullHeaderName(headerShortcut, table) is null)
                    return false;
            }

            return true;
        }

        private string GetFullHeaderName(string fragmentNazwyNaglowka, IXLTable table)
        {
            var headers = new List<string>();

            foreach (var cell in table.HeadersRow().Cells())
            {
                headers.Add(cell.GetString());
            }

            var fullHeaderName = headers.Where(s => s.ToLower().Contains(fragmentNazwyNaglowka.ToLower()))
                                        .FirstOrDefault();
            return fullHeaderName;
        }

    }
}
