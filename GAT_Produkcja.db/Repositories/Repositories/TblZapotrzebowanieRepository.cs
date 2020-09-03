using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZapotrzebowanieRepository : IGenericRepository<tblZapotrzebowanie>
    {
        Task<int> PobierzNowyNrZamowieniaAsync();
    }

    public class TblZapotrzebowanieRepository : GenericRepository<tblZapotrzebowanie, GAT_ProdukcjaModel>, ITblZapotrzebowanieRepository
    {
        public TblZapotrzebowanieRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<int> PobierzNowyNrZamowieniaAsync()
        {
            var najwiekszyNr = await Context.tblZapotrzebowanie.MaxAsync(m => m.Nr);
            return najwiekszyNr + 1;
        }

        public override async Task<tblZapotrzebowanie> GetByIdAsync(int id)
        {
            return await GetAllIncludedQuery()
                                .Where(c => c.IDZapotrzebowanie == id)
                                .SingleOrDefaultAsync();
        }

        public override async Task<IEnumerable<tblZapotrzebowanie>> GetAllAsync()
        {
            return await GetAllIncludedQuery().ToListAsync();
        }
        private IQueryable<tblZapotrzebowanie> GetAllIncludedQuery()
        {
            return Context.tblZapotrzebowanie
                .Include(t => t.tblKontrahent)
                .Include(t => t.tblPracownikGAT)
                .Include(t => t.tblPliki)
                .Include(t => t.tblKlasyfikacjaOgolna)
                .Include(t => t.tblKlasyfikacjaSzczegolowa)
                .Include(t => t.tblUrzadzenia)
                .Include(t => t.tblZapotrzebowaniePozycje)
                .Include(t => t.tblZapotrzebowanieStatus)
                .Include(t => t.PracownikOdpZaZakup);
        }
    }
}
