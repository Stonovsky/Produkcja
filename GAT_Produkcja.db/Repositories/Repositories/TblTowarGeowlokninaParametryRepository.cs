using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeowlokninaParametryRepository : IGenericRepository<tblTowarGeowlokninaParametry>
    {
        Task<tblTowarGeowlokninaParametry> GetByIdIncludeAllTablesAsync(int idTowar);
    }

    public class TblTowarGeowlokninaParametryRepository : GenericRepository<tblTowarGeowlokninaParametry,GAT_ProdukcjaModel>, ITblTowarGeowlokninaParametryRepository
    {
        public TblTowarGeowlokninaParametryRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<tblTowarGeowlokninaParametry> GetByIdIncludeAllTablesAsync(int idTowar)
        {
            return await Context.tblTowarGeowlokninaParametry
                                                .Include(t => t.tblCertyfikatCE)
                                                .Include(t => t.tblTowar)
                                                .SingleOrDefaultAsync(t => t.IDTowar == idTowar);
        }
    }
}

