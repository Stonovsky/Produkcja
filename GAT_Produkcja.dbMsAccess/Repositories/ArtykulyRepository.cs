using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System.Text;

namespace GAT_Produkcja.dbMsAccess.Repositories
{
    public interface IArtykulyRepository : IGenericRepository<Artykuly>
    {
    }

    public class ArtykulyRepository : GenericRepository<Artykuly>, IArtykulyRepository
    {
        public ArtykulyRepository()
        {
            selectAllQuery = SqlSelectQueries.SqlSelectAll_Artykuly();
            tableName = "Artykuły";
        }
    }
}
