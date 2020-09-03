using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblRuchTowarGeowlokninaParametryRepository:IGenericRepository<tblRuchTowarGeowlokninaParametry>
    {
    }

    public class TblRuchTowarGeowlokninaParametryRepository : GenericRepository<tblRuchTowarGeowlokninaParametry,GAT_ProdukcjaModel>, ITblRuchTowarGeowlokninaParametryRepository
    {
        public TblRuchTowarGeowlokninaParametryRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
