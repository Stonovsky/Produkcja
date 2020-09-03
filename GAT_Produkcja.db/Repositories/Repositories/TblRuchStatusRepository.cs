using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblRuchStatusRepository:IGenericRepository<tblRuchStatus>
    {
    }

    public class TblRuchStatusRepository : GenericRepository<tblRuchStatus, GAT_ProdukcjaModel>, ITblRuchStatusRepository
    {
        public TblRuchStatusRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
