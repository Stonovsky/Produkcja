using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeokomorkaParametryZgrzewRepository:IGenericRepository<tblTowarGeokomorkaParametryZgrzew>
    {
    }

    public class TblTowarGeokomorkaParametryZgrzewRepository : GenericRepository<tblTowarGeokomorkaParametryZgrzew,GAT_ProdukcjaModel>, ITblTowarGeokomorkaParametryZgrzewRepository
    {
        public TblTowarGeokomorkaParametryZgrzewRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
