using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGrupaRepository:IGenericRepository<tblTowarGrupa>
    {
        Task<IEnumerable<tblTowarGrupa>> PobierzGrupeTowarowDlaGniazdAsync();
    }

    public class TblTowarGrupaRepository : GenericRepository<tblTowarGrupa,GAT_ProdukcjaModel>, ITblTowarGrupaRepository
    {
        public TblTowarGrupaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<IEnumerable<tblTowarGrupa>> PobierzGrupeTowarowDlaGniazdAsync()
        {
            return await Context.tblTowarGrupa.Where(g => g.Grupa.Contains("Geow")
                                                       || g.Grupa.Contains("Geok"))
                                                           .ToListAsync();
        }
    }
}
