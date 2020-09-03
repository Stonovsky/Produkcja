using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwMagazynGTXRepository:IGenericRepository<vwMagazynGTX>
    {
    }

    public class VwMagazynGTXRepository : GenericRepository<vwMagazynGTX, GAT_ProdukcjaModel>, IVwMagazynGTXRepository
    {
        public VwMagazynGTXRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
