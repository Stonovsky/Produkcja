using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblKodKreskowyTypRepository:IGenericRepository<tblKodKreskowyTyp>
    {
    }

    public class TblKodKreskowyTypRepository : GenericRepository<tblKodKreskowyTyp, GAT_ProdukcjaModel>, ITblKodKreskowyTypRepository
    {
        public TblKodKreskowyTypRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
