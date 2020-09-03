using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwRuchTowaruRepository:IGenericRepository<vwRuchTowaru>
    {
    }

    public class VwRuchTowaruRepository : GenericRepository<vwRuchTowaru,GAT_ProdukcjaModel>, IVwRuchTowaruRepository
    {
        public VwRuchTowaruRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
