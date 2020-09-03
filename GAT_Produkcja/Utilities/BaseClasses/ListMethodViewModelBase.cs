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

    public abstract class ListMethodViewModelBase : ViewModelServiceBase
    {
        #region CTOR
        public ListMethodViewModelBase(IViewModelService viewModelService) : base(viewModelService)
        {
        }
        #endregion

        public abstract Task LoadAsync(int? id);
    }
}
