using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblTowarGeowlokninaParametrySurowiecRepository: IGenericRepository<tblTowarGeowlokninaParametrySurowiec>
    {
    }

    public class TblTowarGeowlokninaParametrySurowiecRepository : GenericRepository<tblTowarGeowlokninaParametrySurowiec, GAT_ProdukcjaModel>, ITblTowarGeowlokninaParametrySurowiecRepository
    {

        public TblTowarGeowlokninaParametrySurowiecRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
