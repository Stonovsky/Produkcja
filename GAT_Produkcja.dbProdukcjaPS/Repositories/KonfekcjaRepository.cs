using GAT_PRodukcja.dbProdukcjaPS.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories
{
    public interface IKonfekcjaRepository
    {
    }

    public class KonfekcjaRepository : GenericRepository<Konfekcja, ProdukcjaPSDbContext>, IKonfekcjaRepository
    {
        public KonfekcjaRepository(ProdukcjaPSDbContext context) : base(context)
        {
        }
    }
}
