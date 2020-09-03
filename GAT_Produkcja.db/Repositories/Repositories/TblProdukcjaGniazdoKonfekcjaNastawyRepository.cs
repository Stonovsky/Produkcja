using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{

    public interface ITblProdukcjaGniazdoKonfekcjaNastawyRepository : IGenericRepository<tblProdukcjaGniazdoKonfekcjaNastawy>
    {
    }

    public class TblProdukcjaGniazdoKonfekcjaNastawyRepository : GenericRepository<tblProdukcjaGniazdoKonfekcjaNastawy, GAT_ProdukcjaModel>, ITblProdukcjaGniazdoKonfekcjaNastawyRepository
    {
        public TblProdukcjaGniazdoKonfekcjaNastawyRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
