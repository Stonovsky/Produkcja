using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblRuchTowarRepository:IGenericRepository<tblRuchTowar>
    {
        Task<IEnumerable<tblRuchTowar>> GetAllWithConnectedTablesAsync();
    }

    public class TblRuchTowarRepository : GenericRepository<tblRuchTowar,GAT_ProdukcjaModel>, ITblRuchTowarRepository
    {
        public TblRuchTowarRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<IEnumerable<tblRuchTowar>> GetAllWithConnectedTablesAsync()
        {
            return await Context.tblRuchTowar
                .Include(i=>i.tblDokumentTyp)
                .Include(i=>i.tblJm)
                .Include(i=>i.tblMagazyn)
                .Include(i=>i.tblTowar)
                .Include(i=>i.tblVAT)
                .Include(i=>i.tblRuchNaglowek)
                .Include(i=>i.tblRuchNaglowek.tblPracownikGAT)
                //.Include(i=>i.tblProdukcjaGniazdaProdukcyjne)
                .ToListAsync();
        }
    }
}
