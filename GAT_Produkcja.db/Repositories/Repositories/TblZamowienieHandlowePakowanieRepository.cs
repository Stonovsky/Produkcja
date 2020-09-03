using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblZamowienieHandlowePakowanieRepository:IGenericRepository<tblZamowienieHandlowePakowanie>
    {
    }

    public class TblZamowienieHandlowePakowanieRepository : GenericRepository<tblZamowienieHandlowePakowanie, GAT_ProdukcjaModel>, ITblZamowienieHandlowePakowanieRepository
    {
        public TblZamowienieHandlowePakowanieRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
