using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Geokomorka.Przerob.DodajPrzerob;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Geokomorka.Przerob.Ewidencja
{
    public class EwidencjaPrzerobProdukcjaGeokomorkaViewModel 
        : ListAddEditDeleteCommandGenericViewModelBase<tblProdukcjaGeokomorkaPodsumowaniePrzerob>
    {
        public EwidencjaPrzerobProdukcjaGeokomorkaViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
        }

        public override string Title => "Ewidencja przerobu geokomórek";

        public override IGenericRepository<tblProdukcjaGeokomorkaPodsumowaniePrzerob> Repository => UnitOfWork.tblProdukcjaGeokomorkaPodsumowaniePrzerob;



        public override Func<tblProdukcjaGeokomorkaPodsumowaniePrzerob, int> GetElementSentId => (obj) => obj.IdProdukcjaGeokomorkaPodsumowaniePrzerob;

        public override Action ShowAddEditWindow => () => ViewService.Show<DodajPrzerobProdukcjaGeokomorkaViewModel>();

        protected override async Task LoadElements()
        {
            await base.LoadElements();
            ListOfVMEntities = new ObservableCollection<tblProdukcjaGeokomorkaPodsumowaniePrzerob>
                                            (ListOfVMEntities.OrderByDescending(e => e.DataDo).ToList());

        }
    }
}
