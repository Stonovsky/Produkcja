using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Tests.Utilities.BaseClasses
{
    public class AddEditCommandGenericViewModelBaseInstance : AddEditCommandGenericViewModelBase<tblTowar>
    {
        public AddEditCommandGenericViewModelBaseInstance(IViewModelService viewModelService) : base(viewModelService)
        {
        }

        public override IGenericRepository<tblTowar> Repository => UnitOfWork.tblTowar;

        public override Func<tblTowar, int> GetIdFromEntityWhenSentByMessenger => (tblTowar) =>0;

        protected override Func<int> GetVMEntityId => () => VMEntity.IDTowar;
    }
}
