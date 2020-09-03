using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe.Helpers
{
    public class PodsumowanieFinansoweHelper : IPodsumowanieFinansoweHelper
    {
        public IPodsumowanieZamowieniaOdKlientowHelper PodsumowanieZamowieniaOdKlientowHelper { get; }
        public IPodsumowanieProdukcjaHelper PodsumowanieProdukcjaHelper { get; }
        public IPodsumowanieSprzedazHelper PodsumowanieSprzedazHelper { get; }
        public IPodsumowanieMagazynyHelper PodsumowanieMagazynyHelper { get; }

        public IPodsumowanieNaleznosciIZobowiazaniaHelper PodsumowanieNaleznosciIZobowiazaniaHelper { get; }

        public IPodsumowanieKontaBankoweHelper PodsumowanieKontBankowychHelper { get; }

        public PodsumowanieFinansoweHelper(IPodsumowanieZamowieniaOdKlientowHelper podsumowanieZamowieniaOdKlientowHelper,
                                           IPodsumowanieProdukcjaHelper podsumowanieProdukcjaHelper,
                                           IPodsumowanieSprzedazHelper podsumowanieSprzedazHelper,
                                           IPodsumowanieMagazynyHelper podsumowanieMagazynyHelper,
                                           IPodsumowanieNaleznosciIZobowiazaniaHelper podsumowanieNaleznosciIZobowiazaniaHelper,
                                           IPodsumowanieKontaBankoweHelper podsumowanieKontaBankoweHelper)
        {
            PodsumowanieZamowieniaOdKlientowHelper = podsumowanieZamowieniaOdKlientowHelper;
            PodsumowanieProdukcjaHelper = podsumowanieProdukcjaHelper;
            PodsumowanieSprzedazHelper = podsumowanieSprzedazHelper;
            PodsumowanieMagazynyHelper = podsumowanieMagazynyHelper;
            PodsumowanieNaleznosciIZobowiazaniaHelper = podsumowanieNaleznosciIZobowiazaniaHelper;
            PodsumowanieKontBankowychHelper = podsumowanieKontaBankoweHelper;
        }

    }
}
