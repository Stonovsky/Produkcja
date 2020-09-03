using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaZlecenieCieciaRepository:IGenericRepository<tblProdukcjaZlecenieCiecia>
    {
        Task<int> PobierzMaxNrZleceniaAsync();
    }

    public class TblProdukcjaZlecenieCieciaRepository : GenericRepository<tblProdukcjaZlecenieCiecia, GAT_ProdukcjaModel>, ITblProdukcjaZlecenieCieciaRepository
    {
        public TblProdukcjaZlecenieCieciaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public override async Task<IEnumerable<tblProdukcjaZlecenieCiecia>> GetAllAsync()
        {
            return await Context.tblProdukcjaZlecenieCiecia
                         .Include(t => t.tblKontrahent)
                         .Include(t => t.tblPracownikGAT_Wykonujacy)
                         .Include(t => t.tblPracownikGAT_Zlecajacy)
                         .Include(t => t.tblProdukcjaZlecenieStatus)
                         .ToListAsync();
        }
        public async Task<int> PobierzMaxNrZleceniaAsync()
        {
            return await Context.tblProdukcjaZlecenieCiecia.MaxAsync(z => z.NrZleceniaCiecia);
        }
    }
}
