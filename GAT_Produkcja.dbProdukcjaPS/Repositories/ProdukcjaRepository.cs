using GAT_PRodukcja.dbProdukcjaPS.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories
{
    public interface IProdukcjaRepository
    {
    }

    public class ProdukcjaRepository : GenericRepository<Produkcja, ProdukcjaPSDbContext>, IProdukcjaRepository
    {
        public ProdukcjaRepository(ProdukcjaPSDbContext context) : base(context)
        {
        }
    }
}
