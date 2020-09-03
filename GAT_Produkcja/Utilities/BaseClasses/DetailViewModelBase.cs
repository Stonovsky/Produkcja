using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public abstract class DetailViewModel : ViewModelBase, IMethodDetailViewModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;

        #region Properties
        public abstract bool IsValid { get; set; }
        public abstract bool IsChanged { get; set; }


        #endregion

        #region Commands
        #endregion

        #region CTOR
        public DetailViewModel(IUnitOfWork unitOfWork,
                                              IViewService viewService,
                                              IDialogService dialogService,
                                              IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.messenger = messenger;

        }

        public abstract Task LoadAsync(int? id);
        protected abstract void LoadAsync();
        public abstract Task SaveAsync(int? id);
        public abstract Task DeleteAsync(int id);
        public abstract void IsChanged_False();

        #endregion


    }
}
