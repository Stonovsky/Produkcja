using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaGniazdoWlokninaBadaniaRepository:IGenericRepository<tblProdukcjaGniazdoWlokninaBadania>
    {
    }

    public class TblProdukcjaGniazdoWlokninaBadaniaRepository : GenericRepository<tblProdukcjaGniazdoWlokninaBadania, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoWlokninaBadaniaRepository
    {
        public TblProdukcjaGniazdoWlokninaBadaniaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
