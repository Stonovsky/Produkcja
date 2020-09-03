using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.Utilities.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public class DirectoryHelper : IDirectoryHelper
    {
        private const string SCIEZKA_BAZOWA = @"\\192.168.34.57\gtex\10. PRODUKCJA\Rozliczenia ksiegowe\0_Magazyn\!_Program\";
        private IDirectoryInfoWrapper directoryInfo;
        private NazwaTowaruSubiektHelper nazwaTowaruHelper;
        private List<string> listaNazwSurowiecGramatura;

        public DirectoryHelper(IDirectoryInfoWrapper directoryInfo)
        {
            nazwaTowaruHelper = new NazwaTowaruSubiektHelper();
            this.directoryInfo = directoryInfo;
        }

        /// <summary>
        /// Generuje sciezke tworzac jednoczesnie nowy katalog
        /// </summary>
        /// <param name="listaRozliczenia"></param>
        /// <returns></returns>
        public string GenerujSciezke(IEnumerable<IProdukcjaRozliczenie> listaRozliczenia)
        {
            return StworzFolder(listaRozliczenia);
        }
        /// <summary>
        /// Tworzy Folder zwracajac jego sciezke
        /// </summary>
        /// <param name="listaRozliczenia">lista RW badz PW z ktorego brany jest nr zlecenia</param>
        /// <returns></returns>
        private string StworzFolder(IEnumerable<IProdukcjaRozliczenie> listaRozliczenia)
        {
            string sciezkaPelna = $"{SCIEZKA_BAZOWA}{DateTime.Now.Date.ToString("yyyy-MM-dd")} - ZP {listaRozliczenia.First().NrZlecenia}\\";

            if (directoryInfo.DirectoryExists(sciezkaPelna))
                sciezkaPelna=$"{sciezkaPelna}_1";

            var directory = directoryInfo.CreateDirectory(sciezkaPelna);
            return sciezkaPelna;
        }
    }
}
