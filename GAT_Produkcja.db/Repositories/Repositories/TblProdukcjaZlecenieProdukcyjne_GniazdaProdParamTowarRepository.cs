using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowarRepository:IGenericRepository<tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar>
    {
    }

    public class TblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowarRepository : GenericRepository<tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar, GAT_ProdukcjaModel>, ITblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowarRepository
    {
        public TblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowarRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
