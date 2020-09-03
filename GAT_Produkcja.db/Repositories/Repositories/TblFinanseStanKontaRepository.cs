using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblFinanseStanKontaRepository : IGenericRepository<tblFinanseStanKonta>
    {
    }

    public class TblFinanseStanKontaRepository : GenericRepository<tblFinanseStanKonta, GAT_ProdukcjaModel>, ITblFinanseStanKontaRepository
    {
        public TblFinanseStanKontaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
