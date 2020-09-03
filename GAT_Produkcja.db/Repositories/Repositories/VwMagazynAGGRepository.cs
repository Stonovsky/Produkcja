using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwMagazynAGGRepository:IGenericRepository<vwMagazynAGG>
    {
    }

    public class VwMagazynAGGRepository : GenericRepository<vwMagazynAGG, GAT_ProdukcjaModel>, IVwMagazynAGGRepository
    {
        public VwMagazynAGGRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
