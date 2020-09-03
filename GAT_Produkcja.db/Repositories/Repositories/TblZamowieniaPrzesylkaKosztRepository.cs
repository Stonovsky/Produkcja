using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowieniaPrzesylkaKosztRepository:IGenericRepository<tblZamowieniaPrzesylkaKoszt>
    {
    }

    public class TblZamowieniaPrzesylkaKosztRepository : GenericRepository<tblZamowieniaPrzesylkaKoszt,GAT_ProdukcjaModel>, ITblZamowieniaPrzesylkaKosztRepository
    {
        public TblZamowieniaPrzesylkaKosztRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
