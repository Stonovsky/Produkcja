using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwZestSprzedazyAGGRepository :IGenericRepository<vwZestSprzedazyAGG>
    {
    }

    public class VwZestSprzedazyAGGRepository : GenericRepository<vwZestSprzedazyAGG, GAT_ProdukcjaModel>, IVwZestSprzedazyAGGRepository
    {
        public VwZestSprzedazyAGGRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
