using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRozliczenie_PWPodsumowanieRepository:IGenericRepository<tblProdukcjaRozliczenie_PWPodsumowanie>
    {
    }

    public class TblProdukcjaRozliczenie_PWPodsumowanieRepository : GenericRepository<tblProdukcjaRozliczenie_PWPodsumowanie, GAT_ProdukcjaModel>, ITblProdukcjaRozliczenie_PWPodsumowanieRepository
    {
        public TblProdukcjaRozliczenie_PWPodsumowanieRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async override Task<IEnumerable<tblProdukcjaRozliczenie_PWPodsumowanie>> GetAllAsync()
        {
            return await Context.tblProdukcjaRozliczenie_PWPodsumowanie
                        .Include(t => t.tblProdukcjaRozliczenie_Naglowek)
                        .Include(t => t.tblJm)
                        .ToListAsync();
        }
    }
}
