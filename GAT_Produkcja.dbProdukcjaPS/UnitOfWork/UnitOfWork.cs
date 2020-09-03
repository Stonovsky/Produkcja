using GAT_PRodukcja.dbProdukcjaPS;
using GAT_PRodukcja.dbProdukcjaPS.Repositories;
using GAT_PRodukcja.dbProdukcjaPS.Repositories.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProdukcjaPSDbContext context;
        public IArtykułyRepository Artykuły { get; private set; }
        public IProdukcjaRepository Produkcja { get; private set; }
        public IKalanderRepository Kalander { get; private set; }
        public IKonfekcjaRepository Konfekcja { get; private set; }
        public INormyZuzyciaRepository NormyZuzycia { get; private set; }
        public ISurowiecRepository Surowiec { get; private set; }

        public UnitOfWork(ProdukcjaPSDbContext context)
        {
            this.context = context;

            Artykuły = new ArtykułyRepository(this.context);
            Produkcja = new ProdukcjaRepository(context);
            Kalander = new KalanderRepository(context);
            Konfekcja = new KonfekcjaRepository(context);
            NormyZuzycia = new NormyZuzyciaRepository(context);
            Surowiec = new SurowiecRepository(context);
        }

        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }

        public int Save()
        {
            return context.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }

}
