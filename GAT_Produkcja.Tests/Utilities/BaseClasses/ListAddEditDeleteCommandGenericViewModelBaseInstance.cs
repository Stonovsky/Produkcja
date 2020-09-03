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
    public class ListAddEditDeleteCommandGenericViewModelBaseInstance : ListAddEditDeleteCommandGenericViewModelBase<tblProdukcjaGeokomorkaPodsumowaniePrzerob>
    {
        public ListAddEditDeleteCommandGenericViewModelBaseInstance(IViewModelService viewModelService) : base(viewModelService)
        {
        }

        public override string Title => throw new NotImplementedException();

        public override IGenericRepository<tblProdukcjaGeokomorkaPodsumowaniePrzerob> Repository => UnitOfWork.tblProdukcjaGeokomorkaPodsumowaniePrzerob;


        public override Func<tblProdukcjaGeokomorkaPodsumowaniePrzerob, int> GetElementSentId => (obj) => obj.IdProdukcjaGeokomorkaPodsumowaniePrzerob;

        public override Action ShowAddEditWindow => ()=> {};
    }
}
