using GAT_Produkcja.db.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.db.Repositories.Repositories
{
    public interface ITblDokumentTypRepository:IGenericRepository<tblDokumentTyp>
    {
    }

    public class TblDokumentTypRepository : GenericRepository<tblDokumentTyp, GAT_ProdukcjaModel>, ITblDokumentTypRepository
    {
        public TblDokumentTypRepository(GAT_ProdukcjaModel context) : base(context)
        {
        }
    }
}
