using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblKlasyfikacjaOgolnaRepository:IGenericRepository<tblKlasyfikacjaOgolna>
    {
    }

    public class TblKlasyfikacjaOgolnaRepository : GenericRepository<tblKlasyfikacjaOgolna, GAT_ProdukcjaModel>, ITblKlasyfikacjaOgolnaRepository
    {
        public TblKlasyfikacjaOgolnaRepository(GAT_ProdukcjaModel ctx) : base(ctx)
        {
        }
    }
}
