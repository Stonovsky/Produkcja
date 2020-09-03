using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarParametryRepository:IGenericRepository<tblTowarParametry>
    {
        Task<IEnumerable<tblTowarParametry>> GetAllWithTblJmAsync();
    }

    public class TblTowarParametryRepository : GenericRepository<tblTowarParametry,GAT_ProdukcjaModel>, ITblTowarParametryRepository
    {
        public TblTowarParametryRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
        public async Task<IEnumerable<tblTowarParametry>> GetAllWithTblJmAsync()
        {
            return await Context.tblTowarParametry
                .Include(t => t.tblJm)
                .ToListAsync();
        }
    }
}
