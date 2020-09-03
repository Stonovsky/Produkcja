using Dapper;
using GAT_Produkcja.dbComarch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbComarch.Repositories
{
    public interface ISurowiecRepository
    {
        Task<SurowiecCenyModel> PobierzSurowiecZIdAsync(int? idSurowca);
        //Task<IEnumerable<SurowiecModel>> PobierzListeSurowcowAsync();
        Task<IEnumerable<SurowiecCenyModel>> PobierzListeSurowcowZCenamiAsync();
        Task<decimal> PobierzCeneJednAsync(int idSurowcaComarch);
        Task<string> PobierzNazweSurowcaAsync(int idSurowcaComarch);
    }

    public class SurowiecRepository : ISurowiecRepository
    {
        public async Task<SurowiecCenyModel> PobierzSurowiecZIdAsync(int? idSurowca)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                return await connection.QuerySingleAsync<SurowiecCenyModel>(SqlPobierzSurowceWrazZCenami(idSurowca));
            }
        }

        public async Task<IEnumerable<SurowiecCenyModel>> PobierzListeSurowcowZCenamiAsync()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                return await connection.QueryAsync<SurowiecCenyModel>(SqlPobierzSurowceWrazZCenami(null));
            }
        }

        public async Task<decimal> PobierzCeneJednAsync(int idSurowcaComarch)
        {
            var surowiec = await PobierzSurowiecZIdAsync(idSurowcaComarch);
            if (surowiec != null)
                return surowiec.CenaJedn;
            else
                return 0;
        }

        public async Task<string> PobierzNazweSurowcaAsync (int idSurowcaComarch)
        {
            var surowiec = await PobierzSurowiecZIdAsync(idSurowcaComarch);

            return surowiec.Nazwa;
        }

        private string SqlPobierzSurowceWrazZCenami(int? idSurowca)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append("TI.TwI_TwrId AS Id ");
            sql.Append(", TI.TwI_TwIId ");
            sql.Append(", M.Mag_MagId AS MagazynId ");
            sql.Append(", M.Mag_Nazwa  AS MagazynNazwa ");
            sql.Append(", T.Twr_Kod AS Kod ");
            sql.Append(", T.Twr_Nazwa AS Nazwa ");
            sql.Append(", TI.TwI_Ilosc AS Ilosc ");
            sql.Append(", T.Twr_JM AS Jm ");
            
            sql.Append(", CASE ");
            sql.Append("WHEN TI.TwI_Ilosc = 0 THEN 0 ");
            sql.Append("ELSE Round(TI.TwI_Wartosc / TI.TwI_Ilosc, 4) ");
            sql.Append("END ");
            sql.Append("AS CenaJedn ");
            
            sql.Append(", TI.TwI_Wartosc  AS Wartosc ");
            sql.Append(", M.Mag_OpeZalNazwisko  AS Operator ");
            sql.Append(", TI.TwI_Data AS [Data] ");
            sql.Append("FROM CDN.TwrIlosci TI ");
            sql.Append("INNER JOIN CDN.Magazyny as M on M.Mag_MagId = TI.TwI_MagId ");
            sql.Append("INNER JOIN CDN.Towary as T on T.Twr_TwrId = TI.TwI_TwrId ");
            
            sql.Append("INNER JOIN "); //--SELECT DO WYZNACZENIA MAX DATY
            sql.Append("(SELECT ");
            sql.Append("TI.TwI_TwrId ");
            sql.Append(", Max(TwI_Data) as MaxDate ");
            sql.Append("FROM CDN.TwrIlosci TI ");
            sql.Append("left join CDN.Magazyny as M    on M.Mag_MagId = TI.TwI_MagId ");
            sql.Append("left join CDN.Towary as T    on T.Twr_TwrId = TI.TwI_TwrId ");
            sql.Append("WHERE ");
            sql.Append("M.Mag_MagId = 8 "); //-- MAGAZYN SUROWCÓW
            sql.Append("GROUP BY  TI.TwI_TwrId ");
            sql.Append(") D on D.MaxDate = TI.TwI_Data and D.TwI_TwrId = TI.TwI_TwrId ");
            
            sql.Append("WHERE ");
            sql.Append("M.Mag_MagId = 8 "); //-- MAGAZYN SUROWCÓW"

            if (idSurowca != null)
                sql.Append($"AND TI.TwI_TwrId = {idSurowca} ");


            return sql.ToString();
        }
    }
}
