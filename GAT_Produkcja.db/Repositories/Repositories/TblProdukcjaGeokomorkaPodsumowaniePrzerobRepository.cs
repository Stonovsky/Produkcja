using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository : IGenericRepository<tblProdukcjaGeokomorkaPodsumowaniePrzerob>
    {
    }

    public class TblProdukcjaGeokomorkaPodsumowaniePrzerobRepository : GenericRepository<tblProdukcjaGeokomorkaPodsumowaniePrzerob, GAT_ProdukcjaModel>, ITblProdukcjaGeokomorkaPodsumowaniePrzerobRepository
    {
        public TblProdukcjaGeokomorkaPodsumowaniePrzerobRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public override async Task<IEnumerable<tblProdukcjaGeokomorkaPodsumowaniePrzerob>> GetAllAsync()
        {
            return await Context.tblProdukcjaGeokomorkaPodsumowaniePrzerob
                                            .Include(c=>c.tblPracownikGAT)
                                            .ToListAsync();
        }
    }
}
