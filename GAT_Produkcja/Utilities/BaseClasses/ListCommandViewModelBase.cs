using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    [AddINotifyPropertyChangedInterface]

    public abstract class ListCommandViewModelBase : ViewModelServiceBase
    {
        #region Commands
        public RelayCommand LoadCommand { get; set; }

        #endregion

        #region CTOR
        public ListCommandViewModelBase(IViewModelService viewModelService) : base(viewModelService)
        {
            LoadCommand = new RelayCommand(LoadCommandExecute);
        }
        #endregion



        protected abstract void LoadCommandExecute();



    }
}
