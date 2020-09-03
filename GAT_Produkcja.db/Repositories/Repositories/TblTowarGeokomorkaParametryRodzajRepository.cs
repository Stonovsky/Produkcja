using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeokomorkaParametryRodzajRepository:IGenericRepository<tblTowarGeokomorkaParametryRodzaj>
    {
    }

    public class TblTowarGeokomorkaParametryRodzajRepository : GenericRepository<tblTowarGeokomorkaParametryRodzaj,GAT_ProdukcjaModel>, ITblTowarGeokomorkaParametryRodzajRepository
    {
        public TblTowarGeokomorkaParametryRodzajRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
