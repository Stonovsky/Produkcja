using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.EppFile
{
    public class EppFileGenerator : IEppFileGenerator
    {
        private StringBuilder stringBuilder;
        private decimal kosztDokumentu;

        public async Task GenerujPlikEPP(StatusRuchuTowarowEnum statusRuchu,
                                      tblProdukcjaRozliczenie_Naglowek naglowek,
                                      IEnumerable<IProdukcjaRozliczenie> listaPozycji,
                                      string sciezkaPliku,
                                      string uwagiDokumentu = "")
        {
            using (StreamWriter streamWriter = new StreamWriter(sciezkaPliku,false, Encoding.GetEncoding(1250)))
            {
                var textEpp = GenerujZawartoscPliku(statusRuchu, naglowek, listaPozycji, uwagiDokumentu);

                await streamWriter.WriteLineAsync(textEpp);

                //naglowek.RemoveChildObjects();
            }
        }
        private string PobierzStatus(StatusRuchuTowarowEnum statusRuchu)
        {
            if (statusRuchu == StatusRuchuTowarowEnum.PrzyjecieWewnetrzne_PW)
                return "PW";
            else
                return "RW";

        }

        public string GenerujZawartoscPliku(StatusRuchuTowarowEnum statusRuchu,
                                            tblProdukcjaRozliczenie_Naglowek naglowek,
                                            IEnumerable<IProdukcjaRozliczenie> lista,
                                            string uwagiDokumentu = "")
        {
            var wartoscDokumentu = GenerujWartoscDokumentu(statusRuchu, lista);

            var uzytkownik = UzytkownikZalogowany.Uzytkownik ?? new tblPracownikGAT { ID_PracownikGAT = 7, Imie = "Tomasz", Nazwisko = "Strączek", ImieINazwiskoGAT = "Tomasz Strączek" };

            UzupelnijNaglowek(naglowek);

            stringBuilder = new StringBuilder();

            #region [INFO]
            stringBuilder.Append("[INFO]");
            stringBuilder.AppendLine();
            //Informacje ogólne
            stringBuilder.Append("\"1.05\",3,1250,\"GTEX_Produkcja\",\"GTEXProdukcja\",\"AG_GEOSYNTHETICS_SPK\",");
            //Nadawca
            stringBuilder.Append("\"AG GEOSYNTHETICS Spólka z ograniczona odpowiedzialnocia Sp.k.\",\"Studzienice\",\"43 - 215\",\"Jaskolek 12L\",\"6381836269\",");
            //Magazyn: Kod, Nazwa, Opis, Analityka magazynu, 1 , data w formacie (YYYYMMDDGGMMSS
            stringBuilder.Append($"\"GEO\",\"Geosyntetyki\",,,1,{naglowek.DataDodania.ToString("yyyyMMddHHmmss")},{naglowek.DataDodania.ToString("yyyyMMddHHmmss")},\"{uzytkownik.ImieINazwiskoGAT}\",{naglowek.DataDodania.ToString("yyyyMMddHHmmss")},\"Polska\",\"PL\",\"PL 6381836269\",1");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            #endregion

            #region [NAGLOWEK]
            stringBuilder.Append("[NAGLOWEK]");
            stringBuilder.AppendLine();
            // Info ogolne
            stringBuilder.Append($"\"{PobierzStatus(statusRuchu)}\",1,0,1,,,\"1/GEO/2020\",,,,,,,,,,,,\"Magazyn\",\"Dokument magazynowy\",\"Studzienice\",{naglowek.DataDodania.ToString("yyyyMMdd000000")},{naglowek.DataDodania.ToString("yyyyMMdd000000")},,2,1,");
            // Koszt dokumentu
            stringBuilder.Append($"\"Cena kartotekowa\",{wartoscDokumentu.Netto},{wartoscDokumentu.VAT},{wartoscDokumentu.Brutto},{wartoscDokumentu.Netto},,0.0000,,{naglowek.DataDodania.ToString("yyyyMMdd000000")},0.0000,{wartoscDokumentu.Brutto},0,0,0,0,");
            // Dane pracownika
            stringBuilder.Append($"\"{uzytkownik.Imie}; {uzytkownik.Nazwisko}\",\"{uzytkownik.ImieINazwiskoGAT}\",,0.0000,0.0000,");
            // Waluta 
            stringBuilder.Append($"\"PLN\",1.0000,");
            // Uwagi do dokumentu
            stringBuilder.Append($"\"{uwagiDokumentu}\",,,,0,0,0,,0.0000,,0.0000,,,0");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();


            stringBuilder.Append("[ZAWARTOSC]");
            stringBuilder.AppendLine();
            DodajZawartosc(statusRuchu, lista);
            stringBuilder.AppendLine();

            #endregion


            #region [TOWARY]
            stringBuilder.Append("[NAGLOWEK]");
            stringBuilder.AppendLine();
            stringBuilder.Append("\"TOWARY\"");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();


            stringBuilder.Append("[ZAWARTOSC]");
            stringBuilder.AppendLine();
            DodajTowar(statusRuchu, lista);
            stringBuilder.AppendLine();

            #endregion

            #region [CENNIK]
            stringBuilder.Append("[NAGLOWEK]");
            stringBuilder.AppendLine();
            stringBuilder.Append("\"CENNIK\"");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();


            stringBuilder.Append("[ZAWARTOSC]");
            stringBuilder.AppendLine();
            DodajCennik(statusRuchu, lista);
            stringBuilder.AppendLine();
            #endregion


            #region [GRUPYTOWAROW]
            stringBuilder.Append("[NAGLOWEK]");
            stringBuilder.AppendLine();
            stringBuilder.Append("\"GRUPYTOWAROW\"");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();


            stringBuilder.Append("[ZAWARTOSC]");
            stringBuilder.AppendLine();
            DodajGrupyTowarow(statusRuchu, lista);
            #endregion

            return stringBuilder.ToString();
        }


        #region Naglowek

        private WartoscDokumentuModel GenerujWartoscDokumentu(StatusRuchuTowarowEnum statusRuchu, IEnumerable<IProdukcjaRozliczenie> listaPozycji)
        {
            kosztDokumentu = listaPozycji.Sum(s => s.Wartosc);
            return new WartoscDokumentuModel
            {
                Netto = kosztDokumentu.ToString(new CultureInfo("en-US")),
                VAT = (kosztDokumentu * 0.23m).ToString(new CultureInfo("en-US")),
                Brutto = (kosztDokumentu * 1.23m).ToString(new CultureInfo("en-US")),
            };
        }

        private static void UzupelnijNaglowek(tblProdukcjaRozliczenie_Naglowek naglowek)
        {
            //if (naglowek.tblPracownikGAT is null)
            //    naglowek.tblPracownikGAT = new tblPracownikGAT { ID_PracownikGAT = 7, Imie = "Tomasz", Nazwisko = "Strączek", ImieINazwiskoGAT = "Tomasz Strączek" };

            naglowek.DataDodania = DateTime.Now;
        }

        #endregion

        #region Zawartosc

        private void DodajZawartosc(StatusRuchuTowarowEnum statusRuchu,
                                      IEnumerable<IProdukcjaRozliczenie> listaPozycji
                                      )
        {
            if (statusRuchu == StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)
                DodajZawartoscZRW(listaPozycji);
            else
                DodajZawartoscZPW(listaPozycji);
        }

        private void DodajZawartoscZRW(IEnumerable<IProdukcjaRozliczenie> listaPozycji)
        {
            int lp = 1;

            foreach (var pozycja in listaPozycji)
            {
                stringBuilder.Append($"{lp},1,\"{pozycja.SymbolTowaruSubiekt}\",1,0,0,0,0.0000,0.0000,\"{pozycja.Jm}\",{pozycja.Ilosc.ToString(new CultureInfo("en-US"))},{pozycja.Ilosc.ToString(new CultureInfo("en-US"))},0.0000,0.0000,0.0000,23.0000,0.0000,0.0000,0.0000,{pozycja.Wartosc.ToString(new CultureInfo("en-US"))},,");
                stringBuilder.AppendLine();
                lp++;

            }
        }

        private void DodajZawartoscZPW(IEnumerable<IProdukcjaRozliczenie> listaPW)
        {
            int lp = 1;
            foreach (var pozycja in listaPW)
            {
                stringBuilder.Append($"{lp},1,\"{pozycja.SymbolTowaruSubiekt}\",1,0,0,0,0.0000,0.0000,\"{pozycja.Jm}\",");
                //Ilosc
                stringBuilder.Append($"{pozycja.Ilosc.ToString(new CultureInfo("en-US"))},{pozycja.Ilosc.ToString(new CultureInfo("en-US"))},");

                //Cena
                var kosztPozycji = PobierzWartoscPozycjiModel(pozycja);
                //Ceny jednostkowe
                stringBuilder.Append($"{kosztPozycji.CenaJednNetto},{kosztPozycji.CenaJednNetto},{kosztPozycji.CenaJednBrutto},23.0000,");
                //wartosc pozycji
                stringBuilder.Append($"{kosztPozycji.WartoscPozycjiNetto},{kosztPozycji.WartoscPozycjiVAT},{kosztPozycji.WartoscPozycjiBrutto},{kosztPozycji.WartoscPozycjiNetto},,");

                stringBuilder.AppendLine();
                lp++;
            }
        }
        private WartoscPozycjiModel PobierzWartoscPozycjiModel(IProdukcjaRozliczenie pozycja)
        {
            return new WartoscPozycjiModel
            {
                CenaJednNetto = Decimal.Round(pozycja.CenaJednostkowa, 4).ToString(new CultureInfo("en-US")),
                CenaJednBrutto = Decimal.Round(pozycja.CenaJednostkowa * 1.23m, 4).ToString(new CultureInfo("en-US")),
                WartoscPozycjiNetto = Decimal.Round(pozycja.Wartosc, 4).ToString(new CultureInfo("en-US")),
                WartoscPozycjiVAT = Decimal.Round(pozycja.Wartosc * 0.23m, 4).ToString(new CultureInfo("en-US")),
                WartoscPozycjiBrutto = Decimal.Round((pozycja.Wartosc * 1.23m), 4).ToString(new CultureInfo("en-US")),
            };
        }
        #endregion

        #region Towary

        private void DodajTowar(StatusRuchuTowarowEnum statusRuchu,
                              IEnumerable<IProdukcjaRozliczenie> listaPozycji
                              )
        {

            foreach (var pozycja in listaPozycji)
            {
                stringBuilder.Append($"1,\"{pozycja.SymbolTowaruSubiekt}\",,,\"{pozycja.NazwaTowaruSubiekt}\",,\"{pozycja.NazwaTowaruSubiekt}\",,,\"{pozycja.Jm}\",\"23\",23.0000,\"23\",23.0000,0.0000,0.0000,,0,,,,0.0000,0,,,0,\"{pozycja.Jm}\",0.0000,0.0000,,0,,0,0,,,,,,,,");
                stringBuilder.AppendLine();
            }

        }
        #endregion

        #region Cennik
        private void DodajCennik(StatusRuchuTowarowEnum statusRuchu,
                      IEnumerable<IProdukcjaRozliczenie> listaPozycji
                      )
        {

            foreach (var pozycja in listaPozycji)
            {
                stringBuilder.Append($"\"{pozycja.SymbolTowaruSubiekt}\",\"Hurtowa\",{pozycja.CenaJednostkowa.ToString(new CultureInfo("en-US"))},0.0000,5.0000,0.0000,0.0000");
                stringBuilder.AppendLine();
            }

        }


        #endregion

        #region Grupa towarowa
        private void DodajGrupyTowarow(StatusRuchuTowarowEnum statusRuchu, IEnumerable<IProdukcjaRozliczenie> listaPozycji)
        {
            foreach (var pozycja in listaPozycji)
            {
                string grupaTowarow = GenerujGrupeTowarow(statusRuchu, pozycja);

                stringBuilder.Append($"\"{pozycja.SymbolTowaruSubiekt}\",\"{grupaTowarow}\",");
                stringBuilder.AppendLine();
            }

        }

        private string GenerujGrupeTowarow(StatusRuchuTowarowEnum statusRuchu, IProdukcjaRozliczenie pozycja)
        {
            if (statusRuchu == StatusRuchuTowarowEnum.RozchodWewnetrzny_RW)
                return "SUROWCE WLOKNINY";

            if (pozycja.SymbolTowaruSubiekt.ToLower().Contains("tasmy"))
                return "SUROWCE WLOKNINY";

            if (pozycja.SymbolTowaruSubiekt.ToUpper().Contains("PP"))
                return "WLOKNINY PP";
            if (pozycja.SymbolTowaruSubiekt.ToUpper().Contains("PES"))
                return "WLOKNINY PES";

            return string.Empty;
        }

        #endregion


    }
}
