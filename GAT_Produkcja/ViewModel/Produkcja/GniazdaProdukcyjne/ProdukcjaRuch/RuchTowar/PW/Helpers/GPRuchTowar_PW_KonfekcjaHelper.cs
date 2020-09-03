using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW.Helpers
{
    public class GPRuchTowar_PW_KonfekcjaHelper : IGPRuchTowar_PW_KonfekcjaHelper
    {
        private readonly IDialogService dialogService;
        private readonly IUnitOfWork unitOfWork;
        #region CTOR
        public GPRuchTowar_PW_KonfekcjaHelper(IDialogService dialogService, IUnitOfWork unitOfWork)
        {
            this.dialogService = dialogService;
            this.unitOfWork = unitOfWork;
        }

        public bool CzyIloscM2ZgodnaZeZleceniem(tblProdukcjaZlecenieTowar zlecenieTowar,
                                                tblProdukcjaRuchTowar rolkaPW,
                                                IEnumerable<tblProdukcjaRuchTowar> listOfVMEntities)
        {
            if (zlecenieTowar is null) return false;
            if (rolkaPW is null) return false;

            // pobiera liste PW dla towaruZeZlecenia
            //var listaPWzBazy = await unitOfWork.tblProdukcjaRuchTowar.WhereAsync(e => e.IDProdukcjaZlecenieTowar == zlecenieTowar.IDProdukcjaZlecenieTowar);

            var sumaM2PrzyjetychDlaDanegoTowaruZlecenia = listOfVMEntities?.Sum(s => s.Ilosc_m2) + rolkaPW.Ilosc_m2; // + listaPWzBazy?.Sum(s => s.Ilosc_m2);
            
            if (sumaM2PrzyjetychDlaDanegoTowaruZlecenia > zlecenieTowar.Ilosc_m2)
                return false;

            return true;
        }
        #endregion
        /// <summary>
        /// Metoda sprawdzajaca czy sumaryczna ilosc m2 rolek przyjetych na PW z danej rolki RW nie przewyzsza ilosci m2 tej rolki RW
        /// </summary>
        /// <param name="rolkaRW">rolka RW z ktorej realizowana jest konfekcja</param>
        /// <param name="rolkaPW">rokla PW przyjmowana</param>
        /// <param name="listaRolekPW">lista rolek PW otrzymanych jak dotad z rolki RW bez rolki <paramref name="rolkaPW"/></param>
        /// <returns></returns>
        public bool CzyIloscM2ZgodnaZRolkaRW(tblProdukcjaRuchTowar rolkaRW,
                                             tblProdukcjaRuchTowar rolkaPW,
                                             IEnumerable<tblProdukcjaRuchTowar> listaRolekPW)
        {
            if (rolkaRW is null) return true;
            if (rolkaPW is null) return true;
            if (listaRolekPW is null) throw new ArgumentException("Brak listy PW do sprawdzenia ilosci zgodnych z rolka RW");

            var listaRolekPWzDanejRolkiRW = listaRolekPW.Where(r => r.IDRolkaBazowa == rolkaRW.IDProdukcjaRuchTowar)
                                                        .Sum(s => s.Ilosc_m2);
            var przyjetaIloscM2zRolkiRW = listaRolekPWzDanejRolkiRW + rolkaPW.Ilosc_m2;

            if (przyjetaIloscM2zRolkiRW > rolkaRW.Ilosc_m2)
            {
                dialogService.ShowError_BtnOK("Suma [m2] rolek na PW jest większa niż ilość [m2] rolki na RW." +
                                              "\nRolka nie może zostać dodana.");
                return false;
            }
            return true;

        }
    }
}
