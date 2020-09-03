using ClosedXML.Excel;
using GAT_Produkcja.db;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.ExcelReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ExcelReportGenerator
{
    public class XlsNewService : IXlsNewService
    {
        private readonly IOpenSaveDialogService openSaveDialogService;
        private string excelPath;
        private XLWorkbook wb;
        private IXLWorksheet ws;

        public XlsNewService(IOpenSaveDialogService openSaveDialogService)
        {
            this.openSaveDialogService = openSaveDialogService;
        }


        public void CreateWorkbook(string excelPath = null)
        {
            wb = new XLWorkbook();

            this.excelPath = GetSaveFilePath(excelPath);
            if (string.IsNullOrEmpty( this.excelPath ))
                throw new ArgumentException("Brak sciezki pliku do zapisu.");

        }

        public void CreateWorksheet(string worksheetName)
        {
            ws = wb.AddWorksheet(worksheetName);
        }

        public void AddListToSheet<T>(IEnumerable<T> listOfEntities,
                                      string titleOfTable,
                                      string worksheetName,
                                      IEnumerable<string> headersToDelete = null)
        {
            if (listOfEntities is null) return;

            var headers = CreateHeaders(listOfEntities.First());

            try
            {
                AddTitle(titleOfTable);

                var emptyRow = ws.LastRowUsed()
                                 .RowBelow();
                AddHeadersToWorksheet(emptyRow, headers);

                var table = CreateTable(listOfEntities, emptyRow);
                DeleteTableColumn(table, headersToDelete);
                CreateTableTotalSum(table);
            }
            catch (Exception ex)
            {
                throw;
            }

            ws.LastRowUsed().RowBelow().RowBelow();
        }

        private IXLTable CreateTable<T>(IEnumerable<T> listOfEntities, IXLRow emptyRow)
        {
            var r = emptyRow.RowBelow().RangeAddress;
            var first = r.FirstAddress;
            ws.Cell(first).InsertData(listOfEntities);
            var range = ws.Range(emptyRow.FirstCell(), ws.LastCellUsed());

            return range.CreateTable();
        }

        private static void DeleteTableColumn(IXLTable table, IEnumerable<string> headersToDelete)
        {
            if (headersToDelete is null) return;

            foreach (var headerToDelete in headersToDelete)
            {
                var columnToDelete = table.Field(headerToDelete);
                if (columnToDelete is null) continue;

                columnToDelete.Delete();
            }
        }

        private void CreateTableTotalSum(IXLTable table)
        {
            table.ShowTotalsRow = true;
            foreach (var field in table.Fields)
            {
                field.TotalsRowFunction = XLTotalsRowFunction.Sum;
            }
        }

        private void AddTitle(string titleOfTable)
        {
            var lastRowUsed = ws.LastRowUsed();

            IXLCell titleCell;
            if (lastRowUsed is null)
                titleCell = ws.FirstCell();
            else
                titleCell = lastRowUsed.RowBelow()
                                       .RowBelow()
                                       .FirstCell();

            titleCell.Value = titleOfTable;

            titleCell.Style.Font.Bold = true;
        }

        public void SaveToFile()
        {
            wb.SaveAs(excelPath);
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

        public IEnumerable<string> CreateHeaders<T>(T entity)
        {
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
        private void AddHeadersToWorksheet(IXLRow emptyRow, IEnumerable<string> headers)
        {
            var headersList = headers.ToList();

            var rowNumber = emptyRow.FirstCell().Address.RowNumber;

            for (int i = 0; i < headersList.Count(); i++)
            {
                ws.Cell(rowNumber, i + 1).Value = headersList[i];
            }
        }


        private bool WorksheetExist(string worksheetName)
        {
            foreach (var worksheet in wb.Worksheets)
            {
                if (worksheet.Name == worksheetName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Usuwa kolumny ktorych naglowki rozpoczynaja sie na "tbl" lub "ID"
        /// </summary>
        /// <param name="headers"></param>
        private void DeleteUnnecessaryColumns(IEnumerable<string> headers,
                                              IEnumerable<string> headersToDelete = null)
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

            DeleteHeaders(headersToDelete, headerList);
        }

        private void DeleteHeaders(IEnumerable<string> headersToDelete, List<string> headerList)
        {
            if (headersToDelete is null) return;

            foreach (var headerToDelete in headersToDelete)
            {
                for (int i = headerList.Count() - 1; i >= 0; i--)
                {
                    if (headerList[i].Contains(headerToDelete))
                        ws.Column(i + 1).Delete();
                }
            }
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
