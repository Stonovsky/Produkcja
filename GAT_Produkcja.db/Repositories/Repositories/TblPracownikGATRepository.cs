using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblPracownikGATRepository:IGenericRepository<tblPracownikGAT>
    {
        Task<List<tblPracownikGAT>> PobierzPracownikowMogacychZglaszacZapotrzebowaniaAsync();
        Task<List<tblPracownikGAT>> PobierzPracownikowPracujacychAsync();
    }

    public class TblPracownikGATRepository : GenericRepository<tblPracownikGAT, GAT_ProdukcjaModel>, ITblPracownikGATRepository
    {

        public TblPracownikGATRepository(GAT_ProdukcjaModel ctx) : base(ctx)
        {
        }

        public async Task<List<tblPracownikGAT>> PobierzPracownikowMogacychZglaszacZapotrzebowaniaAsync()
        {
            return await Context.tblPracownikGAT
                            .Where(p => p.CzyPracuje == true)
                            .OrderBy(p => p.Nazwisko)
                            .ToListAsync();
        }

        public async Task<List<tblPracownikGAT>> PobierzPracownikowPracujacychAsync()
        {
            return await Context.tblPracownikGAT
                                        .Where(p => p.CzyPracuje == true)
                                        .OrderBy(p => p.Imie)
                                        .ToListAsync();
        }


    }
}
