using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.dbMsAccess;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.dbMsAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.Strategy
{
    public class SurowiecSubiektDictionaryMsAccessStrategy :  ISurowiecSubiektDictionaryMsAccessStrategy
    {
        private SurowceDictionary surowceDictionary;
        private IEnumerable<Surowiec> listaSurowcowMsAccess;
        private IEnumerable<vwMagazynRuchGTX> listaSurowcowSubiekt;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkMsAccess unitOfWorkMsAccess;

        public SurowiecSubiektDictionaryMsAccessStrategy(IUnitOfWork unitOfWork, IUnitOfWorkMsAccess unitOfWorkMsAccess)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkMsAccess = unitOfWorkMsAccess;

            surowceDictionary = new SurowceDictionary();
        }
        public async Task<int> PobierzIdSurowcaZSubiektDla(string nazwaSurowcaMsAccess, decimal ilosc = 0)
        {
            await PobierzDaneWyjsciowe();

            var surowiec = PobierzSurowiecZSubiekta(nazwaSurowcaMsAccess);

            return surowiec.IdTowar;
        }

        public async Task<vwMagazynRuchGTX> PobierzSurowiecZSubiektDla(string nazwaSurowcaMsAccess, decimal ilosc = 0)
        {
            await PobierzDaneWyjsciowe();

            var surowiec = PobierzSurowiecZSubiekta(nazwaSurowcaMsAccess);
            
            return surowiec;
        }
        
        private async Task PobierzDaneWyjsciowe()
        {
            await PobierzListeSurowcowZMsAccess();
            await PobierzListeSurowcowZSubiekt();
        }

        private async Task PobierzListeSurowcowZMsAccess()
        {
            if (listaSurowcowMsAccess is null
                || !listaSurowcowMsAccess.Any())
                listaSurowcowMsAccess = await unitOfWorkMsAccess.Surowiec.GetAllAsync();
        }

        private async Task PobierzListeSurowcowZSubiekt()
        {
            if (listaSurowcowSubiekt is null
                || !listaSurowcowSubiekt.Any())
                listaSurowcowSubiekt = await unitOfWork.vwMagazynRuchGTX.GetAllAsync();

        }

        private vwMagazynRuchGTX PobierzSurowiecZSubiekta(string nazwa)
        {

            int idSurowiecMsAccess = PobierzIdSurowcaZNazwyMsAccess(nazwa);
            // pobiera ID subiekta ze slownika
            if (!CzySurowiecWSlownikuMsAccess(idSurowiecMsAccess))
                throw new ArgumentException("Brak surowca w słowniku");

            int idPozycjiWSubiekt = surowceDictionary.PobierzIdSurowacaZSubiekt(idSurowiecMsAccess);

            // pobiera surowiec z Subiekta
            var listaSurowcowZSubiekt = listaSurowcowSubiekt.Where(s => s.IdTowar == idPozycjiWSubiekt);

            if (listaSurowcowZSubiekt is null)
                throw new ArgumentNullException("Nie znaleziono surowca do rozliczenia w bazie danych.");

            var surowiecZSubiekt = listaSurowcowZSubiekt.First();

            return surowiecZSubiekt;
        }

        public int PobierzIdSurowcaZNazwyMsAccess(string nazwaSurowca)
        {
            //var surowiec = await unitOfWorkMsAccess.Surowiec.GetFromName(nazwaSurowca);
            var surowiec = listaSurowcowMsAccess.SingleOrDefault(s => s.NazwaSurowca == nazwaSurowca);
            if (surowiec != null)
                return surowiec.Id;

            return 0;
        }

        public bool CzySurowiecWSlownikuMsAccess(int idSurowiecMsAccess)
        {
            try
            {
                var surowiec = surowceDictionary.PobierzIdSurowacaZSubiekt(idSurowiecMsAccess);

                if (surowiec == 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
