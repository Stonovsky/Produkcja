using ClosedXML.Excel;
using GAT_Produkcja.db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ExcelReport
{
    public interface IXlsService
    {
        string CreateExcelReport<T>(IEnumerable<T> listsOfData,
                         string worksheetName,
                         IEnumerable<string> headers = null,
                         string excelPath = null);

        Task CreateExcelReportAsync<T>(IEnumerable<T> listsOfData,
                                       string worksheetName,
                                       IEnumerable<string> headers = null,
                                       string excelPath = null);
        IEnumerable<string> CreateHeaders<T>(IEnumerable<string> headersList, T entity);
        IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> GetTranferPrices(string worksheetName, string filePath = null);
    }
}