using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Repositories.GenericRepository
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(T entity);

    }
}