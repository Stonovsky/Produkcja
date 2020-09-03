using Dapper;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Repositories
{
    public interface IKalanderRepository : IGenericRepository<Kalander>
    {
        /// <summary>
        /// Get the list of <see cref="Kalander"/> between dates
        /// </summary>
        /// <param name="fromDate">from date</param>
        /// <param name="toDate">to date</param>
        /// <returns></returns>
        Task<IEnumerable<Kalander>> GetByDateAsync(DateTime fromDate, DateTime toDate);
    }

    public class KalanderRepository : GenericRepository<Kalander>, IKalanderRepository
    {
        public KalanderRepository()
        {
            selectAllQuery = SqlSelectQueries.SqlSelectAll_Kalander();
            tableName = "Kalander";
        }

        public async Task<IEnumerable<Kalander>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var fromDateFormatted = fromDate.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
            var toDateFormatted = toDate.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);

            var sql = new StringBuilder(selectAllQuery);
            sql.Append("WHERE ");
            sql.Append($" K.Data >= #{fromDateFormatted}# ");
            sql.Append($" AND K.Data <= #{toDateFormatted}# ");
            sql.ToString();

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Kalander>(sql.ToString());
            }
        }
    }
}
