using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwMagazynRuchAGGRepository:IGenericRepository<vwMagazynRuchAGG>
    {
    }

    public class VwMagazynRuchAGGRepository : GenericRepository<vwMagazynRuchAGG, GAT_ProdukcjaModel>, IVwMagazynRuchAGGRepository
    {
        public VwMagazynRuchAGGRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
