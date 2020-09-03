using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblPlikiRepository:IGenericRepository<tblPliki>
    {
    }

    public class TblPlikiRepository : GenericRepository<tblPliki, GAT_ProdukcjaModel>, ITblPlikiRepository
    {
        public TblPlikiRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
