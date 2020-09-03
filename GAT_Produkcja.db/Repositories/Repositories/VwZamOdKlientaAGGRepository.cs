using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface IVwZamOdKlientaAGGRepository : IGenericRepository<vwZamOdKlientaAGG>
    {
    }

    public class VwZamOdKlientaAGGRepository : GenericRepository<vwZamOdKlientaAGG, GAT_ProdukcjaModel>, IVwZamOdKlientaAGGRepository
    {
        public VwZamOdKlientaAGGRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
