using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaZlecenieRepository : IGenericRepository<tblProdukcjaZlecenie>
    {
        Task<int?> PobierzNrZleceniaAsync();
        Task<IEnumerable<tblProdukcjaZlecenie>> PobierzAktywneZleceniaProdukcyjne();

    }

    public class TblProdukcjaZlecenieRepository : GenericRepository<tblProdukcjaZlecenie, GAT_ProdukcjaModel>, ITblProdukcjaZlecenieRepository
    {
        public TblProdukcjaZlecenieRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<int?> PobierzNrZleceniaAsync()
        {
            return await Context.tblProdukcjaZlcecenie.MaxAsync(m => m.NrZlecenia);
        }

        public async Task<IEnumerable<tblProdukcjaZlecenie>> PobierzAktywneZleceniaProdukcyjne()
        {

            return await Context.tblProdukcjaZlcecenie
                //.Include(t => t.tblTowarGeowlokninaParametryRodzaj)
                //.Include(t => t.tblTowarGeowlokninaParametryGramatura)
                //.Include(t => t.tblTowarGeowlokninaParametrySurowiec)
                //.Where(z => z.CzyZakonczone == false)
                .ToListAsync();
        }
    }


}
