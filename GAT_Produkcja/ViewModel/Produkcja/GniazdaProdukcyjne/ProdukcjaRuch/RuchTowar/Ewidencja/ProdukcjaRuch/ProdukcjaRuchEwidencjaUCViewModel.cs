using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja
{
    public class ProdukcjaRuchEwidencjaUCViewModel
        : ListAddEditDeleteMethodGenericViewModelBase<tblProdukcjaRuchTowar>, IProdukcjaRuchEwidencjaUCViewModel
    {



        #region Commands
        public RelayCommand UsunNaglowekIRuchPowiazanyCommand { get; set; }
        #endregion

        #region CTOR

        public ProdukcjaRuchEwidencjaUCViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {
            UsunNaglowekIRuchPowiazanyCommand = new RelayCommand(UsunNaglowekIRuchPowiazanyCommandExecute, UsunNaglowekIRuchPowiazanyCommandCanExecute);

        }
        #endregion

        #region UsunNaglowekIRuchPowiazany

        #region Execute
        private async void UsunNaglowekIRuchPowiazanyCommandExecute()
        {
            tblProdukcjaRuchNaglowek naglowek = await PobierzNaglowek(SelectedVMEntity.IDProdukcjaRuchNaglowek);
            if (naglowek is null) return;
            UnitOfWork.tblProdukcjaRuchNaglowek.Remove(naglowek);


            IEnumerable<tblProdukcjaRuchTowar> towary = await PobierzTowarDlaNaglowka(SelectedVMEntity.IDProdukcjaRuchNaglowek);
            if (towary != null && towary.Any())
                UnitOfWork.tblProdukcjaRuchTowar.RemoveRange(towary);

            await UsunBadaniaAsync(towary);

            await UnitOfWork.SaveAsync();
        }

        private async Task UsunBadaniaAsync(IEnumerable<tblProdukcjaRuchTowar> towary)
        {
            IEnumerable<tblProdukcjaRuchTowarBadania> badania = await PobierzBadaniaDla(towary);
            if (badania != null && badania.Any())
                UnitOfWork.tblProdukcjaRuchTowarBadania.RemoveRange(badania);
        }

        private async Task<IEnumerable<tblProdukcjaRuchTowarBadania>> PobierzBadaniaDla(IEnumerable<tblProdukcjaRuchTowar> towary)
        {
            if (towary is null) return null;

            return await UnitOfWork.tblProdukcjaRuchTowarBadania
                            .WhereAsync(e => towary.Any(t => t.IDProdukcjaRuchTowar == e.IDProdukcjaRuchTowar));
        }

        private async Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzTowarDlaNaglowka(int? iDProdukcjaRuchNaglowek)
        {
            if (iDProdukcjaRuchNaglowek is null) return null;

            return await UnitOfWork.tblProdukcjaRuchTowar.WhereAsync(e => e.IDProdukcjaRuchNaglowek == iDProdukcjaRuchNaglowek);
        }

        private async Task<tblProdukcjaRuchNaglowek> PobierzNaglowek(int? iDProdukcjaRuchNaglowek)
        {
            if (iDProdukcjaRuchNaglowek is null) return null;

            var naglowek = await UnitOfWork.tblProdukcjaRuchNaglowek.GetByIdAsync(iDProdukcjaRuchNaglowek.Value);

            return naglowek;
        }

        #endregion

        #region CanExecute
        private bool UsunNaglowekIRuchPowiazanyCommandCanExecute()
        {
            return SelectedVMEntity != null
                && SelectedVMEntity.IDProdukcjaRuchNaglowek != 0;
        }

        #endregion

        #endregion

        public override string Title => throw new NotImplementedException();

        public override IGenericRepository<tblProdukcjaRuchTowar> Repository => UnitOfWork.tblProdukcjaRuchTowar;

        public override Func<tblProdukcjaRuchTowar, int> GetElementId => (e) => e.IDProdukcjaRuchTowar;

        #region AddEdit

        public override Action ShowAddEditWindow => () => ViewService.Show<GPRuchNaglowekViewModel>();
        protected override void EditCommandMessage()
        {
            Messenger.Send(SelectedVMEntity.tblProdukcjaRuchNaglowek);
        }
        #endregion

        protected override Action<int?> UpdateEntityBeforeSaveAction => throw new NotImplementedException();

        public override async Task LoadAsync(int? id)
        {
            ListOfVMEntities = new ObservableCollection<tblProdukcjaRuchTowar>
                (await UnitOfWork.tblProdukcjaRuchTowar.GetAllAsync());
        }
    }
}
