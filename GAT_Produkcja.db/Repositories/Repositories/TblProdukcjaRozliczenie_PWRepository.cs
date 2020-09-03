using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRozliczenie_PWRepository:IGenericRepository<tblProdukcjaRozliczenie_PW>
    {
    }

    public class TblProdukcjaRozliczenie_PWRepository : GenericRepository<tblProdukcjaRozliczenie_PW, GAT_ProdukcjaModel>, ITblProdukcjaRozliczenie_PWRepository
    {
        public TblProdukcjaRozliczenie_PWRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
