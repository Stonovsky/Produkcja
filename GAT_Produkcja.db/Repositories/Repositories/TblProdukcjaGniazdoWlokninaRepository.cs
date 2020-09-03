using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaGniazdoWlokninaRepository : IGenericRepository<tblProdukcjaGniazdoWloknina>
    {
        Task<int> PobierzNrRolkiAsync();
    }

    public class TblProdukcjaGniazdoWlokninaRepository : GenericRepository<tblProdukcjaGniazdoWloknina, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoWlokninaRepository
    {
        public TblProdukcjaGniazdoWlokninaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public override async Task<tblProdukcjaGniazdoWloknina> GetByIdAsync(int id)
        {
            return await Context.tblProdukcjaGniazdoWloknina
                .Include(t => t.tblProdukcjaZlcecenieProdukcyjne)
                .Include(t => t.tblProdukcjaGniazdoProdukcyjne)
                .SingleOrDefaultAsync(s=>s.IDProdukcjaGniazdoWloknina==id);
        }
        public async Task<int> PobierzNrRolkiAsync()
        {
            int? nrRolki = await Context.tblProdukcjaGniazdoWloknina.MaxAsync(g => (int?)g.NrRolki);
            if (nrRolki == null || nrRolki==0)
                nrRolki = 1;

            return nrRolki.Value;
        }
    }
}
