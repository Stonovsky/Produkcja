using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaPaletyRepository : IGenericRepository<tblProdukcjaPalety>
    {
    }

    public class TblProdukcjaPaletyRepository : GenericRepository<tblProdukcjaPalety, GAT_ProdukcjaModel>, ITblProdukcjaPaletyRepository
    {
        public TblProdukcjaPaletyRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
