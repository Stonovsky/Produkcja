using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblUrzadzeniaRepository:IGenericRepository<tblUrzadzenia>
    {
        Task<IEnumerable<tblUrzadzenia>> PobierzUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowej(int idKlasyfikacjiSzczegolowej);
    }

    public class TblUrzadzeniaRepository : GenericRepository<tblUrzadzenia,GAT_ProdukcjaModel>, ITblUrzadzeniaRepository
    {
        public TblUrzadzeniaRepository(GAT_ProdukcjaModel ctx) : base(ctx)
        {
        }

        public async Task<IEnumerable<tblUrzadzenia>> PobierzUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowej(int idKlasyfikacjiSzczegolowej)
        {
            IEnumerable<int> listaIdUrzadzen = await Context.tblKlasyfikacjaSzczegolowa_UrzadzeniaMM
                                                        .Where(i => i.IDKlasyfikacjaSzczegolowa == idKlasyfikacjiSzczegolowej)
                                                        .Select(i => i.IDUrzadzenia)
                                                        .ToListAsync();


            return await Context.tblUrzadzenia.Where(u => listaIdUrzadzen.Contains(u.IDUrzadzenia)).ToListAsync();
        }
    }
}
