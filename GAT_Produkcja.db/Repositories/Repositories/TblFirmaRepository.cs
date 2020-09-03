using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblFirmaRepository:IGenericRepository<tblFirma>
    {
    }

    public class TblFirmaRepository : GenericRepository<tblFirma, GAT_ProdukcjaModel>, ITblFirmaRepository
    {
        public TblFirmaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
