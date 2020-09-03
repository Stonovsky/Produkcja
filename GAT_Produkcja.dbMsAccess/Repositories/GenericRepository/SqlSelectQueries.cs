using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Repositories.GenericRepository
{
    public static class SqlSelectQueries
    {
        public static string SqlSelectAll_Produkcja()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" P.Id ");
            sql.Append(", P.[Nr zlecenia] AS ZlecenieID ");
            sql.Append(", [Nr sztuki] As NrSztuki ");
            sql.Append(", P.Data ");
            sql.Append(", P.Godz AS Godzina ");
            sql.Append(", P.Artykuł AS Artykul ");
            sql.Append(", Operator AS OperatorID ");
            sql.Append(", [Pomoc operatora] AS PomocOperatoraID ");
            sql.Append(", CInt (Szerokość) AS Szerokosc ");
            sql.Append(", Długość AS Dlugosc ");
            sql.Append(", Waga ");
            sql.Append(", [Gramatura 1] AS Gramatura1 ");
            sql.Append(", [Gramatura 2] AS Gramatura2 ");
            sql.Append(", [Gramatura 3] AS Gramatura3 ");
            sql.Append(", [Waga odp] AS WagaOdpadu ");
            sql.Append(", Postój AS Postoj ");
            sql.Append(", Odpad ");
            sql.Append(", Kalandrowanie AS CzyProduktKalandowany ");
            sql.Append(", D.[Nr Zlecenia] AS Zlecenie ");
            sql.Append(" FROM Produkcja P ");
            sql.Append(" LEFT JOIN Dyspozycje D ON D.Id=P.[Nr zlecenia] ");

            return sql.ToString();
        }


        public static string SqlSelectAll_Kalander()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" K.Id ");
            sql.Append(", K.[Nr zlecenia] AS ZlecenieID ");
            sql.Append(", K.[Nr sztuki] As NrSztuki ");
            sql.Append(", K.[NumerMG] As NumerMG ");
            sql.Append(", [Szer igł] As SzerokoscIgiel ");
            sql.Append(", [Waga igł] AS WagaIgiel ");
            sql.Append(", [Długość igł] AS DlugoscIgiel ");
            sql.Append(", K.Data ");
            sql.Append(", K.Godz AS Godzina ");
            sql.Append(", K.Artykuł AS Artykul ");
            sql.Append(", Operator AS OperatorID ");
            sql.Append(", [Pomoc operatora] AS PomocOperatoraID ");
            sql.Append(", Szerokość AS Szerokosc ");
            sql.Append(", Długość AS Dlugosc ");
            sql.Append(", Waga ");
            sql.Append(", [Waga krajki] AS WagaKrajki ");
            sql.Append(", Odpad ");
            sql.Append(", [Waga odpad] AS WagaOdpadu ");
            sql.Append(", [Kod odpadu] AS KodOdpadu");
            sql.Append(", Konfekcja ");
            sql.Append(", M2 AS IloscM2 ");
            sql.Append(", Przychody ");
            sql.Append(", D.[Nr Zlecenia] AS Zlecenie ");
            sql.Append("FROM Kalander K");
            sql.Append(" LEFT JOIN Dyspozycje D ON D.Id=K.[Nr zlecenia] ");

            return sql.ToString();
        }


        public static string SqlSelectAll_Konfekcja()
        {
            var sql = new StringBuilder();
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

            return sql.ToString();
        }

        public static string SqlSelectAll_NormyZuzycia()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" N.Id ");
            sql.Append(", N.[Nr zlecenia] AS ZlecenieID ");
            sql.Append(", N.Artykuł AS Artykul ");
            sql.Append(", N.Dostawca ");
            sql.Append(", N.Surowiec ");
            sql.Append(", Ilość AS Ilosc ");
            sql.Append(", D.[Nr Zlecenia] AS Zlecenie ");
            sql.Append("FROM [Normy zużycia] N ");
            sql.Append(" LEFT JOIN Dyspozycje D ON D.Id=N.[Nr zlecenia] ");

            return sql.ToString();
        }

        public static string SqlSelectAll_Surowiec()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" Id ");
            sql.Append(", [Nazwa surowca] AS NazwaSurowca ");
            sql.Append("FROM [Surowiec]");

            return sql.ToString();
        }

        public static string SqlSelectAll_Artykuly()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" Id ");
            sql.Append(", [Nazwa_art] AS NazwaArtykulu");
            sql.Append(", [Opis] ");
            //sql.Append(", [Stawka-operator] AS StawkaOperator");
            //sql.Append(", [Stawka-Stawka-pomoc] AS StawkaPomoc");
            sql.Append("FROM [Artykuły]");

            return sql.ToString();
        }
        public static string SqlSelectAll_Dyspozycje()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" Id ");
            sql.Append(", [Data] ");
            sql.Append(", [Nr zlecenia] AS NrZlecenia ");
            sql.Append(", [Artykuł] AS Artykul ");
            sql.Append(", [Ilość mb] AS Ilosc_m2 ");
            sql.Append(", [Zakończ] AS CzyZakonczone ");
            sql.Append("FROM [Dyspozycje]");

            return sql.ToString();
        }
    }
}