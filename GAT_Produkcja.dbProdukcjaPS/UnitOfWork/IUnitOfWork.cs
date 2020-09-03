using System;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_PRodukcja.dbProdukcjaPS.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IArtykułyRepository Artykuły { get; }
        IProdukcjaRepository Produkcja { get; }
        IKalanderRepository Kalander { get; }
        IKonfekcjaRepository Konfekcja { get; }
        INormyZuzyciaRepository NormyZuzycia { get; }
        ISurowiecRepository Surowiec { get; }

        //void Dispose();
        int Save();
        Task<int> SaveAsync();
        IQueryable<T> Query<T>();
    }
}