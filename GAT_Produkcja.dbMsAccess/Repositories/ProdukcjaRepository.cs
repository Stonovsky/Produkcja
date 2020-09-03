using Dapper;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Repositories
{
    public interface IProdukcjaRepository : IGenericRepository<Produkcja>
    {
        Task<IEnumerable<Produkcja>> GetByDateAsync(DateTime fromDate, DateTime toDate);
    }

    public class ProdukcjaRepository : GenericRepository<Produkcja>, IProdukcjaRepository
    {
        public ProdukcjaRepository()
        {
            selectAllQuery = SqlSelectQueries.SqlSelectAll_Produkcja();
            tableName = "Produkcja";
        }

        public async Task<IEnumerable<Produkcja>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var fromDateFormatted= fromDate.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
            var toDateFormatted= toDate.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);


            var sql = new StringBuilder(selectAllQuery);
            sql.Append("WHERE ");
            sql.Append($" P.Data >= #{fromDateFormatted}# ");
            sql.Append($" AND P.Data <= #{toDateFormatted}# ");
            sql.ToString();

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Produkcja>(sql.ToString());
            }
        }
    }
}
