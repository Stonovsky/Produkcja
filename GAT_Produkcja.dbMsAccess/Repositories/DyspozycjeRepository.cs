using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Repositories
{
    public interface IDyspozycjeRepository:IGenericRepository<Dyspozycje>
    {
    }

    public class DyspozycjeRepository : GenericRepository<Dyspozycje>, IDyspozycjeRepository
    {
        public DyspozycjeRepository()
        {
            tableName = "Dyspozycje";
            selectAllQuery = SqlSelectQueries.SqlSelectAll_Dyspozycje();
        }
    }
}
