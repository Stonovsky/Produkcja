using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblMagazynRepository:IGenericRepository<tblMagazyn>
    {
    }

    public class TblMagazynRepository : GenericRepository<tblMagazyn, GAT_ProdukcjaModel>, ITblMagazynRepository
    {
        public TblMagazynRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
