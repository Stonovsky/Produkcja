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
    public interface IVwTowarGTXRepository:IGenericRepository<vwTowarGTX>
    {
        Task<vwTowarGTX> PobierzTowarZParametrowAsync(tblTowarGeowlokninaParametryGramatura parametryGramatura,
                                                 tblTowarGeowlokninaParametrySurowiec parametrySurowiec,
                                                 bool czyZUv);

    }

    public class VwTowarGTXRepository : GenericRepository<vwTowarGTX, GAT_ProdukcjaModel>, IVwTowarGTXRepository
    {
        public VwTowarGTXRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
        public async Task<vwTowarGTX> PobierzTowarZParametrowAsync(tblTowarGeowlokninaParametryGramatura parametryGramatura,
                                                         tblTowarGeowlokninaParametrySurowiec parametrySurowiec,
                                                         bool czyZUv)
        {
            var listaTowarow = await GetAllAsync();
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
