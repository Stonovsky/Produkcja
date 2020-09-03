using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Repositories
{
    public interface INormyZuzyciaRepository : IGenericRepository<NormyZuzycia>
    {
    }

    public class NormyZuzyciaRepository : GenericRepository<NormyZuzycia>, INormyZuzyciaRepository
    {
        public NormyZuzyciaRepository()
        {
            selectAllQuery = SqlSelectQueries.SqlSelectAll_NormyZuzycia();
            tableName = "[Normy zużycia]";
        }
    }
}
