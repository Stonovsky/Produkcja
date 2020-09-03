using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> TakeOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetFirsAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> GetNewNumberAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, int> selector);
        Task<string> GetNewFullNumberAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, int> selector, string numberInterText);
        Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, decimal> selector);
        Task Reload(TEntity entity);
        EntityState GetState(TEntity entity);

        bool HasChanges();
    }

}
