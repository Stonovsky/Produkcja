using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwStanTowaruRepository:IGenericRepository<vwStanTowaru>
    {
    }

    public class VwStanTowaruRepository : GenericRepository<vwStanTowaru, GAT_ProdukcjaModel>, IVwStanTowaruRepository
    {
        public VwStanTowaruRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
