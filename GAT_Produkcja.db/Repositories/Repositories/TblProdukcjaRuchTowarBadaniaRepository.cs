using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRuchTowarBadaniaRepository: IGenericRepository<tblProdukcjaRuchTowarBadania>
    {
    }

    public class TblProdukcjaRuchTowarBadaniaRepository : GenericRepository<tblProdukcjaRuchTowarBadania, GAT_ProdukcjaModel>, ITblProdukcjaRuchTowarBadaniaRepository
    {
        public TblProdukcjaRuchTowarBadaniaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
