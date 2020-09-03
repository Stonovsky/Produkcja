using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwMagazynRuchGTXRepository:IGenericRepository<vwMagazynRuchGTX>
    {
    }

    public class VwMagazynRuchGTXRepository : GenericRepository<vwMagazynRuchGTX, GAT_ProdukcjaModel>, IVwMagazynRuchGTXRepository
    {
        public VwMagazynRuchGTXRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
