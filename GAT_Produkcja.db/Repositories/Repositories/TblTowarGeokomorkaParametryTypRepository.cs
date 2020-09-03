using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeokomorkaParametryTypRepository:IGenericRepository<tblTowarGeokomorkaParametryTyp>
    {
    }

    public class TblTowarGeokomorkaParametryTypRepository : GenericRepository<tblTowarGeokomorkaParametryTyp,GAT_ProdukcjaModel>, ITblTowarGeokomorkaParametryTypRepository
    {
        public TblTowarGeokomorkaParametryTypRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
