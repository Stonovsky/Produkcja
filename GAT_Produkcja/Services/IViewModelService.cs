using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.Logger;

namespace GAT_Produkcja.Services
{
    public interface IViewModelService
    {
        IDialogService DialogService { get; }
        IMessenger Messenger { get; }
        IUnitOfWork UnitOfWork { get; }
        IUnitOfWorkFactory UnitOfWorkFactory { get; }
        IViewService ViewService { get; }
        IActivityLogger ActivityLogger { get; }
    }
}