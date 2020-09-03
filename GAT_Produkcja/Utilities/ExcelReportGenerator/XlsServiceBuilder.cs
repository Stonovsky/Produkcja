using Autofac;
using GAT_Produkcja.Startup;
using GAT_Produkcja.Utilities.ExcelReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ExcelReportGenerator
{
    public class XlsServiceBuilder : IXlsServiceBuilder
    {
        private readonly IXlsNewService xlsService;
        #region CTOR
        public XlsServiceBuilder()
        {
            this.xlsService = IoC.Container.Resolve<IXlsNewService>();
        }
        #endregion

        public XlsServiceBuilder CreateWorkbook(string excelPath = null)
        {
            xlsService.CreateWorkbook(excelPath);

            return this;
        }

        public XlsServiceBuilder CreateWorksheet(string worksheetName)
        {
            xlsService.CreateWorksheet(worksheetName);
            return this;
        }

        public XlsServiceBuilder AddListToSheet<T>(IEnumerable<T> listOfEntities,
                                      string titleOfTable,
                                      string worksheetName,
                                      IEnumerable<string> headersToDelete = null)
        {

            xlsService.AddListToSheet(listOfEntities, titleOfTable, worksheetName, headersToDelete);
            return this;
        }
        public XlsServiceBuilder Build()
        {
            xlsService.SaveToFile();
            return this;
        }


    }


}
