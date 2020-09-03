using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRozliczenie_RWRepository:IGenericRepository<tblProdukcjaRozliczenie_RW>
    {
    }

    public class TblProdukcjaRozliczenie_RWRepository : GenericRepository<tblProdukcjaRozliczenie_RW, GAT_ProdukcjaModel>, ITblProdukcjaRozliczenie_RWRepository
    {
        public TblProdukcjaRozliczenie_RWRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
