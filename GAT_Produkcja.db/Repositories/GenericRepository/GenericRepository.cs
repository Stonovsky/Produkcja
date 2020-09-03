using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.GenericRepository
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity> 
                where TEntity : class
                where TContext : DbContext
    {
        protected readonly TContext Context;
        public GenericRepository(TContext context)
        {
            Context = context;
        }

        #region Sync

        public virtual TEntity GetById(int id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return Context.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            // Note that here I've repeated Context.Set<TEntity>() in every method and this is causing
            // too much noise. I could get a reference to the DbSet returned from this method in the 
            // constructor and store it in a private field like _entities. This way, the implementation
            // of our methods would be cleaner:
            // 
            // _entities.ToList();
            // _entities.Where();
            // _entities.SingleOrDefault();
            // 
            // I didn't change it because I wanted the code to look like the videos. But feel free to change
            // this on your own.
            Context.Database.Log = s => Debug.WriteLine(s);
            return Context.Set<TEntity>().ToList();
        }

        public virtual IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            Context.Database.Log = Console.Write;
            return Context.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            Context.Database.Log = Console.Write;
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        #endregion

        #region AddRemove

        public void Add(TEntity entity)
        {
            Context.Database.Log = Console.Write;
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Database.Log = Console.Write;
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Database.Log = Console.Write;
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Database.Log = Console.Write;
            Context.Set<TEntity>().RemoveRange(entities);
        }

        #endregion

        #region Async
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            Context.Database.Log = Console.Write;
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            Context.Database.Log = Console.Write;
            return await Context.Set<TEntity>().ToListAsync();
        }
        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Context.Database.Log = Console.Write;
            return await Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }
        public virtual async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Context.Database.Log = Console.Write;

            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<int> GetNewNumberAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, int> selector)
        {
            Context.Database.Log = Console.Write;
            var listOfEntities = await Context.Set<TEntity>().Where(predicate).ToListAsync();
            //var listOfEntities = await Context.Set<TEntity>().Where(predicate)
            //                                                 .Select(s => new { selector})
            //                                                 .ToListAsync();
            var maxNumber = 0;
            if (listOfEntities.Count() > 0)
                maxNumber = listOfEntities.Max(selector);

            maxNumber += 1;
            return maxNumber;
        }

        public async Task<string> GetNewFullNumberAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, int> selector, string numberInterText)
        {
            Context.Database.Log = Console.Write;
            var maxNr = await GetNewNumberAsync(predicate, selector);

            return $"{maxNr}/{numberInterText}/{DateTime.Now.Year}";
        }

        public async Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, decimal> selector)
        {
            Context.Database.Log = Console.Write;
            var listOfEntities = await Context.Set<TEntity>().Where(predicate).ToListAsync();

            if (listOfEntities.Count() > 0)
                return listOfEntities.Sum(selector);
            else
                return 0;
        }

        #endregion

        public async Task Reload(TEntity entity)
        {
            await Context.Entry(entity).ReloadAsync();
        }

        public bool HasChanges()
        {
             return Context.ChangeTracker.HasChanges();
        }

        public async Task<TEntity> TakeOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Context.Database.Log = Console.Write;

            return await Context.Set<TEntity>().Where(predicate).Take(1).SingleOrDefaultAsync();
        }

        public async Task<TEntity> GetFirsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Context.Database.Log = Console.Write;

            return await Context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public EntityState GetState(TEntity entity)
        {
            return Context.Entry(entity).State;
        }
    }
}
