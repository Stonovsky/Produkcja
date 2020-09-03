using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers
{
    public interface IProdukcjaEwidencjaHelper
    {
        IRozliczenieMsAccesHelper RozliczenieMsAccesHelper { get; }
    }

    public class ProdukcjaEwidencjaHelper : IProdukcjaEwidencjaHelper
    {
        public ProdukcjaEwidencjaHelper(IRozliczenieMsAccesHelper rozliczenieMsAccesHelper)
        {
            RozliczenieMsAccesHelper = rozliczenieMsAccesHelper;
        }

        public IRozliczenieMsAccesHelper RozliczenieMsAccesHelper { get; }
    }
}
