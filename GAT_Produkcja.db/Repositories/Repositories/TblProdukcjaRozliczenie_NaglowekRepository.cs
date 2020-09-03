using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRozliczenie_NaglowekRepository: IGenericRepository<tblProdukcjaRozliczenie_Naglowek>
    {
    }

    public class TblProdukcjaRozliczenie_NaglowekRepository : GenericRepository<tblProdukcjaRozliczenie_Naglowek, GAT_ProdukcjaModel>, ITblProdukcjaRozliczenie_NaglowekRepository
    {
        public TblProdukcjaRozliczenie_NaglowekRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
