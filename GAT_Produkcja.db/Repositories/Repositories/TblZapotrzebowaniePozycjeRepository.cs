using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZapotrzebowaniePozycjeRepository:IGenericRepository<tblZapotrzebowaniePozycje>
    {
    }

    public class TblZapotrzebowaniePozycjeRepository : GenericRepository<tblZapotrzebowaniePozycje,GAT_ProdukcjaModel>, ITblZapotrzebowaniePozycjeRepository
    {
        public TblZapotrzebowaniePozycjeRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
