using GAT_Produkcja.db;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public interface IGPRuchTowar_PW_KonfekcjaHelper
    {
        /// <summary>
        /// Metoda sprawdzajaca czy sumaryczna ilosc m2 rolek przyjetych na PW z danej rolki RW nie przewyzsza ilosci m2 tej rolki RW
        /// </summary>
        /// <param name="rolkaRW">rolka RW z ktorej realizowana jest konfekcja</param>
        /// <param name="rolkaPW">rokla PW przyjmowana</param>
        /// <param name="listaRolekPW">lista rolek PW otrzymanych jak dotad z rolki RW bez rolki <paramref name="rolkaPW"/></param>
        /// <returns></returns>
        bool CzyIloscM2ZgodnaZRolkaRW(tblProdukcjaRuchTowar rolkaRW,
                                      tblProdukcjaRuchTowar rolkaPW,
                                      IEnumerable<tblProdukcjaRuchTowar> listaRolekPW);
        
        /// <summary>
        /// Metoda sprawdza, czy przyjete rolki PW + aktualnie przyjmowana rolkaPW jest mniejsza lub rowna towarowi ze zlecenia -> <see cref="tblProdukcjaZlecenieTowar"/>
        /// </summary>
        /// <param name="zlecenieTowar"></param>
        /// <param name="ruchTowar"></param>
        /// <param name="listOfVMEntities"></param>
        /// <returns></returns>
        bool CzyIloscM2ZgodnaZeZleceniem(tblProdukcjaZlecenieTowar zlecenieTowar,
                                         tblProdukcjaRuchTowar ruchTowar,
                                         IEnumerable<tblProdukcjaRuchTowar> listOfVMEntities);
    }
}