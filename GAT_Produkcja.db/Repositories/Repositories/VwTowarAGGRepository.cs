using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwTowarAGGRepository:IGenericRepository<vwTowarAGG>
    {
    }

    public class VwTowarAGGRepository : GenericRepository<vwTowarAGG, GAT_ProdukcjaModel>, IVwTowarAGGRepository
    {
        public VwTowarAGGRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
