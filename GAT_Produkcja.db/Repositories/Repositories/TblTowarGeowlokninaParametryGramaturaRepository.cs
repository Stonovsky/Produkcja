using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeowlokninaParametryGramaturaRepository:IGenericRepository<tblTowarGeowlokninaParametryGramatura>
    {
    }

    public class TblTowarGeowlokninaParametryGramaturaRepository : GenericRepository<tblTowarGeowlokninaParametryGramatura, GAT_ProdukcjaModel>, ITblTowarGeowlokninaParametryGramaturaRepository
    {
        public TblTowarGeowlokninaParametryGramaturaRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
