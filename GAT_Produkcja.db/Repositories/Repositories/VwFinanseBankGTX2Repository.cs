using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwFinanseBankGTX2Repository : IGenericRepository<vwFinanseBankGTX2>
    {
    }

    public class VwFinanseBankGTX2Repository : GenericRepository<vwFinanseBankGTX2, GAT_ProdukcjaModel>, IVwFinanseBankGTX2Repository
    {
        public VwFinanseBankGTX2Repository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
