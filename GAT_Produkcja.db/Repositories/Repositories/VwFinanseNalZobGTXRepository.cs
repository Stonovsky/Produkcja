using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwFinanseNalZobGTXRepository:IGenericRepository<vwFinanseNalZobGTX>
    {
    }

    public class VwFinanseNalZobGTXRepository : GenericRepository<vwFinanseNalZobGTX, GAT_ProdukcjaModel>, IVwFinanseNalZobGTXRepository
    {
        public VwFinanseNalZobGTXRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
