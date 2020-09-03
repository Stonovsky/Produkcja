using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZapotrzebowanieStatusRepository:IGenericRepository<tblZapotrzebowanieStatus>
    {
    }

    public class TblZapotrzebowanieStatusRepository : GenericRepository<tblZapotrzebowanieStatus,GAT_ProdukcjaModel>, ITblZapotrzebowanieStatusRepository
    {
        public TblZapotrzebowanieStatusRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
