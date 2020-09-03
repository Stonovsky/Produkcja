using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblCertyfikatCERepository : IGenericRepository<tblCertyfikatCE>
    {
    }
    public class TblCertyfikatCERepository : GenericRepository<tblCertyfikatCE, GAT_ProdukcjaModel>, ITblCertyfikatCERepository
    {
        public TblCertyfikatCERepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
