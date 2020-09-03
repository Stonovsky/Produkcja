using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.Logger;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    [AddINotifyPropertyChangedInterface]
    [Serializable]

    public abstract class ViewModelServiceBase : ViewModelBase
    {
        public IUnitOfWork UnitOfWork { get; }
        public IUnitOfWorkFactory UnitOfWorkFactory { get; }
        public IViewService ViewService { get; }
        public IDialogService DialogService { get; }
        public IMessenger Messenger { get; }
        public IActivityLogger ActivityLogger { get; }

        public ViewModelServiceBase(IViewModelService viewModelService)
        {
            UnitOfWork = viewModelService.UnitOfWork;
            UnitOfWorkFactory = viewModelService.UnitOfWorkFactory;
            ViewService = viewModelService.ViewService;
            DialogService = viewModelService.DialogService;
            Messenger = viewModelService.Messenger;
            ActivityLogger = viewModelService.ActivityLogger;
        }
    }
}
