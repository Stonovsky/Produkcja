using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwZapotrzebowanieEwidencjaRepository : IGenericRepository<vwZapotrzebowanieEwidencja>
    {
        Task<List<vwZapotrzebowanieEwidencja>> PobierzZapotrzebowanieZbiezacegoMiesiacaAsync();
        Task<List<vwZapotrzebowanieEwidencja>> PobierzZapotrzebowaniaOczekujaceAsync();
        Task<List<vwZapotrzebowanieEwidencja>> PobierzZapotrzebowaniaZweryfikowaneIOczekujaceAsync();
    }

    public class VwZapotrzebowanieEwidencjaRepository : GenericRepository<vwZapotrzebowanieEwidencja, GAT_ProdukcjaModel>, IVwZapotrzebowanieEwidencjaRepository
    {
        public VwZapotrzebowanieEwidencjaRepository(GAT_ProdukcjaModel ctx) : base(ctx)
        {
        }

        public async Task<List<vwZapotrzebowanieEwidencja>> PobierzZapotrzebowanieZbiezacegoMiesiacaAsync()
        {
            var month = DateTime.Now.Month;
            return await Context.vwZapotrzebowanieEwidencja.Where(z => z.DataZgloszenia.Value.Month == month).ToListAsync();
        }

        public async Task<List<vwZapotrzebowanieEwidencja>> PobierzZapotrzebowaniaOczekujaceAsync()
        {
            return await Context.vwZapotrzebowanieEwidencja
                            .Where(z => z.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Oczekuje).ToListAsync();
        }

        public async Task<List<vwZapotrzebowanieEwidencja>> PobierzZapotrzebowaniaZweryfikowaneIOczekujaceAsync()
        {
            return await Context.vwZapotrzebowanieEwidencja
                .Where(z => z.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Oczekuje)
                .Where(z => z.CzyZweryfikowano==true)
                .ToListAsync();

        }
        public override async Task<IEnumerable<vwZapotrzebowanieEwidencja>> GetAllAsync()
        {
            return await Context.vwZapotrzebowanieEwidencja.OrderByDescending(d=>d.DataZapotrzebowania).ToListAsync();
        }
    }
}
