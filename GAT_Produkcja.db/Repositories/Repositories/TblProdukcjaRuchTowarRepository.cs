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
    public interface ITblProdukcjaRuchTowarRepository:IGenericRepository<tblProdukcjaRuchTowar>
    {
    }

    public class TblProdukcjaRuchTowarRepository : GenericRepository<tblProdukcjaRuchTowar, GAT_ProdukcjaModel>, ITblProdukcjaRuchTowarRepository
    {
        public TblProdukcjaRuchTowarRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public override async Task<IEnumerable<tblProdukcjaRuchTowar>> GetAllAsync()
        {
            return await GetAllIncludeQuery().ToListAsync();
        }

        public override async Task<tblProdukcjaRuchTowar> GetByIdAsync(int id)
        {
            return await GetAllIncludeQuery().SingleOrDefaultAsync(t => t.IDProdukcjaRuchTowar == id);
        }

        public override IEnumerable<tblProdukcjaRuchTowar> Where(Expression<Func<tblProdukcjaRuchTowar, bool>> predicate)
        {
            return GetAllIncludeQuery().Where(predicate).ToList();
        }

        public override async Task<IEnumerable<tblProdukcjaRuchTowar>> WhereAsync(Expression<Func<tblProdukcjaRuchTowar, bool>> predicate)
        {
            return await GetAllIncludeQuery().Where(predicate).ToListAsync();
        }

        public override async Task<tblProdukcjaRuchTowar> SingleOrDefaultAsync(Expression<Func<tblProdukcjaRuchTowar, bool>> predicate)
        {
            return await GetAllIncludeQuery().SingleOrDefaultAsync(predicate); //base.SingleOrDefaultAsync(predicate);
        }
        private IQueryable<tblProdukcjaRuchTowar> GetAllIncludeQuery()
        {
            return Context.tblProdukcjaRuchTowar
                    .Include(t => t.tblProdukcjaRozliczenieStatus)
                    .Include(t => t.tblProdukcjaRuchNaglowek)
                    .Include(t => t.tblProdukcjaRuchNaglowek.tblProdukcjaGniazdoProdukcyjne)
                    .Include(t => t.tblProdukcjaRuchNaglowek.tblProdukcjaZlcecenieProdukcyjne)
                    .Include(t => t.tblProdukcjaRuchNaglowek.tblProdukcjaZlecenieCiecia)
                    .Include(t => t.tblProdukcjaRuchNaglowek.tblPracownikGAT)
                    .Include(t => t.tblProdukcjaRuchTowarStatus)
                    .Include(t => t.tblProdukcjaZlecenieTowar)
                    .Include(t => t.tblProdukcjaZlecenieTowar.tblProdukcjaZlecenie)
                    .Include(t => t.tblProdukcjaZlecenieTowar.tblProdukcjaZlecenieCiecia)
                    .Include(t => t.tblProdukcjaZlecenieTowar.tblProdukcjaZlecenieCiecia.tblKontrahent)
                    .Include(t => t.tblRuchStatus)
                    .Include(t => t.tblTowarGeowlokninaParametryGramatura)
                    .Include(t => t.tblTowarGeowlokninaParametrySurowiec);
        }

    }
}
