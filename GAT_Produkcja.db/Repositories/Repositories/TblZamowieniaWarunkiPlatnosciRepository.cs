using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowieniaWarunkiPlatnosciRepository:IGenericRepository<tblZamowieniaWarunkiPlatnosci>
    {
    }

    public class TblZamowieniaWarunkiPlatnosciRepository : GenericRepository<tblZamowieniaWarunkiPlatnosci, GAT_ProdukcjaModel>, ITblZamowieniaWarunkiPlatnosciRepository
    {
        public TblZamowieniaWarunkiPlatnosciRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
