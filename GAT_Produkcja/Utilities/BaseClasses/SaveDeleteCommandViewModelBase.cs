using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public abstract class SaveCommandGenericViewModelBase : ViewModelServiceBase, IDetailViewModel
    {
        #region Commands
        public RelayCommand SaveCommand{ get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand<CancelEventArgs> CloseWindowCommand { get; set; }

        public abstract bool IsChanged { get;  }
        public abstract bool IsValid { get;  }
        #endregion

        #region CTOR
        public SaveCommandGenericViewModelBase(IViewModelService viewModelService) : base(viewModelService)
        {
            SaveCommand = new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);
            DeleteCommand = new RelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);
            LoadCommand = new RelayCommand(LoadCommandExecute);
            CloseWindowCommand = new RelayCommand<CancelEventArgs>(CloseWindowCommandExecute);
        }

        protected abstract void LoadCommandExecute();
        protected virtual void CloseWindowCommandExecute(CancelEventArgs args)
        {
            if (!IsChanged)
            {
                ViewService.Close(this.GetType().Name);
                return;
            }

            if (DialogService.ShowQuestion_BoolResult("Wprowadzone zmiany nie będą zapisane. Czy kontynuować?"))
            {
                ViewService.Close(this.GetType().Name);
            }
            else
            {
                if (args != null)
                    args.Cancel = true;
            }
        }

        protected abstract void DeleteCommandExecute();
        protected abstract bool DeleteCommandCanExecute();
        protected abstract void SaveCommandExecute();
        protected abstract bool SaveCommandCanExecute();

        public abstract void IsChanged_False();

        #endregion

    }
}
