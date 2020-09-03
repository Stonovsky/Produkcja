using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.Factory
{
    public interface IProdukcjaEwidencjaStateFactory
    {
        IProdukcjaEwidencjaState PobierzStan(int zaznaczonyTabItem);
    }
}
