using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Models.Adapters.Helpers
{
    public class ProdukcjaZlecenieIdHelper : IProdukcjaZlecenieIdHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private IEnumerable<tblProdukcjaZlecenie> zleceniaProdukcyjne;

        public ProdukcjaZlecenieIdHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            PobierzZleceniaProdukcyjne();

        }
        private void PobierzZleceniaProdukcyjne()
        {
            zleceniaProdukcyjne = unitOfWork.tblProdukcjaZlecenie.GetAll();
        }

        public int PobierzIdZlecenia(int IdZlecenieMsAccess)
        {
            return zleceniaProdukcyjne.SingleOrDefault(z => z.IDMsAccess == IdZlecenieMsAccess).IDProdukcjaZlecenie;

        }

    }
}
