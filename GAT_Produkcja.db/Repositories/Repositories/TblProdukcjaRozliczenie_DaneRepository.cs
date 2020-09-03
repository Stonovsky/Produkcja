using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblProdukcjaRozliczenie_DaneRepository : IGenericRepository<tblProdukcjaRozliczenie_Naglowek>
    {
    }

    public class TblProdukcjaRozliczenie_DaneRepository : GenericRepository<tblProdukcjaRozliczenie_Naglowek, GAT_ProdukcjaModel>, ITblProdukcjaRozliczenie_DaneRepository
    {
        public TblProdukcjaRozliczenie_DaneRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
