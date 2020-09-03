using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaGniazdoWlokninaNastawyRepository : IGenericRepository<tblProdukcjaGniazdoWlokninaNastawy>
    {
    }

    public class TblProdukcjaGniazdoWlokninaNastawyRepository : GenericRepository<tblProdukcjaGniazdoWlokninaNastawy, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoWlokninaNastawyRepository
    {
        public TblProdukcjaGniazdoWlokninaNastawyRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
