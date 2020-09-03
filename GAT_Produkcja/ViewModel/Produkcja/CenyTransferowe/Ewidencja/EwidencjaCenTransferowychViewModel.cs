using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Dodaj;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Ewidencja
{
    public class EwidencjaCenTransferowychViewModel : ListCommandViewModelBase
    {
        #region Properties
        public IEnumerable<tblProdukcjaRozliczenie_CenyTransferowe> ListaCenTransferowych { get; set; }
        #endregion

        #region Commands
        public RelayCommand ZmienCenyTransferoweCommand { get; set; }
        #endregion
        public EwidencjaCenTransferowychViewModel(IViewModelService viewModelService) : base(viewModelService)
        {
            ZmienCenyTransferoweCommand = new RelayCommand(ZmienCenyTransferoweCommandExecute);

            Messenger.Register<ZmianaCenTrasferowychMessage>(this, GdyZmienionoCenyTransferowe);
        }

        private async void  GdyZmienionoCenyTransferowe(ZmianaCenTrasferowychMessage obj)
        {
            await PobierzCenyTransferoweZBazy();
        }

        private void ZmienCenyTransferoweCommandExecute()
        {
            ViewService.Show<DodajCenyTransferoweViewModel>();
        }

        protected override async void LoadCommandExecute()
        {
            await PobierzCenyTransferoweZBazy();
        }

        private async Task PobierzCenyTransferoweZBazy()
        {
            ListaCenTransferowych = await UnitOfWork.tblProdukcjaRozliczenie_CenyTransferowe.WhereAsync(c => c.CzyAktualna == true);

        }
    }
}
