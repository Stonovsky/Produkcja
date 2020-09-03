using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarRepository:IGenericRepository<tblTowar>
    {
        Task<IEnumerable<tblTowar>> PobierzSurowceAsync();
        Task<IEnumerable<tblTowar>> GetAllInclRelatedTablesAsync();
        Task<IEnumerable<tblTowar>> GetByGroupIdAsync(TowarGrupaEnum towarGrupaEnum);
        Task<tblTowar> PobierzTowarZParametrowAsync(tblTowarGeowlokninaParametryGramatura parametryGramatura,
                                                         tblTowarGeowlokninaParametrySurowiec parametrySurowiec,
                                                         bool czyZUv);
    }

    public class TblTowarRepository : GenericRepository<tblTowar,GAT_ProdukcjaModel>, ITblTowarRepository
    {
        public TblTowarRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }

        public async Task<IEnumerable<tblTowar>> PobierzSurowceAsync()
        {
            //grupa nr 7 to surowce
            return await Context.tblTowar.Where(t => t.IDTowarGrupa == 7)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<tblTowar>> GetAllInclRelatedTablesAsync()
        {
            return await Context.tblTowar
                                .Include(t => t.tblJm)
                                .Include(t => t.tblTowarGrupa)
                                .ToListAsync();
        }

        public async Task<IEnumerable<tblTowar>> GetByGroupIdAsync(TowarGrupaEnum towarGrupaEnum)
        {
            return await Context.tblTowar
                    .Include(t => t.tblJm)
                    .Include(t => t.tblTowarGrupa)
                    .Where(t=>t.IDTowarGrupa==(int)towarGrupaEnum)
                    .ToListAsync();
        }

        public async Task<tblTowar> PobierzTowarZParametrowAsync(tblTowarGeowlokninaParametryGramatura parametryGramatura,
                                                                 tblTowarGeowlokninaParametrySurowiec parametrySurowiec,
                                                                 bool czyZUv)
        {
            var listaTowarow = await GetByGroupIdAsync(TowarGrupaEnum.Geowlokniny);
            listaTowarow = listaTowarow.Where(t => t.Nazwa.Contains($"{parametryGramatura.Gramatura}"))
                                       .Where(t => t.Nazwa.Contains($"{parametrySurowiec.Skrot}"))
                                       .ToList();

            if (czyZUv)
                listaTowarow = listaTowarow.Where(t => t.Nazwa.Contains("UV")).ToList();
            else
                listaTowarow = listaTowarow.Where(t => !t.Nazwa.Contains("UV")).ToList();



            return listaTowarow.Count() == 0 ? null : listaTowarow.First();
        }
    }
}
