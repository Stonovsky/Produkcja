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

    public abstract class ListAddEditDeleteCommandViewModelBase : ViewModelServiceBase
    {
        #region Commands
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        #endregion

        #region CTOR
        public ListAddEditDeleteCommandViewModelBase(IViewModelService viewModelService) : base(viewModelService)
        {
            LoadCommand = new RelayCommand(LoadCommandExecute);
            AddCommand = new RelayCommand(AddCommandExecute);
            EditCommand = new RelayCommand(EditCommandExecute, EditCommandCanExecute);
            DeleteCommand = new RelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);
        }

        #endregion

        #region AbstractMethods

        protected abstract void LoadCommandExecute();
        protected abstract void AddCommandExecute();
        protected abstract void EditCommandExecute();
        protected abstract bool EditCommandCanExecute();
        protected abstract void DeleteCommandExecute();
        protected abstract bool DeleteCommandCanExecute();
        #endregion


    }


}
