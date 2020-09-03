using GalaSoft.MvvmLight;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ZebraPrinter
{
    public class ZebraLabelGenerator
    {
        #region EPL language comment
        // tekst str 41
        // barcode str 50
        // barcode 
        //0. Sending an initial newline guarantees that any previous borked
        //      command is submitted.
        //1. [N] Clear the image buffer.This is an important step and
        //      generally should be the first command in any EPL document;
        //      who knows what state the previous job left the printer in.
        //2. [q] Set the label width to 609 dots(3 inch label x 203 dpi
        //      = 609 dots wide).
        //3. [Q] Set the label height to 203 dots(1 inch label) with a 26
        //      dot gap between the labels. (The printer will probably auto-
        //      sense, but this doesn't hurt.)
        //4. [B] Draw a UPC-A barcode with value "603679025109" at
        //      x = 26 dots (1/8 in), y = 26 dots (1/8 in) with a narrow bar
        //      width of 2 dots and make it 152 dots (3/4 in) high. (The
        //      origin of the label coordinate system is the top left corner
        //      of the label.)
        //5. [A]
        //Draw the text "SKU 6205518 MFG 6354" at
        //      x = 253 dots (3/4 in), y = 26 dots (1/8 in) in
        //      printer font "3", normal horizontal and vertical scaling,
        //      and no fancy white-on-black effect.
        //(6 through 9 are similar to line 4.)
        //10. [P]
        //Print one copy of one label. 
        #endregion


        #region EtykietyWewnetrzne
        public string MagazynPrzyjecieZewnetrzne(LabelModel labelModel)
        {

            int horizontalStartPosition = 400;
            int verticalStartPosition = 0;
            int verticalStep = 30;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("N");
            sb.AppendLine("q609");
            sb.AppendLine("Q203,26");
            sb.AppendLine(string.Format(
                CultureInfo.InvariantCulture,
                "B5,5,0,E30,3,3,150,B,\"{0}\"",
                //"B26,26,0,UA0,2,2,152,B,\"{0}\"",
                //"B26,26,0,E30,2,2,152,B,\"{0}\"",
                labelModel.KodKreskowy));
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"{labelModel.Kontrahent.Substring(1, 20)}\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"{labelModel.NazwaTowaru.Substring(1, 20)}\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"TYP:{labelModel.TypTowaru.Substring(1, 15)}\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"ILOSC:{labelModel.Ilosc}\"");

            sb.AppendLine("P1,1");

            return sb.ToString();
        }

        public string EtykietaKonfekcja_Old(LabelModel labelModel)
        {
            if (labelModel == null)
                return string.Empty;

            int horizontalStartPosition = 377;
            int verticalStartPosition = 0;
            int verticalStep = 30;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("N");
            //szer. 9cm = 3,54inch
            sb.AppendLine("q718");
            sb.AppendLine("Q203,26");
            if (!string.IsNullOrEmpty(labelModel.NrZP))
                sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"ZP:{labelModel.NrZP}\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"SUROWIEC:{labelModel.RodzajSurowca}\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"GRAMATURA:{labelModel.Gramatura}g/m2\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"DL.NAWOJU={labelModel.DlugoscNawoju.ToString("N2")}m\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"SZER.ROLKI={labelModel.SzerokoscRolki.ToString("N2")}m\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"PROD:{DateTime.Now.ToString("MM.dd.yyyy HH:mm")}\"");
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"KALANDROWANA:{Kalandrowana(labelModel)}\"");
            if (UzytkownikZalogowany.Uzytkownik != null)
            {
                string inicjalyUzytkownika = UzytkownikZalogowany.Uzytkownik.Imie.Substring(0, 1) + UzytkownikZalogowany.Uzytkownik.Nazwisko.Substring(0, 1);
                sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"PRZEZ: {inicjalyUzytkownika}\"");
            }
            sb.AppendLine($"A{horizontalStartPosition},{verticalStartPosition += verticalStep},0,3,1,1,N,\"GNIAZDO: KONF\"");
            sb.AppendLine($"P{labelModel.IloscEtykietDoDruku},1");

            //KodKreskowy
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "B60,30,0,E30,3,2,220,B,\"{0}\"", labelModel.KodKreskowy));

            return sb.ToString();
        }

        public string EtykietaProdukcja(LabelModel labelModel)
        {
            if (labelModel == null)
                return string.Empty;

            int horizStartPosLeft = 80;
            int horizStartPosRight = 350;
            int verticalStep = 30;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("N");
            sb.AppendLine("q718");
            sb.AppendLine("Q203,26");

            //Kod kreskowy
            if (labelModel.TypKoduKreskowego == TypKoduKreskowegoEnum.EAN13)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "B150,210,0,E30,4,2,150,B,\"{0}\"", labelModel.KodKreskowy));
            }
            else
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "B150,210,0,1,4,2,150,B,\"{0}\"", labelModel.KodKreskowy));
            }

            //Lewa kolumna
            int vertStartPosition = 0;
            sb.AppendLine($"A{horizStartPosLeft},{vertStartPosition += verticalStep},0,3,1,1,R,\"SUROWIEC:{labelModel.RodzajSurowca}\"");
            sb.AppendLine($"A{horizStartPosLeft},{vertStartPosition += verticalStep},0,3,1,1,R,\"GRAMATURA:{labelModel.Gramatura}g/m2\"");
            sb.AppendLine($"A{horizStartPosLeft},{vertStartPosition += verticalStep},0,3,1,1,N,\"SZER.ROL.={labelModel.SzerokoscRolki.ToString("N2")}m\"");
            sb.AppendLine($"A{horizStartPosLeft},{vertStartPosition += verticalStep},0,3,1,1,N,\"NAWOJ={labelModel.DlugoscNawoju.ToString("N2")}m\"");
            sb.AppendLine($"A{horizStartPosLeft},{vertStartPosition += verticalStep},0,3,1,1,N,\"KALANDROWANA:{Kalandrowana(labelModel)}\"");
            var waga = (decimal)labelModel.Gramatura / 1000 * labelModel.SzerokoscRolki * labelModel.DlugoscNawoju;
            sb.AppendLine($"A{horizStartPosLeft},{vertStartPosition += verticalStep},0,3,1,1,N,\"ILOSC ROL.:{labelModel.Ilosc}\"");

            //Prawa kolumna
            if (!string.IsNullOrEmpty(labelModel.NrZP))
            {
                vertStartPosition = 30;
                sb.AppendLine($"A{horizStartPosRight},{vertStartPosition += verticalStep},0,3,1,1,N,\"ZP:{labelModel.NrZP}\"");
            }
            else
            {
                vertStartPosition = 60;
            }

            sb.AppendLine($"A{horizStartPosRight},{vertStartPosition += verticalStep},0,3,1,1,N,\"PROD:{DateTime.Now.ToString("MM.dd.yyyy HH:mm")}\"");

            if (UzytkownikZalogowany.Uzytkownik != null)
            {
                string inicjalyUzytkownika = UzytkownikZalogowany.Uzytkownik.Imie.Substring(0, 1) + UzytkownikZalogowany.Uzytkownik.Nazwisko.Substring(0, 1);
                sb.AppendLine($"A{horizStartPosRight},{vertStartPosition += verticalStep},0,3,1,1,N,\"PRZEZ: {inicjalyUzytkownika}\"");
            }
            sb.AppendLine($"A{horizStartPosRight},{vertStartPosition += verticalStep},0,3,1,1,N,\"GNIAZDO: {PobierzGniazdo(labelModel.GniazdoProdukcyjne)}\"");
            sb.AppendLine($"P{labelModel.IloscEtykietDoDruku},1");


            return sb.ToString();
        }

        private string Kalandrowana(LabelModel labelModel)
        {
            if (labelModel.Kalandrowana == true)
                return "Tak";
            else
                return "Nie";
        }

        private string PobierzGniazdo(GniazdaProdukcyjneEnum gniazdaProdukcyjneEnum)
        {
            string gniazdo = null;
            switch (gniazdaProdukcyjneEnum)
            {
                case GniazdaProdukcyjneEnum.LiniaWloknin:
                    gniazdo = "Wloknina";
                    break;
                case GniazdaProdukcyjneEnum.LiniaDoKalandowania:
                    gniazdo = "Kalander";
                    break;
                case GniazdaProdukcyjneEnum.LiniaDoKonfekcji:
                    gniazdo = "Konfekcja";
                    break;
                default:
                    break;
            }
            return gniazdo;


        }

        #endregion


        #region Drukowanie
        public string EtykietaCE_PL(LabelModel labelModel, tblTowarGeowlokninaParametry towarParametry)
        {
            string znakCE = "˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙Ŕ˙˙˙ ˙˙˙˙ţ ˙˙ř ˙˙˙˙ř ˙˙ŕ ˙˙˙˙ŕ ˙˙Ŕ ˙˙˙˙€ ˙˙  ˙˙˙˙  ˙ţ  ˙˙˙ţ ˙˙ř ˙˙˙ü ˙˙ř ˙˙˙˙ü˙˙˙đ˙˙˙˙ř˙˙˙ŕ˙˙˙˙đ˙˙˙Ŕ˙˙˙˙đ˙˙˙Ŕ?˙˙˙˙ŕ˙˙˙€˙˙˙˙ŕ?˙˙˙€˙˙˙˙˙Ŕ˙˙˙ ˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙€˙˙˙˙˙˙˙˙˙€˙˙˙˙˙˙˙˙˙€˙˙˙ţ  ˙˙˙˙˙˙ţ  ˙˙˙˙˙˙ţ  ˙˙˙˙˙˙ţ  ˙˙˙€˙˙˙ţ  ˙˙˙€˙˙˙ţ  ˙˙˙€˙˙˙ţŞż˙˙˙€˙˙˙˙˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙Ŕ?˙˙˙€˙˙˙˙˙ŕ?˙˙˙€˙˙˙˙˙ŕ˙˙˙€˙˙˙˙đ˙˙˙Ŕ?˙˙˙˙đ˙˙˙Ŕ˙˙˙˙ř˙˙˙ŕ˙˙˙˙ü˙˙˙đ˙˙˙˙ü ˙˙đ ˙˙˙˙ţ ˙˙ř ˙˙˙˙  ˙ü  ˙˙˙˙€ ˙˙  ˙˙˙˙Ŕ ˙˙€ ˙˙˙˙đ ˙˙ŕ ˙˙˙˙ü ˙˙đ ˙˙˙˙˙Ŕ˙˙˙ ˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙";

            var marginesPionowy = 30;
            var marginesPoziomy = 10;
            var skokPionowy = 14;
            var dlugoscL = 720;
            var gruboscL = 3;
            //var nrCertyfikatu = "1488-CPR-0519/Z";
            var rokWprowadzeniaWyrobuDoObrotu = "18";
            //maks ilosc znakow 71 przy x=10

            string str = $"\r\n";
            str += $"N\r\n";
            str += $"ZB\r\n"; //Orientacja pionowa calej etykiety, parametry: ZB i ZT
            str += $"D10\r\n"; //Ustawia density, czyli wartość temperatury generowanej przez glowice - im wyzsza wartosc tym ciemniejsze labele
            str += $"I8,A,003\r\n"; //Ustawia rodzaj liter, czyli jezyk tekstu, manual str 110, brak j. polskiego i polskich znakow
                                    //str += $"q718\r\n"; ;
                                    //str += $"Q203,26\r\n";

            #region ZnakCE
            str += $"GW{marginesPoziomy + 310},{marginesPionowy},11,63,{znakCE}\r\n";
            str += $"A{marginesPoziomy + 280},{marginesPionowy += 60},0,1,1,1,N,\"{towarParametry.tblCertyfikatCE.NumerCertyfikatu}\"\r\n";
            str += $"A{marginesPoziomy + 340},{marginesPionowy += skokPionowy},0,1,1,1,N,\"{rokWprowadzeniaWyrobuDoObrotu}\"\r\n";//tutaj ma byc stala?
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            #endregion

            #region Firma
            str += $"A{marginesPoziomy + 150},{marginesPionowy += skokPionowy},0,1,1,1,N,\"EMG Geosynthetics Sp. z o.o.\"\r\n";
            str += $"A{marginesPoziomy + 150},{marginesPionowy += skokPionowy},0,1,1,1,N,\"ul. Jaskólek 12, 43-215 Studzienice\"\r\n";
            str += $"A{marginesPoziomy + 230},{marginesPionowy += skokPionowy},0,1,1,1,N,\"DoP – {"ZMIENNA"}\"\r\n";//co tutaj?
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            #endregion

            #region Normy
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"EN 13249:2016, EN 13250:2016, EN 13251:2016, EN 13252:2016,\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"EN 13253:2016,EN 13254:2016,EN 13255:2016, EN 13257:2016, EN 13265:2016\"\r\n";
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            #endregion

            #region Parametry
            str += $"A{marginesPoziomy + 200},{marginesPionowy += skokPionowy},0,1,1,1,N,\"{towarParametry.tblTowar.Nazwa}\"\r\n";//zmienilem
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Zastosowanie: do budowy dróg i innych powierzchni obciazonych ruchem\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"(z wylaczeniem nawierzchni asfaltowych), do budowy drog kolejowych,w\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"robotach ziemnych, fundamentowaniu i konstrukcjach oporowych, w\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"systemach drenazowych, w zabezpieczeniach przeciwerozyjnych(ochrona i\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"umocnienia brzegow),do budowy zbiornikow wodnych i zapor,\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"do budowy kanalow, do budowy skladowisk odpadow stalych,\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"do budowy zbiornikow odpadow cieklych;\"\r\n";
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            str += $"A{marginesPoziomy + 230},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Funkcje: (F + S)\"\r\n";
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Rozmiar rolki[mm]: Dl: {labelModel.DlugoscNawoju} ± {"ZM"}% Szer: {labelModel.SzerokoscRolki}± {"ZM"} Waga:{"ZM"} ±kg\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wytrzymalosc na rozciaganie(EN ISO 10319): MD {towarParametry.WytrzymaloscNaRozciaganie_MD}(-{towarParametry.WytrzymaloscNaRozciaganie_MD - towarParametry.WytrzymaloscNaRozciaganie_MD_Minimum}) kN/m\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wytrzymalosc na rozciaganie(EN ISO 10319): CMD {towarParametry.WytrzymaloscNaRozciaganie_CMD}(- {towarParametry.WytrzymaloscNaRozciaganie_CMD - towarParametry.WytrzymaloscNaRozciaganie_CMD_Minimum}) kN/m\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wydluzenie przy maksymalnym obciazeniu(EN ISO 10319):MD {towarParametry.WydluzeniePrzyZerwaniu_MD}(+-{towarParametry.WydluzeniePrzyZerwaniu_MD_Maksimum - towarParametry.WydluzeniePrzyZerwaniu_MD_Minimum})%\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wydluzenie przy maksymalnym obciazeniu(EN ISO 10319):CMD {towarParametry.WydluzeniePrzyZerwaniu_CMD}(+-{towarParametry.WydluzeniePrzyZerwaniu_CMD_Maksimum - towarParametry.WydluzeniePrzyZerwaniu_CMD_Minimum})%\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Odpornosc na przebicie statyczne[CBR] (EN ISO 12236): {towarParametry.OdpornoscNaPrzebicieStatyczne_CBR}(-{towarParametry.OdpornoscNaPrzebicieStatyczne_CBR - towarParametry.OdpornoscNaPrzebicieStatyczne_CBR_Minimum})kN\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Odpornosc na przebicie dynamiczne(EN ISO 13433): {towarParametry.OdpornoscNaPrzebicieDynamiczne}(+{towarParametry.OdpornoscNaPrzebicieDynamiczne_Maksimum - towarParametry.OdpornoscNaPrzebicieDynamiczne})mm\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Charakterystyczna wielkosc porow , O90(EN ISO 12956):{towarParametry.CharakterystycznaWielkoscPorow} (+-{towarParametry.CharakterystycznaWielkoscPorow_Maksimum - towarParametry.CharakterystycznaWielkoscPorow_Minimum})um\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wodoprzepuszczalnosc prostopadla(EN ISO 11058):{towarParametry.WodoprzepuszczalnoscProsotpadla}(-{towarParametry.WodoprzepuszczalnoscProsotpadla - towarParametry.WodoprzepuszczalnoscProsotpadla_Minimum})mm/s\"\r\n";
            str += $"A240,{marginesPionowy += skokPionowy},0,1,1,1,N,\"Trwalosc:\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"- Nalezy zakryc gruntem w ciagu jednego dnia po wbudowaniu.\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"- Przewidywana trwalosc co najmniej 5 lat w gruntach naturalnych o\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"4<pH<9  i w gruncie o temperaturze <25°C,jezeli produkt nie będzie\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"pelnil funkcji zbrojenia”\"\r\n";
            #endregion

            #region Kod Kreskowy
            str += $"B150,{marginesPionowy += skokPionowy},0,E30,4,2,100,B,\"{labelModel.KodKreskowy}\"\r\n";//zmienilem
            #endregion

            #region Ilosc sztuk
            str += $"P1,1";
            #endregion

            return str;
        }
        public string EtykietaCEZPolskimiZnakami(LabelModel labelModel, tblTowarGeowlokninaParametry towarParametry)
        {
            string znakCE = "˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙Ŕ˙˙˙ ˙˙˙˙ţ ˙˙ř ˙˙˙˙ř ˙˙ŕ ˙˙˙˙ŕ ˙˙Ŕ ˙˙˙˙€ ˙˙  ˙˙˙˙  ˙ţ  ˙˙˙ţ ˙˙ř ˙˙˙ü ˙˙ř ˙˙˙˙ü˙˙˙đ˙˙˙˙ř˙˙˙ŕ˙˙˙˙đ˙˙˙Ŕ˙˙˙˙đ˙˙˙Ŕ?˙˙˙˙ŕ˙˙˙€˙˙˙˙ŕ?˙˙˙€˙˙˙˙˙Ŕ˙˙˙ ˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙€˙˙˙˙˙˙˙˙˙€˙˙˙˙˙˙˙˙˙€˙˙˙ţ  ˙˙˙˙˙˙ţ  ˙˙˙˙˙˙ţ  ˙˙˙˙˙˙ţ  ˙˙˙€˙˙˙ţ  ˙˙˙€˙˙˙ţ  ˙˙˙€˙˙˙ţŞż˙˙˙€˙˙˙˙˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙˙Ŕ˙˙˙˙˙˙˙˙Ŕ?˙˙˙€˙˙˙˙˙ŕ?˙˙˙€˙˙˙˙˙ŕ˙˙˙€˙˙˙˙đ˙˙˙Ŕ?˙˙˙˙đ˙˙˙Ŕ˙˙˙˙ř˙˙˙ŕ˙˙˙˙ü˙˙˙đ˙˙˙˙ü ˙˙đ ˙˙˙˙ţ ˙˙ř ˙˙˙˙  ˙ü  ˙˙˙˙€ ˙˙  ˙˙˙˙Ŕ ˙˙€ ˙˙˙˙đ ˙˙ŕ ˙˙˙˙ü ˙˙đ ˙˙˙˙˙Ŕ˙˙˙ ˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙˙";

            var marginesPionowy = 30;
            var marginesPoziomy = 10;
            var skokPionowy = 14;
            var dlugoscL = 720;
            var gruboscL = 3;

            //maks ilosc znakow 71 przy x=10

            //str += $"GW640,{marginesPionowy},11,63,{znakCE}\r\n";
            string str = $"\r\n";
            str += $"N\r\n";
            //str += $"q718\r\n"; ;
            //str += $"Q203,26\r\n";
            str += $"GW{marginesPoziomy + 630},{marginesPionowy},11,63,{znakCE}\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy},0,1,1,1,N,\"EMG Plast Group Sp. z o.o., ul. Jaskółek 12, 43-215 Studzienice\"\r\n";
            str += $"A{marginesPoziomy + 230},{marginesPionowy += skokPionowy},0,1,1,1,N,\"DoP – {"ZMIENNA"}\"\r\n";//co tutaj?
            str += $"A{marginesPoziomy + 230},{marginesPionowy += skokPionowy},0,1,1,1,N,\"\r\n";
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"EN 13249:2016, EN 13250:2016, EN 13251:2016, EN 13252:2016,\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"EN 13253:2016,EN 13254:2016,EN 13255:2016, EN 13257:2016, EN 13265:2016\"\r\n";
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            str += $"A{marginesPoziomy + 200},{marginesPionowy += skokPionowy},0,1,1,1,N,\"{towarParametry.tblTowar.Nazwa}\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Zastosowanie: do budowy dróg i innych powierzchni obciążonych ruchem\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"(z wyłączeniem nawierzchni asfaltowych), do budowy dróg kolejowych,w\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"robotach ziemnych, fundamentowaniu i konstrukcjach oporowych, w\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"systemach drenażowych, w zabezpieczeniach przeciwerozyjnych(ochrona i\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"umocnienia brzegów),do budowy zbiorników wodnych i zapór,\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"do budowy kanałów, do budowy składowisk odpadów stałych,\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"do budowy zbiorników odpadów ciekłych;\"\r\n";
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            str += $"A{marginesPoziomy + 230},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Funkcje: (F + S)\"\r\n";
            str += $"LO{marginesPoziomy},{marginesPionowy += skokPionowy},{dlugoscL},{gruboscL}\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Rozmiar rolki:  Długość, mm: {labelModel.DlugoscNawoju} ± {"ZM"}%\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Szerokość, mm: {labelModel.SzerokoscRolki}± {"ZM"}% Waga rolki: {"ZM"} ±kg\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wytrzymałość na rozciąganie(EN ISO 10319): MD {towarParametry.WytrzymaloscNaRozciaganie_MD}(-{towarParametry.WytrzymaloscNaRozciaganie_MD - towarParametry.WytrzymaloscNaRozciaganie_MD_Minimum}) kN / m\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wytrzymałość na rozciąganie(EN ISO 10319): CMD {towarParametry.WytrzymaloscNaRozciaganie_CMD}(- {towarParametry.WytrzymaloscNaRozciaganie_CMD - towarParametry.WytrzymaloscNaRozciaganie_CMD_Minimum}) kN / m\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wydłużenie przy maksymalnym obciążeniu(EN ISO 10319):MD {towarParametry.WydluzeniePrzyZerwaniu_MD}(+-{towarParametry.WydluzeniePrzyZerwaniu_MD_Maksimum - towarParametry.WydluzeniePrzyZerwaniu_MD_Minimum})%\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wydłużenie przy maksymalnym obciążeniu(EN ISO 10319):CMD {towarParametry.WydluzeniePrzyZerwaniu_CMD}(+-{towarParametry.WydluzeniePrzyZerwaniu_CMD_Maksimum - towarParametry.WydluzeniePrzyZerwaniu_CMD_Minimum})%\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Odporność na przebicie statyczne[CBR] (EN ISO 12236): {towarParametry.OdpornoscNaPrzebicieStatyczne_CBR}(-{towarParametry.OdpornoscNaPrzebicieStatyczne_CBR - towarParametry.OdpornoscNaPrzebicieStatyczne_CBR_Minimum})kN\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Odporność na przebicie dynamiczne(EN ISO 13433): {towarParametry.OdpornoscNaPrzebicieDynamiczne}(+{towarParametry.OdpornoscNaPrzebicieDynamiczne_Maksimum - towarParametry.OdpornoscNaPrzebicieDynamiczne})mm\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Charakterystyczna wielkość porów , O90(EN ISO 12956):{towarParametry.CharakterystycznaWielkoscPorow} (+-{towarParametry.CharakterystycznaWielkoscPorow_Maksimum - towarParametry.CharakterystycznaWielkoscPorow_Minimum})um\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"Wodoprzepuszczalność prostopadła(EN ISO 11058):{towarParametry.WodoprzepuszczalnoscProsotpadla}(-{towarParametry.WodoprzepuszczalnoscProsotpadla - towarParametry.WodoprzepuszczalnoscProsotpadla_Minimum})mm / s\"\r\n";
            str += $"A240,{marginesPionowy += skokPionowy},0,1,1,1,N,\"Trwałość:\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"- Należy zakryć gruntem w ciągu jednego dnia po wbudowaniu.\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"- Przewidywana trwałość co najmniej 5 lat w gruntach naturalnych o\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"4≤pH≤9  i w gruncie o temperaturze ≤25°C,jeżeli produkt nie będzie\"\r\n";
            str += $"A{marginesPoziomy},{marginesPionowy += skokPionowy},0,1,1,1,N,\"pełnił funkcji zbrojenia”\"\r\n";
            str += $"A240,{marginesPionowy += skokPionowy},0,1,1,1,N,\"Nr. Partii:{"ZMIENNA"}\"\r\n";//co tutaj?
            str += $"P1,1";


            return str;
        }

        #endregion

    }

}
