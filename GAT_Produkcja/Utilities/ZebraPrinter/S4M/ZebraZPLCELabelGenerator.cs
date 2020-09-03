using GAT_Produkcja.db;
using GAT_Produkcja.db.Migrations;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ZebraPrinter.S4M
{
    public class ZebraZPLCELabelGenerator : IZebraZPLCELabelGenerator
    {
        private readonly IUnitOfWork unitOfWork;
        #region CTOR
        public ZebraZPLCELabelGenerator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        /// <summary>
        /// Generuje Etykiete CE w oparciu o dane z <see cref="tblProdukcjaRuchTowar"/> oraz <see cref="tblTowarGeowlokninaParametry"/> 
        /// </summary>
        /// <param name="towar"></param>
        /// <param name="ilosc"></param>
        /// <returns></returns>
        public async Task<string> GetLabelCE(tblProdukcjaRuchTowar towar, int ilosc, bool czyUV=false)
        {
            var parametry = await unitOfWork.tblTowarGeowlokninaParametry.SingleOrDefaultAsync
                (e => e.IDTowarGeowlokninaParametryGramatura == towar.IDGramatura
                   && e.IDTowarGeowlokninaParametrySurowiec == towar.IDTowarGeowlokninaParametrySurowiec
                   && e.CzyBadanieAktualne == true
                   && e.CzyUV == czyUV);

            string CE = "~DG000.GRF,01024,016,,::::::::::::::::::::::gI01,H0C0C38700F3C78071E3C3837E,01C1C6CD81FBE7C0D9E3E6C33E,014144C981833660990064C20C,H043468D01032641890066860C,H0424387010367C189F0C38618,H04646CDBD03C6DF883186C410,H047EC58818306C198330C4C30,H0404CD981DB0660D8330CHC60,01F0478F00F30660F1E3E7887E,gH018,:,:::::R060E0,R0E1B0,R0A130,R021A0,R020E0,R021B0,R02310,R02330,R0F9E0,,:::::::::::::~DG001.GRF,01920,020,,::::::::::::T0150Q0H140,T0BF80P02BE0,R017FFC0O07FHF0,R03FHFC0N02FHFE0,Q01FIFC0M017FIF0,Q03BFHF80N0JFE0,P017FIFC0M07FJF0,P02FJFC0M0KFE0,P07FJFC0L01FKF0,P0LFC0L03FJFE0,O01FJF540K017FJF60,O03FHFE80N0JF80,O07FHFD0N01FHFD,O03FFE0O03FFB8,N01FHFC0O07FHF0,O0IF80O0IF80,N01FHFP01FHF,N03FFC0O01FFE,N07FFC0O07FFC,N03FF80O03FF8,N07FF0P07FF0,N0HFE0P03FE0,M01FFE0P07FF0,N0HFE0P0HFE0,M01FFC0O01FFC0,M01FF80P0HF80,M05FFC0O01FFC0,M03FF80P0HF80,M01FF0P01FFC0,M03FF0P01FFAJA0,M07FF0P01FLF0,M03FE0P03FLF0,M07FF0P01FLF0,M03FE0P03FLF0,M07FF0P07FLF0,M03FE0P03FLF0,M07FF0P01FLF0,M03FE0P01FLF0,M07FF0P01FLF0,M03FF0P01FF80,M01FF0P01FFC0,M01FF80P0HF80,M01FFC0O01FFC0,N0HF80P0HFC0,M01FFC0O01FFC0,N0HF80P0BFE0,N0IFQ07FF0,N0HFE0P07FF8,N07FF0P07FFC,N03FF80O03FF8,N07FFC0O01FHF,N03FFE0P0HFE,N01FHFP01FHF80,O0IF80O0IF80,N047FHFP07FHF4,O03FHF80N03FHF8,O01FIFO01FIF40,P0JFA80M0JFE8,O017FJFC0L07FJFE0,P03FJFC0L03FJFE0,P01FJFC0L01FJFE0,Q0KF80M0KFE0,Q07FIFC0M07FIFE0,R0JFC0N0JFE0,R07FHFC0N07FIF0,S0BFF80O0BFFE0,S05FFC0O05FFE0,U020R0280,,::::::::::::::";

            var sb = new StringBuilder();

            #region CE
            sb.Append("^XA");
            sb.Append(CE);
            sb.Append("^XA");
            sb.Append("^FT672,160^XG000.GRF,1,1^FS");
            sb.Append("^FT640,128^XG001.GRF,1,1^FS");
            #endregion

            #region Naglowek
            sb.Append($"^FT170,42^A0N,19,19^FB458,1,0,C^FH\\^FD,GTEX\" Sp.z o.o., ul.Jaskolek 12L, 43-215 Studzienice^FS");
            sb.Append($"^FT170,65^A0N,19,19^FB458,1,0,C^FH\\^FDDWU - {parametry.NrDWU}^FS");
            sb.Append($"^FO54,70^GB575,0,5^FS");
            sb.Append($"^FT59,94^A0N,12,12^FB564,1,0,C^FH\\^FDEN 13249:2016, EN 13250:2016, EN 13251:2016, EN 13252:2016, EN 13253:2016, EN 13254:2016, ^FS");
            sb.Append($"^FT59,108^A0N,12,12^FB564,1,0,C^FH\\^FDEN 13255:2016, EN 13257:2016, EN 13265:2016^FS");
            sb.Append($"^FT59,122^A0N,12,12^FB564,1,0,C^FH\\^FDJednostka notyfikowana: 1488 Instytut Techniki Budowlanej, Zaklad Certyfikacji, ul. Filtrowa 1, 00-611 Warszawa^FS");
            sb.Append($"^FT256,153^A0N,17,16^FH\\^FDGeowloknina (GTX-NW) ALTEX AT {towar.tblTowarGeowlokninaParametrySurowiec.Skrot} {towar.tblTowarGeowlokninaParametryGramatura.Gramatura}^FS");
            #endregion

            #region Opis
            sb.Append($"^FT8,173^A0N,12,12^FB775,1,0,C^FH\\^FDZastosowanie: do budowy drog i innych powierzchni obciazonych ruchem (z wylaczeniem nawierzchni asfaltowych), do budowy drog kolejowych, w robotach ^FS");
            sb.Append($"^FT8,187^A0N,12,12^FB775,1,0,C^FH\\^FDziemnych, fundamentowaniu i konstrukcjach oporowych, w systemach drenazowych, w zabezpieczeniach przeciwerozyjnych (ochrona i umocnienia brzegow), ^FS");
            sb.Append($"^FT8,201^A0N,12,12^FB775,1,0,C^FH\\^FDdo budowy zbiornikow wodnych i zapor, do budowy kanalow, do budowy skladowisk odpadow stalych, do budowy zbiornikow odpadow cieklych.^FS");
            sb.Append($"^FT8,215^A0N,12,12^FB775,1,0,C^FH\\^FDFunkcja: F+S^FS");
            #endregion

            var waga = (towar.Szerokosc_m * towar.Dlugosc_m * towar.tblTowarGeowlokninaParametryGramatura.Gramatura) / 1000;
            sb.Append($"^FT79,238^A0N,11,12^FH\\^FDRoll dimension|Rozmiar rolki: Width|Szerokosc, m: {towar.Szerokosc_m:N2} \\F1 5%; Length|Dlugosc, m: {towar.Dlugosc_m:N2} \\F1  5%; Weight|Waga, kg: {waga:N2} \\F1 10%^FS");


            #region Parametry

            //TODO!!! dostosowac parametry do wlokniny PES!! brak wodoprzepuszczalnosci pod obciazeniem!, przegladnac generalnie DWU!

            int horizStartPosLeft = 20;
            int vertStartPosition = 260;//50;
            int verticalStep = 13;
            int fontHeight = 11;
            int fontWidth = 12;


            if (parametry.WytrzymaloscNaRozciaganie_CMD != 0 && parametry.WytrzymaloscNaRozciaganie_MD != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition }^A0N,{fontHeight},{fontWidth}^FH\\^FDWytrzymalosc na rozciaganie (EN ISO 10319): MD 22,0 {parametry.WytrzymaloscNaRozciaganie_MD} ({parametry.WytrzymaloscNaRozciaganie_MD_Minimum - parametry.WytrzymaloscNaRozciaganie_MD}) kN/m, CMD {parametry.WytrzymaloscNaRozciaganie_CMD} ({parametry.WytrzymaloscNaRozciaganie_CMD_Minimum - parametry.WytrzymaloscNaRozciaganie_CMD}) kN/m^FS");
            if (parametry.WydluzeniePrzyZerwaniu_CMD != 0 && parametry.WydluzeniePrzyZerwaniu_MD != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDWydluzenie przy maksymalnym obciazeniu (EN ISO 10319): MD {parametry.WydluzeniePrzyZerwaniu_MD} (\\F120,0) %, CMD {parametry.WydluzeniePrzyZerwaniu_CMD} (\\F120,0) %^FS");
            if (parametry.OdpornoscNaPrzebicieStatyczne_CBR != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDOdpornosc na przebicie statyczne [CBR] (EN ISO 12236): {parametry.OdpornoscNaPrzebicieStatyczne_CBR} ({GenerujTolerancje(parametry.OdpornoscNaPrzebicieStatyczne_CBR, parametry.OdpornoscNaPrzebicieStatyczne_CBR_Minimum)}) kN^FS");
            if (parametry.OdpornoscNaPrzebicieDynamiczne != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDOdpornosc na przebicie dynamiczne (EN ISO 13433): {parametry.OdpornoscNaPrzebicieDynamiczne} ({GenerujTolerancje(parametry.OdpornoscNaPrzebicieDynamiczne, parametry.OdpornoscNaPrzebicieDynamiczne_Maksimum)}) mm^FS");
            if (parametry.WodoprzepuszczalnoscWPlaszczyznie_20kPa != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDWodoprzepuszczalnosc w plaszczyznie, i=1,  20 kPa (EN ISO 12958): {parametry.WodoprzepuszczalnoscWPlaszczyznie_20kPa} ({GenerujTolerancje(parametry.WodoprzepuszczalnoscWPlaszczyznie_20kPa, parametry.WodoprzepuszczalnoscWPlaszczyznie_20kPa_Minimum)}) 10\\5E(-7)m\\5E(2)/s,^FS");
            if (parametry.WodoprzepuszczalnoscWPlaszczyznie_100kPa != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDWodoprzepuszczalnosc w plaszczyznie, i=1, 100 kPa (EN ISO 12958): {parametry.WodoprzepuszczalnoscWPlaszczyznie_100kPa} ({GenerujTolerancje(parametry.WodoprzepuszczalnoscWPlaszczyznie_100kPa, parametry.WodoprzepuszczalnoscWPlaszczyznie_100kPa_Minimum)}) 10\\5E(-7)m\\5E(2)/s,^FS");
            if (parametry.WodoprzepuszczalnoscWPlaszczyznie_200kPa != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDWodoprzepuszczalnosc w plaszczyznie, i=1, 200 kPa (EN ISO 12958): {parametry.WodoprzepuszczalnoscWPlaszczyznie_200kPa} ({GenerujTolerancje(parametry.WodoprzepuszczalnoscWPlaszczyznie_200kPa, parametry.WodoprzepuszczalnoscWPlaszczyznie_200kPa_Minimum)}) 10\\5E(-7)m\\5E(2)/s^FS");
            if (parametry.CharakterystycznaWielkoscPorow != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDCharakterystyczna wielkosc porow, O90 (EN ISO 12956): {parametry.CharakterystycznaWielkoscPorow} (\\F120,0) \\E6m^FS");
            if (parametry.WodoprzepuszczalnoscProsotpadla != 0)
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDWodoprzepuszczalnosc prostopadla (EN ISO 11058): {parametry.WodoprzepuszczalnoscProsotpadla} ({ GenerujTolerancje(parametry.WodoprzepuszczalnoscProsotpadla, parametry.WodoprzepuszczalnoscProsotpadla_Minimum)}) [l/(m\\5E2s)]^FS");
            if (!string.IsNullOrEmpty(parametry.OdpornoscNaWarunkiAtmosferyczne))
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDOdpornosc na warunki atmosferyczne (EN 12224): {parametry.OdpornoscNaWarunkiAtmosferyczne}^FS");
            if (parametry.OdpornoscNaUtlenianie != 0)
            {
                sb.Append($"^FT{horizStartPosLeft},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FDOdpornosc na utlenianie (EN ISO 13438): Przewidywana trwalosc co najmniej {parametry.OdpornoscNaUtlenianie} lat w gruntach naturalnych^FS");
                sb.Append($"^FT{horizStartPosLeft + 200},{vertStartPosition += verticalStep}^A0N,{fontHeight},{fontWidth}^FH\\^FD o 4<pH<9 i temperaturze <25\\F8C.^FS");
            }
            #endregion


            #region Kod kreskowy
            if (!string.IsNullOrEmpty(towar.KodKreskowy))
                sb.Append($"^BY2,2,146^FT757,438^BEB,,Y,N^FD{towar.KodKreskowy}^FS"); // KodKreskowy
            #endregion

            sb.Append($"^PQ1,0,1,Y"); // ilosc
            sb.Append($"");
            sb.Append($"^XZ");

            return sb.ToString();

        }

        private string GenerujTolerancje(decimal wartoscNominalna, decimal wartoscZTolerancja)
        {
            if (wartoscNominalna >= wartoscZTolerancja)
            {
                var tolerancja = wartoscZTolerancja - wartoscNominalna;
                return tolerancja.ToString("n2");
            }
            else
            {
                var tolerancja = wartoscZTolerancja - wartoscNominalna;
                return "+" + tolerancja.ToString("n2");
            }
        }

        public string GetLabelCE()
        {

            string CE = "~DG000.GRF,01024,016,,::::::::::::::::::::::gI01,H0C0C38700F3C78071E3C3837E,01C1C6CD81FBE7C0D9E3E6C33E,014144C981833660990064C20C,H043468D01032641890066860C,H0424387010367C189F0C38618,H04646CDBD03C6DF883186C410,H047EC58818306C198330C4C30,H0404CD981DB0660D8330CHC60,01F0478F00F30660F1E3E7887E,gH018,:,:::::R060E0,R0E1B0,R0A130,R021A0,R020E0,R021B0,R02310,R02330,R0F9E0,,:::::::::::::~DG001.GRF,01920,020,,::::::::::::T0150Q0H140,T0BF80P02BE0,R017FFC0O07FHF0,R03FHFC0N02FHFE0,Q01FIFC0M017FIF0,Q03BFHF80N0JFE0,P017FIFC0M07FJF0,P02FJFC0M0KFE0,P07FJFC0L01FKF0,P0LFC0L03FJFE0,O01FJF540K017FJF60,O03FHFE80N0JF80,O07FHFD0N01FHFD,O03FFE0O03FFB8,N01FHFC0O07FHF0,O0IF80O0IF80,N01FHFP01FHF,N03FFC0O01FFE,N07FFC0O07FFC,N03FF80O03FF8,N07FF0P07FF0,N0HFE0P03FE0,M01FFE0P07FF0,N0HFE0P0HFE0,M01FFC0O01FFC0,M01FF80P0HF80,M05FFC0O01FFC0,M03FF80P0HF80,M01FF0P01FFC0,M03FF0P01FFAJA0,M07FF0P01FLF0,M03FE0P03FLF0,M07FF0P01FLF0,M03FE0P03FLF0,M07FF0P07FLF0,M03FE0P03FLF0,M07FF0P01FLF0,M03FE0P01FLF0,M07FF0P01FLF0,M03FF0P01FF80,M01FF0P01FFC0,M01FF80P0HF80,M01FFC0O01FFC0,N0HF80P0HFC0,M01FFC0O01FFC0,N0HF80P0BFE0,N0IFQ07FF0,N0HFE0P07FF8,N07FF0P07FFC,N03FF80O03FF8,N07FFC0O01FHF,N03FFE0P0HFE,N01FHFP01FHF80,O0IF80O0IF80,N047FHFP07FHF4,O03FHF80N03FHF8,O01FIFO01FIF40,P0JFA80M0JFE8,O017FJFC0L07FJFE0,P03FJFC0L03FJFE0,P01FJFC0L01FJFE0,Q0KF80M0KFE0,Q07FIFC0M07FIFE0,R0JFC0N0JFE0,R07FHFC0N07FIF0,S0BFF80O0BFFE0,S05FFC0O05FFE0,U020R0280,,::::::::::::::";

            var sb = new StringBuilder();

            #region CE
            sb.Append("^XA");
            sb.Append(CE);
            sb.Append("^XA");
            sb.Append("^FT672,160^XG000.GRF,1,1^FS");
            sb.Append("^FT640,128^XG001.GRF,1,1^FS");
            #endregion

            #region Naglowek
            sb.Append($"^FT170,42^A0N,19,19^FB458,1,0,C^FH\\^FD,GTEX\" Sp.z o.o., ul.Jaskolek 12L, 43-215 Studzienice^FS");
            sb.Append($"^FT170,65^A0N,19,19^FB458,1,0,C^FH\\^FDDWU - ALTEX AT PP 300 - 15052020^FS");
            sb.Append($"^FO54,70^GB575,0,5^FS");
            sb.Append($"^FT59,94^A0N,12,12^FB564,1,0,C^FH\\^FDEN 13249:2016, EN 13250:2016, EN 13251:2016, EN 13252:2016, EN 13253:2016, EN 13254:2016, ^FS");
            sb.Append($"^FT59,108^A0N,12,12^FB564,1,0,C^FH\\^FDEN 13255:2016, EN 13257:2016, EN 13265:2016^FS");
            sb.Append($"^FT59,122^A0N,12,12^FB564,1,0,C^FH\\^FDJednostka notyfikowana: 1488 Instytut Techniki Budowlanej, Zaklad Certyfikacji, ul. Filtrowa 1, 00-611 Warszawa^FS");
            sb.Append($"^FT256,153^A0N,17,16^FH\\^FDGeowloknina (GTX-NW) ALTEX AT PP 300^FS");
            #endregion

            #region Opis
            sb.Append($"^FT8,173^A0N,12,12^FB775,1,0,C^FH\\^FDZastosowanie: do budowy drog i innych powierzchni obciazonych ruchem (z wylaczeniem nawierzchni asfaltowych), do budowy drog kolejowych, w robotach ^FS");
            sb.Append($"^FT8,187^A0N,12,12^FB775,1,0,C^FH\\^FDziemnych, fundamentowaniu i konstrukcjach oporowych, w systemach drenazowych, w zabezpieczeniach przeciwerozyjnych (ochrona i umocnienia brzegow), ^FS");
            sb.Append($"^FT8,201^A0N,12,12^FB775,1,0,C^FH\\^FDdo budowy zbiornikow wodnych i zapor, do budowy kanalow, do budowy skladowisk odpadow stalych, do budowy zbiornikow odpadow cieklych.^FS");
            sb.Append($"^FT8,215^A0N,12,12^FB775,1,0,C^FH\\^FDFunkcja: F+S^FS");
            #endregion

            sb.Append($"^FT79,238^A0N,11,12^FH\\^FDRoll dimension/Rozmiar rolki: Width|Szerokosc, m: 2,0 \\F1 5%; Length|Dlugosc, m: 50 \\F1  5%; Weight|Waga, kg: 30,0 \\F1 10%^FS");


            #region Parametry
            sb.Append($"^FT77,261^A0N,11,12^FH\\^FDWytrzymalosc na rozciaganie (EN ISO 10319): MD 22,0 (-3,3) kN/m, CMD 19,0 (-2,5) kN/m; ^FS");
            sb.Append($"^FT77,274^A0N,11,12^FH\\^FDWydluzenie przy maksymalnym obciazeniu (EN ISO 10319): MD 50,0 (\\F120,0) %, CMD 55,0 (\\F120,0) %;^FS");
            sb.Append($"^FT77,287^A0N,11,12^FH\\^FDOdpornosc na przebicie statyczne [CBR] (EN ISO 12236): 3,3 (-0,28) kN; ^FS");
            sb.Append($"^FT77,300^A0N,11,12^FH\\^FDOdpornosc na przebicie dynamiczne (EN ISO 13433): 14,0 (+4,0) mm; ^FS");
            sb.Append($"^FT77,313^A0N,11,12^FH\\^FDWodoprzepuszczalnosc w plaszczyznie, i=1 (EN ISO 12958): 20kPa|48,0 (-9,1) 10-7m\\5E/s, ^FS");
            sb.Append($"^FT77,326^A0N,11,12^FH\\^FDWodoprzepuszczalnosc w plaszczyznie, i=1 (EN ISO 12958): 100kPa|21,5 (-4,6) 10-7m\\5E/s, ^FS");
            sb.Append($"^FT77,339^A0N,11,12^FH\\^FDWodoprzepuszczalnosc w plaszczyznie, i=1 (EN ISO 12958): 200kPa |13,0 (-3,6) 10-7m\\5E/s;^FS");
            sb.Append($"^FT77,352^A0N,11,12^FH\\^FDCharakterystyczna wielkosc porow, O90 (EN ISO 12956): 85,0 (\\F120,0) \\E6m; ^FS");
            sb.Append($"^FT77,365^A0N,11,12^FH\\^FDWodoprzepuszczalnosc prostopadla (EN ISO 11058): 52,0 (-14,0) [l/m\\5E2s)];^FS");
            sb.Append($"^FT73,392^A0N,11,12^FB513,1,0,C^FH\\^FDOdpornosc na warunki atmosferyczne (EN 12224): Nalezy przykryc w ciagu jednego dnia od wbudowania;^FS");
            sb.Append($"^FT73,405^A0N,11,12^FB513,1,0,C^FH\\^FDOdpornosc na utlenianie (EN ISO 13438): Przewidywana trwalosc co najmniej 100 lat ^FS");
            sb.Append($"^FT73,418^A0N,11,12^FB513,1,0,C^FH\\^FDw gruntach naturalnych o 4<pH<9 i temperaturze <25\\F8C.^FS");
            #endregion


            #region Kod kreskowy
            sb.Append($"^BY2,2,146^FT757,438^BEB,,Y,N^FD1234567890128^FS"); // KodKreskowy
            #endregion

            sb.Append($"^PQ1,0,1,Y"); // ilosc
            sb.Append($"");
            sb.Append($"^XZ");

            return sb.ToString();
        }
    }
}
