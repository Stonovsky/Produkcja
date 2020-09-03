using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.RW
{
    public class RozliczenieMsAccessRWViewModel : SaveDeleteMethodViewModelBase, IRozliczenieMsAccessRWViewModel
    {
        public RozliczenieMsAccessRWViewModel(IViewModelService viewModelService) 
            : base(viewModelService)
        {
        }

        public override bool IsChanged => throw new NotImplementedException();

        public override bool IsValid => throw new NotImplementedException();

        public override Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override void IsChanged_False()
        {
            throw new NotImplementedException();
        }

        public override Task LoadAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public override Task SaveAsync(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
