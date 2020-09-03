using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblMieszankaRepository:IGenericRepository<tblMieszanka>
    {
        Task<IEnumerable<tblMieszanka>> GetAllWithJmAsync();

    }

    public class TblMieszankaRepository : GenericRepository<tblMieszanka, GAT_ProdukcjaModel>, ITblMieszankaRepository
    {
        public TblMieszankaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<IEnumerable<tblMieszanka>> GetAllWithJmAsync()
        {
            return await Context.tblMieszanka
                    .Include(t => t.tblJm)
                    .ToListAsync();
        }
    }
}
