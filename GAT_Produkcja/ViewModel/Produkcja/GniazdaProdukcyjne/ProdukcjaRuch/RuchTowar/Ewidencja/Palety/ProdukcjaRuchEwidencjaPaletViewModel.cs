using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja.Palety.Adapters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja.Palety
{
    public class ProdukcjaRuchEwidencjaPaletViewModel : ListAddEditDeleteCommandGenericViewModelBase<tblProdukcjaRuchTowar>
    {
        public ProdukcjaRuchEwidencjaPaletViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
        }

        public override string Title => "Ewidencja palet";

        public override IGenericRepository<tblProdukcjaRuchTowar> Repository => UnitOfWork.tblProdukcjaRuchTowar;

        public override Func<tblProdukcjaRuchTowar, int> GetElementSentId => (e)=>e.IDProdukcjaRuchTowar;

        public override Action ShowAddEditWindow => ()=> { };

        protected override async Task LoadElements()
        {
            await base.LoadElements();

            ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar> (
                new ProdukcjaRuchPaletyAdapter(ListOfVMEntities).GroupBy(g => new { g.NrPalety})); //, g.TowarNazwaSubiekt
        }
    }
}
