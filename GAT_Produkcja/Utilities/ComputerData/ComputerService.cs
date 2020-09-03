using DocumentFormat.OpenXml.Bibliography;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ComputerData
{
    public class ComputerService
    {
        #region CTOR
        public ComputerService()
        {
           
        }
        #endregion

        public string GetComputerName()
        {
            return Environment.MachineName;
        }

    }
}
