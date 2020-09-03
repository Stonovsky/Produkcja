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
    public interface ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository : IGenericRepository<tblProdukcjaZlecenieProdukcyjne_Mieszanka>
    {
    }

    public class TblProdukcjaZlecenieProdukcyjne_MieszankaRepository : GenericRepository<tblProdukcjaZlecenieProdukcyjne_Mieszanka, GAT_ProdukcjaModel>, ITblProdukcjaZlecenieProdukcyjne_MieszankaRepository
    {
        public TblProdukcjaZlecenieProdukcyjne_MieszankaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public override async Task<IEnumerable<tblProdukcjaZlecenieProdukcyjne_Mieszanka>> WhereAsync(Expression<Func<tblProdukcjaZlecenieProdukcyjne_Mieszanka, bool>> predicate)
        {
            return await Context.tblProdukcjaZlecenieProdukcyjne_Mieszanka
                .Where(predicate)
                .Include(t => t.tblProdukcjaZlcecenieProdukcyjne)
                .Include(t => t.tblJm)
                .ToListAsync();
        }
    }
}
