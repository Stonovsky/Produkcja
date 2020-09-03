using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.BaseClasses
{
    public abstract class SaveDeleteMethodViewModelBase : ViewModelServiceBase, IMethodDetailViewModel
    {
        public abstract bool IsChanged { get; }

        public abstract bool IsValid { get; }

        public SaveDeleteMethodViewModelBase(IViewModelService viewModelService) 
            : base(viewModelService)
        {
        }


        public abstract Task LoadAsync(int? id);


        public abstract Task SaveAsync(int? id);

        public abstract Task DeleteAsync(int id);

        public abstract void IsChanged_False();
    }
}
