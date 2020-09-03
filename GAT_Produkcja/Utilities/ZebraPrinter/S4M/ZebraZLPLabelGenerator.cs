using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public class ZebraZLPLabelGenerator : IZebraZLPLabelGenerator
    {
        /// <summary>
        /// Metoda generuje etykiete w jezyku ZPL
        /// </summary>
        /// <param name="towar"> towar jako klasa <see cref="tblProdukcjaRuchTowar"/></param>
        /// <param name="iloscEtykietDoDruku"> liczba danych etykiet do wydruku</param>
        /// <returns></returns>
        public string GetInternalHorizontalLabel(tblProdukcjaRuchTowar towar, int iloscEtykietDoDruku)
        {

            int horizStartPosLeft = 40;
            int horizontalStep = 380;
            int vertStartPosition = 40;//50;
            int verticalStep = 33;
            int wysCzcionki = 25;
            int szerCzcionki = 24;
            int wysCzcionkiNaglowek = 28;
            int szerCzcionkiNaglowek = 31;

            StringBuilder sb = new StringBuilder();

            // ^FD - poczatek danych - np nr kodu kreskowego
            // ^FS - koniec danych
            sb.AppendLine();
            sb.AppendLine("^XA"); // poczatek etykiety

            //LEWA STRONA
            int vertPosition = vertStartPosition;
            sb.AppendLine($"^FO{horizStartPosLeft - 5},{vertPosition + 5}^GB330,{wysCzcionkiNaglowek + 5},{wysCzcionkiNaglowek + 5}^FS"); // zaczernienie
            sb.AppendLine($"^FT{horizStartPosLeft},{vertPosition += verticalStep}^A0N,{wysCzcionkiNaglowek},{szerCzcionkiNaglowek}^FR^FH\\^FDSUROWIEC: {towar.tblTowarGeowlokninaParametrySurowiec.Skrot}^FS");
            sb.AppendLine($"^FO{horizStartPosLeft - 5},{vertPosition + 5}^GB330,{wysCzcionkiNaglowek + 5},{wysCzcionkiNaglowek + 5}^FS"); // zaczernienie
            sb.AppendLine($"^FT{horizStartPosLeft},{vertPosition += verticalStep}^A0N,{wysCzcionkiNaglowek},{szerCzcionkiNaglowek}^FR^FH\\^FDGRAMATURA [g/m2]: {towar.tblTowarGeowlokninaParametryGramatura.Gramatura}^FS");
            sb.AppendLine($"^FT{horizStartPosLeft},{vertPosition += verticalStep}^A0N,{wysCzcionki},{szerCzcionki}^FH\\^FDSZER.ROL. [m]: {towar.Szerokosc_m:N2}^FS");
            sb.AppendLine($"^FT{horizStartPosLeft},{vertPosition += verticalStep}^A0N,{wysCzcionki},{szerCzcionki}^FH\\^FDNAWOJ [m]: {towar.Dlugosc_m:N2}^FS");
            sb.AppendLine($"^FT{horizStartPosLeft},{vertPosition += verticalStep}^A0N,{wysCzcionki},{szerCzcionki}^FH\\^FDKALANDROWANA: {Kalandrowana(towar.CzyKalandrowana)}^FS");

            //PRAWA STRONA
            vertPosition = vertStartPosition + verticalStep;
            sb.AppendLine($"^FT{horizStartPosLeft + horizontalStep},{vertPosition += verticalStep }^A0N,{wysCzcionki},{szerCzcionki}^FH\\^FDNR.ZLEC.: {towar.NrDokumentu}^FS");
            sb.AppendLine($"^FT{horizStartPosLeft + horizontalStep},{vertPosition += verticalStep}^A0N,{wysCzcionki},{szerCzcionki}^FH\\^FDGNIAZDO: {PobierzGniazdo((GniazdaProdukcyjneEnum)towar.IDProdukcjaGniazdoProdukcyjne)}^FS");
            sb.AppendLine($"^FT{horizStartPosLeft + horizontalStep},{vertPosition += verticalStep}^A0N,{wysCzcionki},{szerCzcionki}^FH\\^FDDATA PROD.: {towar.DataDodania}^FS");
            sb.AppendLine($"^FT{horizStartPosLeft + horizontalStep},{vertPosition += verticalStep}^A0N,{wysCzcionki},{szerCzcionki}^FH\\^FDOPERATOR: {PobierzInicjalyOperatora()}^FS");
            sb.AppendLine($"^BY6,2,182^FT{horizStartPosLeft + 100},{vertPosition += 6 * verticalStep}^BEN,,Y,N^FD{towar.KodKreskowy}^FS"); // barcode (^BE to EAN 13)

            sb.AppendLine($"^PQ{iloscEtykietDoDruku}"); // ilosc etykiet do wydruku
            sb.AppendLine("^XZ"); // koniec etykiety

            return sb.ToString();
        }

        private string Kalandrowana(bool czyKalandrowana)
        {
            if (czyKalandrowana == true)
                return "Tak";
            else
                return "Nie";
        }
        private string PobierzInicjalyOperatora()
        {
            if (UzytkownikZalogowany.Uzytkownik is null) return "TS";
            return UzytkownikZalogowany.Uzytkownik.Imie.Substring(0, 1) + UzytkownikZalogowany.Uzytkownik.Nazwisko.Substring(0, 1);

        }
        private string PobierzGniazdo(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {
            string gniazdo = null;
            switch (gniazdaProdukcyjneEnum)
            {
                case GniazdaProdukcyjneEnum.LiniaWloknin:
                    gniazdo = "W";
                    break;
                case GniazdaProdukcyjneEnum.LiniaDoKalandowania:
                    gniazdo = "KA";
                    break;
                case GniazdaProdukcyjneEnum.LiniaDoKonfekcji:
                    gniazdo = "KO";
                    break;
                default:
                    break;
            }
            return gniazdo;


        }


    }
}
