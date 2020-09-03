using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeowlokninaParametryRodzajRepository:IGenericRepository<tblTowarGeowlokninaParametryRodzaj>
    {
    }

    public class TblTowarGeowlokninaParametryRodzajRepository : GenericRepository<tblTowarGeowlokninaParametryRodzaj,GAT_ProdukcjaModel>, ITblTowarGeowlokninaParametryRodzajRepository
    {
        public TblTowarGeowlokninaParametryRodzajRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
