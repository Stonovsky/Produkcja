using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaZlecenieStatusRepository:IGenericRepository<tblProdukcjaZlecenieStatus>
    {
    }

    public class TblProdukcjaZlecenieStatusRepository : GenericRepository<tblProdukcjaZlecenieStatus, GAT_ProdukcjaModel>, ITblProdukcjaZlecenieStatusRepository
    {
        public TblProdukcjaZlecenieStatusRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
