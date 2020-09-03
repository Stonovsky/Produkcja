using GAT_Produkcja.db.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses.ListAddEditDeleteCommandStates
{
    public class SelectListSelectionState : IListSelectionState
    {
        private readonly ListViewModelStatesEnum listViewModelStatesEnum;

        public string SelectEditButtonTitle => "Wybierz";

        public SelectListSelectionState(ListViewModelStatesEnum listViewModelStatesEnum)
        {
            this.listViewModelStatesEnum = listViewModelStatesEnum;
        }

        public void PreformWithSelection()
        {
            //Messenger.Send(SelectedVMEntity);
            //ViewService.Close(this.GetType().Name);

        }


    }
}
