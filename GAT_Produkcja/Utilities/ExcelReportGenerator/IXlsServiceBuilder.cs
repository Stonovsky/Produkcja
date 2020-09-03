using System.Collections.Generic;

namespace GAT_Produkcja.Utilities.ExcelReportGenerator
{
    public interface IXlsServiceBuilder
    {
        XlsServiceBuilder AddListToSheet<T>(IEnumerable<T> listOfEntities, string titleOfTable, string worksheetName, IEnumerable<string> headersToDelete = null);
        XlsServiceBuilder Build();
        XlsServiceBuilder CreateWorkbook(string excelPath = null);
        XlsServiceBuilder CreateWorksheet(string worksheetName);
    }
}