using GAT_PRodukcja.dbProdukcjaPS.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories
{
    public interface IKalanderRepository
    {
    }

    public class KalanderRepository : GenericRepository<Kalander, ProdukcjaPSDbContext>, IKalanderRepository
    {
        public KalanderRepository(ProdukcjaPSDbContext context) : base(context)
        {
        }
    }
}
