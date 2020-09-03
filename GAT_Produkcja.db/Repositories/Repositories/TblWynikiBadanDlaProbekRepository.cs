using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblWynikiBadanDlaProbekRepository:IGenericRepository<tblWynikiBadanDlaProbek>
    {
    }

    public class TblWynikiBadanDlaProbekRepository : GenericRepository<tblWynikiBadanDlaProbek,GAT_ProdukcjaModel>, ITblWynikiBadanDlaProbekRepository
    {
        public TblWynikiBadanDlaProbekRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
