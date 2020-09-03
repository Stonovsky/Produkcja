using GAT_PRodukcja.dbProdukcjaPS.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories
{
    public interface INormyZuzyciaRepository
    {
    }

    public class NormyZuzyciaRepository : GenericRepository<Normy_zużycia, ProdukcjaPSDbContext>, INormyZuzyciaRepository
    {
        public NormyZuzyciaRepository(ProdukcjaPSDbContext context) : base(context)
        {
        }
    }
}
