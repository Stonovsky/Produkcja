using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowienieHandloweTowarGeokomorkaRepository:IGenericRepository<tblZamowienieHandloweTowarGeokomorka>
    {
    }

    public class TblZamowienieHandloweTowarGeokomorkaRepository : GenericRepository<tblZamowienieHandloweTowarGeokomorka,GAT_ProdukcjaModel>, ITblZamowienieHandloweTowarGeokomorkaRepository
    {
        public TblZamowienieHandloweTowarGeokomorkaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
