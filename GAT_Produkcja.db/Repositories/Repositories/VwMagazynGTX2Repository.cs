using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwMagazynGTX2Repository : IGenericRepository<vwMagazynGTX2>
    {
    }

    public class VwMagazynGTX2Repository : GenericRepository<vwMagazynGTX2, GAT_ProdukcjaModel>, IVwMagazynGTX2Repository
    {
        public VwMagazynGTX2Repository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
