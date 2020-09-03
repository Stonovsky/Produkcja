using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru.Messages;
using PropertyChanged;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Services;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.Adapters;

namespace GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru
{
    [AddINotifyPropertyChangedInterfaceAttribute]

    public class MagazynStanTowaruViewModel : ListCommandViewModelBase
    {
        private vwMagazynGTX przeslanyMagazyn;

        #region Properties
        public IEnumerable<vwMagazynRuchGTX> ListaTowarow { get; set; }
        public vwMagazynRuchGTX WybranyTowar { get; set; }
        public IEnumerable<vwMagazynGTX> ListaMagazynow { get; set; }
        public vwMagazynGTX WybranyMagazyn { get; set; }
        public string Tytul { get; set; }
        #endregion

        #region Commands
        public RelayCommand ZmianaElementuNaTreeViewCommand { get; set; }
        public RelayCommand WyslijTowarMessengeremCommand { get; set; }

        #endregion

        #region CTOR
        public MagazynStanTowaruViewModel(IViewModelService viewModelService)
            : base(viewModelService)
        {

            WyslijTowarMessengeremCommand = new RelayCommand(WyslijTowarMessengeremCommandExecute);
            ZmianaElementuNaTreeViewCommand = new RelayCommand(ZmianaElementuNaTreeViewCommandExecute);

            Tytul = "Stan magazynu";

            Messenger.Register<vwMagazynGTX>(this, GdyPrzeslanoMagazyn);
        }
        #endregion


        private async void ZmianaElementuNaTreeViewCommandExecute()
        {
            if (WybranyMagazyn is null) return;

            ListaTowarow = await UnitOfWork.vwMagazynRuchGTX.WhereAsync(s => s.IdMagazyn == WybranyMagazyn.IdMagazyn);
            GrupujListeTowarow();

        }

        private void GdyPrzeslanoMagazyn(vwMagazynGTX obj)
        {
            if (obj is null) return;
            przeslanyMagazyn = obj;
        }

        private void WyslijTowarMessengeremCommandExecute()
        {
            if (WybranyTowar is null) return;

            Messenger.Send(WybranyTowar);
        }


        protected override async void LoadCommandExecute()
        {
            ListaTowarow = await UnitOfWork.vwMagazynRuchGTX.GetAllAsync();
            ListaMagazynow = await UnitOfWork.vwMagazynGTX.GetAllAsync();

            await PobierzListeTowarowDlaPrzeslanegoMagazynu();
        }

        private async Task PobierzListeTowarowDlaPrzeslanegoMagazynu()
        {
            if (przeslanyMagazyn is null) return;
            ListaTowarow = await UnitOfWork.vwMagazynRuchGTX.WhereAsync(i => i.IdMagazyn == przeslanyMagazyn.IdMagazyn).ConfigureAwait(false);
            GrupujListeTowarow();
        }

        private void GrupujListeTowarow()
        {
            ListaTowarow = new MagazynRuchGTXGroupAdapter(ListaTowarow).PobierzRuchZgrupowanyPoIdTowar();
        }
    }
}
