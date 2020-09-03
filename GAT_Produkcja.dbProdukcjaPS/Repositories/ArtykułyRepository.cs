using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_PRodukcja.dbProdukcjaPS.Repositories.GenericRepository;


namespace GAT_PRodukcja.dbProdukcjaPS.Repositories
{
    public interface IArtykułyRepository
    {
        int MyProperty { get; set; }
    }

    public class ArtykułyRepository : GenericRepository<Artykuły, ProdukcjaPSDbContext>, IArtykułyRepository
    {
        public int MyProperty { get; set; }
        public ArtykułyRepository(ProdukcjaPSDbContext context) : base(context)
        {
        }
    }
}
