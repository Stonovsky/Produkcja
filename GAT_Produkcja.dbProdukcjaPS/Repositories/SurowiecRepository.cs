using GAT_PRodukcja.dbProdukcjaPS.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories
{
    public interface ISurowiecRepository
    {
    }

    public class SurowiecRepository : GenericRepository<Surowiec, ProdukcjaPSDbContext>, ISurowiecRepository
    {
        public SurowiecRepository(ProdukcjaPSDbContext context) : base(context)
        {
        }
    }
}
