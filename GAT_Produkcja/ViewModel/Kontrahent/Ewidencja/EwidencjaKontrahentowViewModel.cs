using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.GenericRepository;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GAT_Produkcja.ViewModel.Kontrahent.Ewidencja
{
    public class EwidencjaKontrahentowViewModel
               : ListAddEditDeleteCommandGenericViewModelBase<tblKontrahent>
    {
        #region Properties

        public tblKontrahent KontrahentSzukaj { get; set; } = new tblKontrahent();
        public override IGenericRepository<tblKontrahent> Repository => UnitOfWork.tblKontrahent;
        public override string Title => "Ewidencja kontrahentów";
        
        #endregion

        #region Commands
        public RelayCommand SzukajCommand { get; set; }
        #endregion

        #region CTOR

        public EwidencjaKontrahentowViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {
            SzukajCommand = new RelayCommand(SzukajCommandExecute);
        }
        #endregion
        
        private async void SzukajCommandExecute()
        {
            ListOfVMEntities = new ObservableCollection<tblKontrahent>
                (await UnitOfWork.tblKontrahent.WyszukajKontrahenta(KontrahentSzukaj));
        }


        #region Delegates

        public override Func<tblKontrahent, int> GetElementSentId => (k) => k.ID_Kontrahent;
        public override Action ShowAddEditWindow => () => ViewService.Show<DodajKontrahentaViewModel>();

        #endregion
    }
}
