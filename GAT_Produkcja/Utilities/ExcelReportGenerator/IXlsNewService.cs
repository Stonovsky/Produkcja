using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ExcelReportGenerator
{
  public  interface IXlsNewService
    {
        void CreateWorkbook(string excelPath = null);
        void CreateWorksheet(string worksheetName);
        void AddListToSheet<T>(IEnumerable<T> listOfEntities,
                              string titleOfTable,
                              string worksheetName,
                              IEnumerable<string> headersToDelete = null);
        void SaveToFile();



    }
}
