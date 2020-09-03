using GAT_Produkcja.db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public interface IGPRuchTowar_PW_RolkaBazowaHelper
    {
        /// <summary>
        /// Pobiera dane ze zlecenia dla <see cref="BazowaRolkaPW"/>, ktora nastepnie wysylana jest do formularza dodawania
        /// </summary>
        tblProdukcjaRuchTowar PobierzDaneZeZlecenia(tblProdukcjaRuchTowar bazowaRolkaPW, tblProdukcjaZlecenieTowar zlecenieTowar);

        /// <summary>
        /// Pobiera dane z Gniazda Produkcyjnego dla <see cref="BazowaRolkaPW"/>, ktora nastepnie wysylana jest do formularza dodawania
        /// </summary>
        Task<tblProdukcjaRuchTowar> PobierzNoweNryDlaRolki(tblProdukcjaRuchTowar bazowarRolkaPW, tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne, IEnumerable<tblProdukcjaRuchTowar> listaPW);

        /// <summary>
        /// Pobiera dane z RolkiRW dla <see cref="BazowaRolkaPW"/>, ktora nastepnie wysylana jest do formularza dodawania
        /// </summary>
        tblProdukcjaRuchTowar PobierzDaneZRolkiRw(tblProdukcjaRuchTowar bazowaRolkaPW, tblProdukcjaRuchTowar rolkaRW, tblProdukcjaGniazdoProdukcyjne gniazdoProdukcyjne);
    }
}