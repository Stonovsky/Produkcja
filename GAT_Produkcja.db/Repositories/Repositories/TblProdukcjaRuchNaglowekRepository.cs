using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRuchNaglowekRepository : IGenericRepository<tblProdukcjaRuchNaglowek>
    {
    }

    public class TblProdukcjaRuchNaglowekRepository : GenericRepository<tblProdukcjaRuchNaglowek, GAT_ProdukcjaModel>, ITblProdukcjaRuchNaglowekRepository
    {
        public TblProdukcjaRuchNaglowekRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public override async Task<tblProdukcjaRuchNaglowek> GetByIdAsync(int id)
        {
            return await GetAllIncludedQuery().SingleOrDefaultAsync(e=>e.IDProdukcjaRuchNaglowek==id);
        }

        public override async Task<tblProdukcjaRuchNaglowek> SingleOrDefaultAsync(Expression<Func<tblProdukcjaRuchNaglowek, bool>> predicate)
        {
            return await GetAllIncludedQuery().SingleOrDefaultAsync(predicate);
        }

        private IQueryable<tblProdukcjaRuchNaglowek> GetAllIncludedQuery()
        {
            return Context.tblProdukcjaRuchNaglowek
                                .Include(t => t.tblProdukcjaGniazdoProdukcyjne)
                                .Include(t => t.tblProdukcjaZlcecenieProdukcyjne)
                                .Include(t => t.tblProdukcjaZlecenieCiecia)
                                .Include(t => t.tblProdukcjaZlecenieTowar);
        }
    }
}
