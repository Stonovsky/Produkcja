using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblVATRepository:IGenericRepository<tblVAT>
    {
    }

    public class TblVATRepository : GenericRepository<tblVAT,GAT_ProdukcjaModel>, ITblVATRepository
    {
        public TblVATRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
