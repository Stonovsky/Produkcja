using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblRuchNaglowekRepository : IGenericRepository<tblRuchNaglowek>
    {
        Task<int> PobierzNrDokumentuWewnetrznegoAsync(int IDDokumentTyp);
    }

    public class TblRuchNaglowekRepository : GenericRepository<tblRuchNaglowek,GAT_ProdukcjaModel>, ITblRuchNaglowekRepository
    {
        public TblRuchNaglowekRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
        public async Task<int> PobierzNrDokumentuWewnetrznegoAsync(int idRuchStatus)
        {
            int nrDokumentu = 0;
            var listaNaglowkowDlaStatusu = await Context.tblRuchNaglowek.Where(n => n.IDRuchStatus == idRuchStatus).ToListAsync();
            if (listaNaglowkowDlaStatusu.Count() == 0)
            {
                nrDokumentu += 1;
            }
            else
            {
                var numerDokumentuOstatni = listaNaglowkowDlaStatusu.Max(n => n.NrDokumentu);
                    nrDokumentu = numerDokumentuOstatni.GetValueOrDefault() + 1;
            }
            return nrDokumentu;
        }
    }
}
