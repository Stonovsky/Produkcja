using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ScaleService.Exceptions
{
    public class ScaleException : Exception
    {
        public ScaleException()
        {

        }
        public ScaleException(string name) : base(name)
        {

        }
    }
}
