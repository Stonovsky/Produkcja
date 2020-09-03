using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public interface ICommandDetailViewModel: IDetailViewModel
    {
        void LoadCommandExecute();
        void CloseWindowCommandExecute(CancelEventArgs args);
        void DeleteCommandExecute();
        bool DeleteCommandCanExecute();
        void SaveCommandExecute();
        bool SaveCommandCanExecute();
    }
}
