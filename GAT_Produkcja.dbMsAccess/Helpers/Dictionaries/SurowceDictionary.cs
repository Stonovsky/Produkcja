using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess
{
    public class SurowceDictionary
    {
        Dictionary<int, int> surowiecMsAccessComarchDictionary = new Dictionary<int, int>();
        Dictionary<int, int> surowiecMsAccessGtexProdukcjaDictionary = new Dictionary<int, int>();
        Dictionary<int, int> surowiecGtexProdukcjaMsAccessDictionary = new Dictionary<int, int>();
        Dictionary<int, int> surowiec_Subiekt_MsAccessDictionary = new Dictionary<int, int>();
        Dictionary<int, int> surowiec_MsAccess_SubiektDictionary = new Dictionary<int, int>();
        Dictionary<int, string> surowiecMsAccessDictionary = new Dictionary<int, string>();
        Dictionary<int, string> surowiecComarchDictionary = new Dictionary<int, string>();
        Dictionary<int, string> surowiecSubiektGtexDictionary = new Dictionary<int, string>();
        public SurowceDictionary()
        {
            StworzMsAccessDictionary();
            StworzSubiektGtexDictionary();

            StworzPolaczenia_Subiekt_MsAccess_Ditionary();
            StworzPolaczenia_MsAccess_Subiekt_Ditionary();

            //na potrzeby testow
            MsAccess_Comarch_Dictionary();

        }

        private void MsAccess_Comarch_Dictionary()
        {
            surowiecComarchDictionary.Add(292, "Włókno PP 6,7/76 W UV HT");
            surowiecComarchDictionary.Add(293, "Włókno PP 4,4/76 W UV HT");
            surowiecComarchDictionary.Add(343, "Włókno PES cięte 6 DEN/64 mm");
            surowiecComarchDictionary.Add(344, "Włókno PES cięte 4 DEN/51 mm");
            surowiecComarchDictionary.Add(405, "Włókno PES cięte 3 DEN/64 mm");

            surowiecMsAccessComarchDictionary.Add(1, 343);
            surowiecMsAccessComarchDictionary.Add(2, 344);
            surowiecMsAccessComarchDictionary.Add(3, 405);
            surowiecMsAccessComarchDictionary.Add(9, 292);
            surowiecMsAccessComarchDictionary.Add(10, 293);

            surowiecMsAccessGtexProdukcjaDictionary.Add(1, 943);
            surowiecMsAccessGtexProdukcjaDictionary.Add(2, 942);
            surowiecMsAccessGtexProdukcjaDictionary.Add(3, 941);
            surowiecMsAccessGtexProdukcjaDictionary.Add(9, 946);
            surowiecMsAccessGtexProdukcjaDictionary.Add(10, 945);

            surowiecGtexProdukcjaMsAccessDictionary.Add(943, 1);
            surowiecGtexProdukcjaMsAccessDictionary.Add(942, 2);
            surowiecGtexProdukcjaMsAccessDictionary.Add(941, 3);
            surowiecGtexProdukcjaMsAccessDictionary.Add(946, 9);
            surowiecGtexProdukcjaMsAccessDictionary.Add(945, 10);
        }

        private void StworzPolaczenia_MsAccess_Subiekt_Ditionary()
        {
            surowiec_MsAccess_SubiektDictionary.Add(1, 72);
            surowiec_MsAccess_SubiektDictionary.Add(2, 71);
            surowiec_MsAccess_SubiektDictionary.Add(3, 70);
            surowiec_MsAccess_SubiektDictionary.Add(9, 81);
            surowiec_MsAccess_SubiektDictionary.Add(10, 79);
            surowiec_MsAccess_SubiektDictionary.Add(11, 131);
            surowiec_MsAccess_SubiektDictionary.Add(13, 132);
            surowiec_MsAccess_SubiektDictionary.Add(15, 71);
        }

        private void StworzPolaczenia_Subiekt_MsAccess_Ditionary()
        {
            surowiec_Subiekt_MsAccessDictionary.Add(70, 3);
            //surowiec_Subiekt_MsAccessDictionary.Add(71, 15);
            surowiec_Subiekt_MsAccessDictionary.Add(71, 2);
            surowiec_Subiekt_MsAccessDictionary.Add(72, 1);
            surowiec_Subiekt_MsAccessDictionary.Add(79, 9);
            surowiec_Subiekt_MsAccessDictionary.Add(81, 10);
            surowiec_Subiekt_MsAccessDictionary.Add(131, 11);
            surowiec_Subiekt_MsAccessDictionary.Add(132, 13);
            surowiec_Subiekt_MsAccessDictionary.Add(179, 10);
            surowiec_Subiekt_MsAccessDictionary.Add(180, 9);
        }

        private void StworzSubiektGtexDictionary()
        {
            surowiecSubiektGtexDictionary.Add(70, "PES_3/64");
            surowiecSubiektGtexDictionary.Add(71, "PES_4/51");
            surowiecSubiektGtexDictionary.Add(72, "PES_6/64");
            surowiecSubiektGtexDictionary.Add(79, "PP_4,4/76UV");
            surowiecSubiektGtexDictionary.Add(81, "PP_6,7/76UV");
            surowiecSubiektGtexDictionary.Add(128, "TAŚMY_PES");
            surowiecSubiektGtexDictionary.Add(129, "TAŚMY_PP");
            surowiecSubiektGtexDictionary.Add(131, "SUROWIEC_PES_TAŚMY");
            surowiecSubiektGtexDictionary.Add(132, "SUROWIEC_PP_TAŚMY");
            surowiecSubiektGtexDictionary.Add(179, "PP_4,4/75UV");
            surowiecSubiektGtexDictionary.Add(180, "PP_6,7/75UV");
        }

        private void StworzMsAccessDictionary()
        {
            surowiecMsAccessDictionary.Add(1, "PES 6/64 biały");
            surowiecMsAccessDictionary.Add(2, "BICO 4/54 biały");
            surowiecMsAccessDictionary.Add(3, "PES 3/64 biały");
            surowiecMsAccessDictionary.Add(9, "PP 6,7/75/UV");
            surowiecMsAccessDictionary.Add(10, "PP 4,4/75/UV");
            surowiecMsAccessDictionary.Add(11, "Szarpanka PES");
            surowiecMsAccessDictionary.Add(13, "Szarpanka PP");
            surowiecMsAccessDictionary.Add(14, "Przejściówka");
            surowiecMsAccessDictionary.Add(15, "BICO 4/51 biały");
        }

        public int PobierzIdSurowcaComarch(int idSurowcaMsAccess)
        {
            try
            {
                return surowiecMsAccessComarchDictionary[idSurowcaMsAccess];
            }
            catch (Exception ex )
            {
                return 0;
            }

        }

        public int PobierzIdSurowacaZSubiekt(int idSurowiecMsAccess)
        {
            try
            {
                return surowiec_MsAccess_SubiektDictionary[idSurowiecMsAccess];
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int PobierzIdSurowacaZMsAccess(int idSurowcaWSubiekcie)
        {
            try
            {
                return surowiec_Subiekt_MsAccessDictionary[idSurowcaWSubiekcie];
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int PobierzIdSurowcaAccess(int idSurowcaGtexProdukcja)
        {
            try
            {
                return surowiecGtexProdukcjaMsAccessDictionary[idSurowcaGtexProdukcja];
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
