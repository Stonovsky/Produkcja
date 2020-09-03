using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblKonfiguracjaUrzadzenRepository : IGenericRepository<tblKonfiguracjaUrzadzen>
    {
    }

    public class TblKonfiguracjaUrzadzenRepository : GenericRepository<tblKonfiguracjaUrzadzen, GAT_ProdukcjaModel>, ITblKonfiguracjaUrzadzenRepository
    {
        public TblKonfiguracjaUrzadzenRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
