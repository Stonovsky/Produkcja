using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaZlecenieTowarRepository : IGenericRepository<tblProdukcjaZlecenieTowar>
    {
    }

    public class TblProdukcjaZlecenieTowarRepository : GenericRepository<tblProdukcjaZlecenieTowar, GAT_ProdukcjaModel>, ITblProdukcjaZlecenieTowarRepository
    {
        public TblProdukcjaZlecenieTowarRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public override async Task<IEnumerable<tblProdukcjaZlecenieTowar>> GetAllAsync()
        {
            var query = GetAllIncludeQuery();

            return await query.ToListAsync();
        }
        public override async Task<IEnumerable<tblProdukcjaZlecenieTowar>> WhereAsync(Expression<Func<tblProdukcjaZlecenieTowar, bool>> predicate)
        {
            Context.Database.Log = Console.Write;
            var query = GetAllIncludeQuery();

            return await query.Where(predicate).ToListAsync();
        }

        public override async Task<tblProdukcjaZlecenieTowar> GetByIdAsync(int id)
        {
            var query = GetAllIncludeQuery();

            return await query.SingleOrDefaultAsync(t => t.IDProdukcjaZlecenieTowar == id);
        }

        private IQueryable<tblProdukcjaZlecenieTowar> GetAllIncludeQuery()
        {
            return Context.tblProdukcjaZlecenieTowar
                    .Include(t => t.tblProdukcjaZlecenieCiecia)
                    .Include(t => t.tblProdukcjaZlecenieCiecia.tblKontrahent)
                    .Include(t => t.tblProdukcjaZlecenieCiecia.tblProdukcjaZlecenieStatus)
                    .Include(t => t.tblTowarGeowlokninaParametryGramatura)
                    .Include(t => t.tblTowarGeowlokninaParametrySurowiec)
                    .Include(t => t.tblProdukcjaZlecenie)
                    .Include(t => t.tblProdukcjaZlecenie.tblPracownikGAT)
                    .Include(t => t.tblProdukcjaZlecenie.tblKontrahent)
                    .Include(t => t.tblProdukcjaZlecenieStatus)
                    .Include(t => t.tblProdukcjaZlecenie.tblProdukcjaZlecenieStatus)
                    .Include(t => t.tblProdukcjaGniazdoProdukcyjne);
        }
    }
}
