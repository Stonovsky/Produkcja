using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblKlasyfikacjaSzczegolowaRepository:IGenericRepository<tblKlasyfikacjaSzczegolowa>
    {
        Task<IEnumerable<tblKlasyfikacjaSzczegolowa>> PobierzKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejAsync(int idKlasyfikacjiOgolnej);
    }

    public class TblKlasyfikacjaSzczegolowaRepository : GenericRepository<tblKlasyfikacjaSzczegolowa, GAT_ProdukcjaModel>, ITblKlasyfikacjaSzczegolowaRepository
    {
        public TblKlasyfikacjaSzczegolowaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<IEnumerable<tblKlasyfikacjaSzczegolowa>> PobierzKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejAsync(int idKlasyfikacjiOgolnej)
        {
            IEnumerable<int> listaIdKlasyfikacjiSzczegolowej = await Context.tblKlasyfikacjaOgolna_SzczegolowaMM
                                                        .Where(i => i.IDKlasyfikacjaOgolna == idKlasyfikacjiOgolnej)
                                                        .Select(i => i.IDKlasyfikacjaSzczegolowa).ToListAsync();
            return await Context.tblKlasyfikacjaSzczegolowa.Where(u => listaIdKlasyfikacjiSzczegolowej.Contains(u.IDKlasyfikacjaSzczegolowa)).ToListAsync();


        }
    }
}
