using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRozliczenie_CenyTransferoweRepository:IGenericRepository<tblProdukcjaRozliczenie_CenyTransferowe>
    {
    }

    public class TblProdukcjaRozliczenie_CenyTransferoweRepository : GenericRepository<tblProdukcjaRozliczenie_CenyTransferowe, GAT_ProdukcjaModel>, ITblProdukcjaRozliczenie_CenyTransferoweRepository
    {
        public TblProdukcjaRozliczenie_CenyTransferoweRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
