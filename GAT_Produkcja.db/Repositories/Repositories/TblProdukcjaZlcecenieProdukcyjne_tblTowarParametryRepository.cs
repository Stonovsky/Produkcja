using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaZlcecenieProdukcyjne_tblTowarParametryRepository:IGenericRepository<tblProdukcjaZlcecenieProdukcyjne_tblTowarParametry>
    {
    }

    public class TblProdukcjaZlcecenieProdukcyjne_tblTowarParametryRepository : 
                        GenericRepository<tblProdukcjaZlcecenieProdukcyjne_tblTowarParametry, GAT_ProdukcjaModel>, 
                        ITblProdukcjaZlcecenieProdukcyjne_tblTowarParametryRepository
    {
        public TblProdukcjaZlcecenieProdukcyjne_tblTowarParametryRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
