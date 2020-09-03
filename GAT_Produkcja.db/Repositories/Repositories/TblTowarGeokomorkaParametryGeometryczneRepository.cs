using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeokomorkaParametryGeometryczneRepository:IGenericRepository<tblTowarGeokomorkaParametryGeometryczne>
    {
    }

    public class TblTowarGeokomorkaParametryGeometryczneRepository : GenericRepository<tblTowarGeokomorkaParametryGeometryczne,GAT_ProdukcjaModel>, ITblTowarGeokomorkaParametryGeometryczneRepository
    {
        public TblTowarGeokomorkaParametryGeometryczneRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
