using Dapper;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Repositories
{
    public interface ISurowiecRepository:IGenericRepository<Surowiec>
    {
        Task<Surowiec> GetFromName(string name);

    }
    public class SurowiecRepository : GenericRepository<Surowiec>, ISurowiecRepository
    {
        public SurowiecRepository()
        {
            selectAllQuery = SqlSelectQueries.SqlSelectAll_Surowiec();
            tableName = "Surowiec";
        }

        public async Task<Surowiec> GetFromName(string name)
        {
            IEnumerable<Surowiec> surowiece;
            using (var connection = CreateConnection())
            {
               surowiece =  await connection.QueryAsync<Surowiec>(GetFromNameSqlQuery(name));
            }

            return surowiece.First();
        }

        private string GetFromNameSqlQuery(string name)
        {
            var sb = new StringBuilder(selectAllQuery);
            sb.Append($" where [Nazwa surowca] = '{name}'");

            return sb.ToString();
        }

    }
}
