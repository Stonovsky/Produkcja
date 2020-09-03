using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowienieHandlowePakowanieRodzajRepository:IGenericRepository<tblZamowienieHandlowePakowanieRodzaj>
    {
    }

    public class TblZamowienieHandlowePakowanieRodzajRepository : GenericRepository<tblZamowienieHandlowePakowanieRodzaj,GAT_ProdukcjaModel>, ITblZamowienieHandlowePakowanieRodzajRepository
    {
        public TblZamowienieHandlowePakowanieRodzajRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
