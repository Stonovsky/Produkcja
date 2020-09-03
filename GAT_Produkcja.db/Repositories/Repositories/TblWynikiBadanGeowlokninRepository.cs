using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblWynikiBadanGeowlokninRepository:IGenericRepository<tblWynikiBadanGeowloknin>
    {
        Task<tblWynikiBadanGeowloknin> PobierzBadanieZNrProbkiIDaty(string nrRolki, DateTime dataBadania);
        Task<IEnumerable<tblWynikiBadanGeowloknin>> PobierzBadaniaAsync();
        IEnumerable<tblWynikiBadanGeowloknin> PobierzBadania();
    }

    public class TblWynikiBadanGeowlokninRepository : GenericRepository<tblWynikiBadanGeowloknin, GAT_ProdukcjaModel>, ITblWynikiBadanGeowlokninRepository
    {
        public TblWynikiBadanGeowlokninRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
        public async Task<tblWynikiBadanGeowloknin> PobierzBadanieZNrProbkiIDaty(string nrRolki, DateTime dataBadania)
        {
            return await Context.tblWynikiBadanGeowloknin
                                    .Where(p => p.NrRolki == nrRolki)
                                    .Where(p => p.DataBadania == dataBadania)
                                    .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<tblWynikiBadanGeowloknin>> PobierzBadaniaAsync()
        {
            return await Context.tblWynikiBadanGeowloknin.ToListAsync();
        }

        public IEnumerable<tblWynikiBadanGeowloknin> PobierzBadania()
        {
            return Context.tblWynikiBadanGeowloknin.ToList();
        }
    }
}
