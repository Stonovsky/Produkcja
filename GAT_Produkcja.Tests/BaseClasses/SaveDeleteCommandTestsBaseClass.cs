using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.BaseClasses
{
    public interface SaveDeleteCommandTestsBaseClass
    {
        #region LoadCommandExecute
        void LoadCommandExecute_PobieraElementyZBazy();

        #endregion

        #region SaveCommandExecute

        //void SaveCommandExecute_PoZapisieWyswietlDialog();
        void SaveCommandExecute_GdyIdZero_DodajDoBazy();
        void SaveCommandExecute_GdyIdWiekszeOdZera_NieDodawajDoBazy();
        void SaveCommandExecute_PoZapisieWyslijMessage();
        void SaveCommandExecute_PoZapisieZamknijOkno();

        #endregion


        #region DeleteExecute
        void DeleteCommandExecute_PrzedUsunieciemWyswietlDialogZPytaniem();
        void DeleteCommandExecute_Dialog_True_Usun();
        void DeleteCommandExecute_PoUsunieciuWyslijMessage();
        void DeleteCommandExecute_PoUsunieciuWyswietlInformacje();
        void DeleteCommandExecute_PoUsunieciuZamknijOkno(); 
        #endregion

    }
}
