using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwFinanseNalZobAGGRepository:IGenericRepository<vwFinanseNalZobAGG>
    {
    }

    public class VwFinanseNalZobAGGRepository : GenericRepository<vwFinanseNalZobAGG, GAT_ProdukcjaModel>, IVwFinanseNalZobAGGRepository
    {
        public VwFinanseNalZobAGGRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
