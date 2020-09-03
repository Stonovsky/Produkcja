using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblJmRepository:IGenericRepository<tblJm>
    {
    }

    public class TblJmRepository : GenericRepository<tblJm, GAT_ProdukcjaModel>, ITblJmRepository
    {
        public TblJmRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
