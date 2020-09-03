using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaGniazdoKalanderNastawyRepository : IGenericRepository<tblProdukcjaGniazdoKalanderNastawy>
    {
    }

    public class TblProdukcjaGniazdoKalanderNastawyRepository : GenericRepository<tblProdukcjaGniazdoKalanderNastawy, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoKalanderNastawyRepository
    {
        public TblProdukcjaGniazdoKalanderNastawyRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
