using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblLogRepository:IGenericRepository<tblLog>
    {
    }

    public class TblLogRepository : GenericRepository<tblLog, GAT_ProdukcjaModel>, ITblLogRepository
    {
        public TblLogRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
