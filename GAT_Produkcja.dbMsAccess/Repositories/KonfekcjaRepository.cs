using Dapper;
using GAT_Produkcja.db;
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
    public interface IKonfekcjaRepository : IGenericRepository<Konfekcja>
    {
        Task UpdateRangeZaksiegowano(IEnumerable<Konfekcja> entities);
        Task UpdateRangeNieZaksiegowano(IEnumerable<tblProdukcjaRozliczenie_PW> entities);
        Task<IEnumerable<Konfekcja>> GetByParametersAsync(decimal szerokosc_m, decimal dlugosc_m, string nazwaTowaru, DateTime dataProdukcjiFinalna, bool czyZaksiegowano = false);
        Task<IEnumerable<Konfekcja>> GetByZlecenieIdAsync(int idZlecenie);
        Task<IEnumerable<Konfekcja>> GetByDateAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Konfekcja>> GetUnaccountedAsync();
        Task<IEnumerable<Konfekcja>> GetByNrSztukiAsync(IEnumerable<string> listaNumerowSztuk);
        IEnumerable<Konfekcja> GetByNrSztuki(IEnumerable<string> listaNumerowSztuk);

    }

    public class KonfekcjaRepository : GenericRepository<Konfekcja>, IKonfekcjaRepository
    {

        public KonfekcjaRepository()
        {
            selectAllQuery = SqlSelectQueries.SqlSelectAll_Konfekcja();
            tableName = "Konfekcja";
        }

        #region Update CzyZaksiegowano
        #region Zaksiegowano

        public async Task UpdateRangeZaksiegowano(IEnumerable<Konfekcja> entities)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(GenerateUpdateZaksiegowanoQuery(entities));
            }
        }

        private string GenerateUpdateZaksiegowanoQuery(IEnumerable<Konfekcja> entities)
        {
            var updateQuery = new StringBuilder($"UPDATE Konfekcja SET CzyZaksiegowano=True WHERE Id IN (");

            foreach (var entity in entities)
            {
                updateQuery.Append($"{entity.Id}, ");
            }
            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(")");

            return updateQuery.ToString();
        }

        #endregion
        #region Niezaksiegowano
        public async Task UpdateRangeNieZaksiegowano(IEnumerable<tblProdukcjaRozliczenie_PW> entities)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(GenerateUpdateNieZaksiegowanoQuery(entities));
            }
        }

        private string GenerateUpdateNieZaksiegowanoQuery(IEnumerable<tblProdukcjaRozliczenie_PW> entities)
        {
            var updateQuery = new StringBuilder($"UPDATE Konfekcja SET CzyZaksiegowano=False WHERE Id IN (");

            foreach (var entity in entities)
            {
                updateQuery.Append($"{entity.IDMsAccess}, ");
            }
            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(")");

            return updateQuery.ToString();
        }

        #endregion
        public async Task<IEnumerable<Konfekcja>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var fromDateFormatted = fromDate.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);
            var toDateFormatted = toDate.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);

            var sql = new StringBuilder(selectAllQuery);
            sql.Append("WHERE ");
            sql.Append($" K.Data>=#{fromDateFormatted}# ");
            sql.Append($" AND K.Data<=#{toDateFormatted}# ");
            sql.ToString();

            using (IDbConnection connection = CreateConnection())
            {
                return await connection.QueryAsync<Konfekcja>(sql.ToString());
            }
        }

        public async Task<IEnumerable<Konfekcja>> GetByParametersAsync(decimal szerokosc_m, decimal dlugosc_m, string nazwaTowaru, DateTime dataProdukcjiFinalna, bool czyZaksiegowano = false)
        {

            using (IDbConnection connection = CreateConnection())
            {
                return await connection.QueryAsync<Konfekcja>(sqlGetByParameters(szerokosc_m, dlugosc_m, nazwaTowaru, dataProdukcjiFinalna, czyZaksiegowano));
            }
        }

        public async Task<IEnumerable<Konfekcja>> GetByZlecenieIdAsync(int idZlecenie)
        {
            using (var connection = CreateConnection())
            {
                var query = $"{selectAllQuery} WHERE K.[Nr zlecenia]={idZlecenie}";
                var result = await connection.QueryAsync<Konfekcja>($"{selectAllQuery} WHERE K.[Nr zlecenia]=@Id", new { Id = idZlecenie });
                return result;
            }

        }

        public async Task<IEnumerable<Konfekcja>> GetUnaccountedAsync()
        {
            using (var connection = CreateConnection())
            {
                var query = $"{selectAllQuery} WHERE K.[CzyZaksiegowano]=0";
                var result = await connection.QueryAsync<Konfekcja>(query);
                return result;
            }

        }

        private string sqlGetByParameters(decimal szerokosc_m, decimal dlugosc_m, string nazwaTowaru, DateTime dataProdukcjiFinalna, bool czyZaksiegowano = false)
        {
            var sql = new StringBuilder();

            var szerokosc_cm = Convert.ToInt16(szerokosc_m * 100);
            var dlugosc = Convert.ToInt16(dlugosc_m).ToString().Replace(",", ".");
            var s = szerokosc_cm.ToString().Replace(",", ".");
            var dataSformatowana = dataProdukcjiFinalna.Date.ToString("MM/dd/yy", CultureInfo.InvariantCulture);

            sql.Append("SELECT ");
            sql.Append(" K.Id ");
            sql.Append(", K.[Nr zlecenia] AS ZlecenieID ");
            sql.Append(", K.[Nr sztuki] As NrSztuki ");
            sql.Append(", K.Data ");
            sql.Append(", K.Godz AS Godzina ");
            sql.Append(", K.Artykuł AS Artykul ");
            sql.Append(", Numer ");
            sql.Append(", NumerMG ");
            sql.Append(", Operator AS OperatorID ");
            sql.Append(", [Pomoc operatora] AS PomocOperatoraID ");
            sql.Append(", Szerokość AS Szerokosc ");
            sql.Append(", Długość AS Dlugosc ");
            sql.Append(", Waga ");
            sql.Append(", Gatunek ");
            sql.Append(", Odpad ");
            sql.Append(", [Waga odpad] AS WagaOdpadu ");
            sql.Append(", M2 AS IloscM2 ");
            sql.Append(", DataWG ");
            sql.Append(", [Nr WZ] AS NrWz ");
            sql.Append(", CzyZaksiegowano ");
            sql.Append(", Przychody ");
            sql.Append(", D.[Nr Zlecenia] AS Zlecenie ");

            sql.Append("FROM Konfekcja K");
            sql.Append(" LEFT JOIN Dyspozycje D ON D.Id=K.[Nr zlecenia] ");

            sql.Append($" WHERE CzyZaksiegowano={czyZaksiegowano} ");
            sql.Append($" AND K.Artykuł LIKE '{nazwaTowaru}' ");
            sql.Append($" AND K.Szerokość='{s}' ");
            sql.Append($" AND K.Długość={dlugosc} ");
            sql.Append($" AND K.Data<=#{dataSformatowana}# ");

            var se = sql.ToString();
            return sql.ToString();
        }

        public async Task<IEnumerable<Konfekcja>> GetByNrSztukiAsync(IEnumerable<string> listaNumerowSztuk)
        {

            using (var connection = CreateConnection())
            {
                var result = await connection.QueryAsync<Konfekcja>(SelectByNrSztukiQuery(listaNumerowSztuk));
                return result;
            }

        }

        public IEnumerable<Konfekcja> GetByNrSztuki(IEnumerable<string> listaNumerowSztuk)
        {

            using (var connection = CreateConnection())
            {
                var result = connection.Query<Konfekcja>(SelectByNrSztukiQuery(listaNumerowSztuk));
                return result;
            }

        }

        private string SelectByNrSztukiQuery(IEnumerable<string> listaNumerowSztuk)
        {
            StringBuilder selectQuery = new StringBuilder($"{selectAllQuery} WHERE [Nr Sztuki] IN (");

            foreach (var nrSztuki in listaNumerowSztuk)
            {
                selectQuery.Append($"\'{nrSztuki}\',");
            }
            selectQuery.Remove(selectQuery.Length - 1, 1); //remove last comma
            selectQuery.Append(")");

            return selectQuery.ToString();

        }

        #endregion
    }
}
