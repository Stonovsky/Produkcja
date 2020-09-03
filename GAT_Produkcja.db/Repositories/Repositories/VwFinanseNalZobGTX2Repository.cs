using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwFinanseNalZobGTX2Repository:IGenericRepository<vwFinanseNalZobGTX2>
    {
    }

    public class VwFinanseNalZobGTX2Repository : GenericRepository<vwFinanseNalZobGTX2, GAT_ProdukcjaModel>, IVwFinanseNalZobGTX2Repository
    {
        public VwFinanseNalZobGTX2Repository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
