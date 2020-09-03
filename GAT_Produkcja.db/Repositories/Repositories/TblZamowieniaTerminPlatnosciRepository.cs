using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowieniaTerminPlatnosciRepository:IGenericRepository<tblZamowieniaTerminPlatnosci>
    {
    }

    public class TblZamowieniaTerminPlatnosciRepository : GenericRepository<tblZamowieniaTerminPlatnosci,GAT_ProdukcjaModel>, ITblZamowieniaTerminPlatnosciRepository
    {
        public TblZamowieniaTerminPlatnosciRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
