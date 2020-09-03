using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwMagazynRuchGTX2Repository:IGenericRepository<vwMagazynRuchGTX2>
    {
    }

    public class VwMagazynRuchGTX2Repository : GenericRepository<vwMagazynRuchGTX2, GAT_ProdukcjaModel>, IVwMagazynRuchGTX2Repository
    {
        public VwMagazynRuchGTX2Repository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
