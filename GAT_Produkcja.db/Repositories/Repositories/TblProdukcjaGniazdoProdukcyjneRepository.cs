using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaGniazdoProdukcyjneRepository:IGenericRepository<tblProdukcjaGniazdoProdukcyjne>
    {
    }

    public class TblProdukcjaGniazdoProdukcyjneRepository : GenericRepository<tblProdukcjaGniazdoProdukcyjne, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoProdukcyjneRepository
    {
        public TblProdukcjaGniazdoProdukcyjneRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
