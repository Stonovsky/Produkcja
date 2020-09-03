using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblMieszankaSkladRepository:IGenericRepository<tblMieszankaSklad>
    {
        Task<IEnumerable<tblMieszankaSklad>> GetAllWithTblJmAsync();
    }

    public class TblMieszankaSkladRepository : GenericRepository<tblMieszankaSklad, GAT_ProdukcjaModel>, ITblMieszankaSkladRepository
    {
        public TblMieszankaSkladRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<IEnumerable<tblMieszankaSklad>> GetAllWithTblJmAsync()
        {
            return await Context.tblMieszankaSklad
                            .Include(t => t.tblJm)
                            .ToListAsync();
        }

    }
}
