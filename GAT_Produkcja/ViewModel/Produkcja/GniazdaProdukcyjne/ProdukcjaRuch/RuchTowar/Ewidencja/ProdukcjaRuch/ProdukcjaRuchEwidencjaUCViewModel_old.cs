using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja
{
    [AddINotifyPropertyChangedInterface]

    public class ProdukcjaRuchEwidencjaUCViewModel_old : ListCommandViewModelBase
    {
        #region Properties
        public IEnumerable<tblProdukcjaRuchTowar> ListaRuchuTowarow { get; set; }
        public tblProdukcjaRuchTowar WybranyTowar { get; set; }
        #endregion


        #region Commands
        public RelayCommand EdytujCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        #endregion

        public ProdukcjaRuchEwidencjaUCViewModel_old(IViewModelService viewModelService)
            : base(viewModelService)
        {
            EdytujCommand = new RelayCommand(EdytujCommandExecute, EdytujCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
        }

        private void UsunCommandExecute()
        {
            throw new NotImplementedException();
        }

        private bool UsunCommandCanExecute()
        {
            throw new NotImplementedException();
        }

        private void EdytujCommandExecute()
        {
            throw new NotImplementedException();
        }

        private bool EdytujCommandCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override async void LoadCommandExecute()
        {
            ListaRuchuTowarow = await UnitOfWork.tblProdukcjaRuchTowar.GetAllAsync();
        }
    }
}
