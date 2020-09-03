using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GAT_Produkcja.dbMsAccess.Repositories.GenericRepository
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
                                       where T : class, IIdEntity
    {
        protected string selectAllQuery;
        protected string tableName;
        public GenericRepository()
        {
        }

        #region Connection

        #region Old ConnectionStrings

        //private const string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\192.168.34.57\gtex\10. PRODUKCJA\Program-MsAccess\BAZA.mdb";
        //private const string ConnectionStringCopy = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\192.168.34.57\gat\PRODUKCJA\GEO\KOPIE\BAZA_2020-04-28.mdb";
        //private const string ConnectionStringTests = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Tomasz Strączek\Desktop\BAZA.mdb";

        #endregion

        private OleDbConnection OleDbConnection()
        {
            //Configuration manager gets ConnectionString from app.config which is current in use - in this case this is -> GAT_Produkcja.UI <- assembly
            var sql = new OleDbConnection(ConfigurationManager.ConnectionStrings["BazaPS"].ConnectionString);
            //var sql = new OleDbConnection(ConnectionString);
            return sql;
        }

        /// <summary>
        /// Open new connection and return it for use
        /// </summary>
        /// <returns></returns>
        protected IDbConnection CreateConnection()
        {
            var conn = OleDbConnection();
            conn.OpenAsync();
            return conn;
        }

        #endregion

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (IDbConnection connection = CreateConnection())
            {
                return await connection.QueryAsync<T>(selectAllQuery);
            }
        }


        public async Task<T> GetByIdAsync(T entity)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"{selectAllQuery} WHERE Id=@Id", new { Id = entity.Id });
                if (result == null)
                    throw new KeyNotFoundException($"{tableName} with id [{entity.Id}] could not be found.");

                return result;
            }
        }
    }
}
