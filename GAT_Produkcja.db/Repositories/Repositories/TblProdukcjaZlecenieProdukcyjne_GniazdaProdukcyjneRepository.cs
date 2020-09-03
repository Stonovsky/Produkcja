using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjneRepository: IGenericRepository<tblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjne>
    {
    }

    public class TblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjneRepository : GenericRepository<tblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjne, GAT_ProdukcjaModel>, ITblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjneRepository
    {
        public TblProdukcjaZlecenieProdukcyjne_GniazdaProdukcyjneRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
