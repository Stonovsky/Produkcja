using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblKontrahentRepository:IGenericRepository<tblKontrahent>
    {
        Task<IEnumerable<tblKontrahent>> WyszukajKontrahenta(tblKontrahent kontrahent);
        Task<IEnumerable<tblKontrahent>> WyszukajKontrahentaPoNIP(string NIP);


    }

    public class TblKontrahentRepository : GenericRepository<tblKontrahent, GAT_ProdukcjaModel>, ITblKontrahentRepository
    {

        public TblKontrahentRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<IEnumerable<tblKontrahent>> WyszukajKontrahenta(tblKontrahent kontrahent)
        {
            return await Context.tblKontrahent
                                .Where(k => kontrahent.Nazwa==null|| k.Nazwa.Contains(kontrahent.Nazwa))
                                .Where(k => kontrahent.NIP == null || k.NIP == kontrahent.NIP)
                                .Where(k => kontrahent.Miasto == null || k.Miasto.Contains(kontrahent.Miasto))
                                .ToListAsync();
        }

        public async Task<IEnumerable<tblKontrahent>> WyszukajKontrahentaPoNIP(string NIP)
        {
            return await Context.tblKontrahent.Where(k => k.NIP == NIP).ToListAsync();
        }
    }
}
