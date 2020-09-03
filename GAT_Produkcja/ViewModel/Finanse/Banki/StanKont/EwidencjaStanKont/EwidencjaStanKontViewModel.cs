using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.DodajStanKonta;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.EwidencjaStanKont
{
    public class EwidencjaStanKontViewModel : ListAddEditDeleteCommandGenericViewModelBase<tblFinanseStanKonta>
    {
        public override string Title => "Ewidencja stanów kont";

        public override IGenericRepository<tblFinanseStanKonta> Repository => UnitOfWork.tblFinanseStanKonta; 

        public override Func<tblFinanseStanKonta, int> GetElementSentId => (obj)=> obj.IDFinanseStanKonta;

        public override Action ShowAddEditWindow => ()=> ViewService.Show<DodajStanKontaViewModel>();

        public EwidencjaStanKontViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
        }



        protected override async Task LoadElements()
        {
            await base.LoadElements();
            ListOfVMEntities = new ObservableCollection<tblFinanseStanKonta> 
                                    (ListOfVMEntities.OrderByDescending(e => e.DataStanu));
        }
    }

}
