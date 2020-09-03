using GAT_Produkcja.dbMsAccess.UnitOfWork;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.Helpers;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja.State.MsAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.ViewModels.Produkcja.Ewidencja.State
{
    public class ProdukcjaEwidencjaStateBaseInstance : ProdukcjaEwidencjaStateBase
    {
        public ProdukcjaEwidencjaStateBaseInstance(IUnitOfWorkMsAccess unitOfWorkMsAccess, IProdukcjaEwidencjaHelper helper) : base(unitOfWorkMsAccess, helper)
        {
        }

        public override  async Task GrupujTowary()
        {
            throw new NotImplementedException();
        }

        public override Task PobierzListeRolekZMsAccess()
        {
            throw new NotImplementedException();
        }

        public override void PodsumujListe()
        {
            throw new NotImplementedException();
        }
    }
}
