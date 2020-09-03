using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaTulejeRepository : IGenericRepository<tblProdukcjaTuleje>
    {
    }

    public class TblProdukcjaTulejeRepository : GenericRepository<tblProdukcjaTuleje, GAT_ProdukcjaModel>, ITblProdukcjaTulejeRepository
    {
        public TblProdukcjaTulejeRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
