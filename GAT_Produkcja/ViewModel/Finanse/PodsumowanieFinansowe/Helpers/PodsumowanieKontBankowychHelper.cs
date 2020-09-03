using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.CurrencyService.NBP;
using GAT_Produkcja.Utilities.CurrencyService.NBP.Model;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public class PodsumowanieKontBankowychHelper : IPodsumowanieKontaBankoweHelper
    {
        #region Fields

        private IUnitOfWork unitOfWork;
        private readonly INBPService nbpService;
        private IEnumerable<tblFinanseStanKonta> stanyKont;
        private IEnumerable<PodsFinans_StanyKontModel> stanyKontModel;
        private decimal podsumowanie;

        #endregion
        #region Propetries
        public decimal Podsumowanie
        {
            get
            {
                if (stanyKontModel is null) return 0;
                return stanyKontModel.Sum(s => s.StanWPrzeliczeniu);
            }
            set => podsumowanie = value;
        }

        public bool IsButtonActive { get; set; } = true;
        #endregion

        #region CTOR
        public PodsumowanieKontBankowychHelper(IUnitOfWork unitOfWork, INBPService nbpService)
        {
            this.unitOfWork = unitOfWork;
            this.nbpService = nbpService;
        }
        #endregion

        /// <summary>
        /// Pobiera zestawienie sumaryczne konta dla kazdej z firm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PodsFinans_StanyKontModel>> PobierzStanKontZDaty(DateTime data)
        {
            IsButtonActive = false;

            IEnumerable<tblFinanseStanKonta> stanyKontZTygodnia = await PobierzStanyKontZTygodnia(data);

            if (stanyKontZTygodnia.Count() == 0)
                return null;
            //return new List<PodsFinans_StanyKontModel> { new PodsFinans_StanyKontModel()};

            var dataOstatniegoDodaniaKont = stanyKontZTygodnia.First().DataStanu;


            stanyKont = stanyKontZTygodnia.Where(e => e.DataStanu == dataOstatniegoDodaniaKont);

            stanyKontModel = stanyKont.Select(s => new PodsFinans_StanyKontModel
            {
                Firma = s.Firma,
                BankNazwa = s.BankNazwa,
                NrKonta = s.NrKonta,
                StanKonta = s.Stan,
                Waluta = s.Waluta,
                DataStanuKonta = s.DataStanu
            });
            IsButtonActive = true;

            return await PrzeliczStanyKontWgWaluty(stanyKontModel, data);
        }

        /// <summary>
        /// Pobiera stany kont z ostatniego tygodnia
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<IEnumerable<tblFinanseStanKonta>> PobierzStanyKontZTygodnia(DateTime data)
        {
            DateTime dataPoczatkowa = data.AddDays(-7);
            var stanyKontZTygodnia = await unitOfWork.tblFinanseStanKonta.WhereAsync(e => e.DataStanu <= data
                                                                                       && e.DataStanu >= dataPoczatkowa);
            stanyKontZTygodnia = stanyKontZTygodnia.OrderByDescending(e => e.DataStanu).ToList();
            return stanyKontZTygodnia;
        }

        /// <summary>
        /// Przelicza stany kont na zł zgodnie z waluta konta
        /// </summary>
        /// <param name="stanyKontModel"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<IEnumerable<PodsFinans_StanyKontModel>> PrzeliczStanyKontWgWaluty(IEnumerable<PodsFinans_StanyKontModel> stanyKontModel, DateTime data)
        {
            var stanyKont = new List<PodsFinans_StanyKontModel>(stanyKontModel);

            foreach (var stan in stanyKont)
            {
                stan.Kurs = await nbpService.GetActualCurrencyRate(stan.Waluta);
                stan.StanWPrzeliczeniu = stan.StanKonta * stan.Kurs;
            }
            this.stanyKontModel = stanyKont;
            return stanyKont;
        }
    }
}
