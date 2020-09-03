using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwFVKosztowezSubiektGTRepository:IGenericRepository<vwFVKosztowezSubiektGT>
    {
    }

    public class VwFVKosztowezSubiektGTRepository : GenericRepository<vwFVKosztowezSubiektGT,GAT_ProdukcjaModel>, IVwFVKosztowezSubiektGTRepository
    {
        public VwFVKosztowezSubiektGTRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
