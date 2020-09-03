using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaGniazdoKalanderRepository : IGenericRepository<tblProdukcjaGniazdoKalander>
    {
    }

    public class TblProdukcjaGniazdoKalanderRepository : GenericRepository<tblProdukcjaGniazdoKalander, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoKalanderRepository
    {
        public TblProdukcjaGniazdoKalanderRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
