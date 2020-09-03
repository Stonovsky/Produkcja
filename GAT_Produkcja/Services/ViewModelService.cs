using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Services
{
    public class ViewModelService : IViewModelService
    {
        public IUnitOfWork UnitOfWork { get; }
        public IUnitOfWorkFactory UnitOfWorkFactory { get; }
        public IViewService ViewService { get; }
        public IDialogService DialogService { get; }
        public IMessenger Messenger { get; }

        public IActivityLogger ActivityLogger { get; }

        public IMessenger MessengerOrg { get; }

        public ViewModelService(IUnitOfWork unitOfWork,
                                IUnitOfWorkFactory unitOfWorkFactory,
                                IViewService viewService,
                                IDialogService dialogService,
                                IMessenger messenger,
                                IActivityLogger activityLogger)
        {
            this.UnitOfWork = unitOfWork;
            this.UnitOfWorkFactory = unitOfWorkFactory;
            this.ViewService = viewService;
            this.DialogService = dialogService;
            this.Messenger = messenger;
            this.ActivityLogger = activityLogger;
        }

    }
}
