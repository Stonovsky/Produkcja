using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.RozliczenieFV.Szczegoly;
using GalaSoft.MvvmLight.Messaging;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.RozliczenieFV.Ewidencja
{
    public class ZapotrzebowanieRozliczenieFVEwidencjaViewModel:ViewModelBase
    {

        #region Properties
        private IEnumerable<vwFVKosztowezSubiektGT> listaRozliczenKosztowWSubiektGT;
        private vwFVKosztowezSubiektGT vwFVKosztowezSubiektGT;
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        private string tytul;


        public RelayCommand OnWindowLoadedCommand{ get; set; }
        public RelayCommand PokazSzczegolyCommand { get; set; }

        public IEnumerable<vwFVKosztowezSubiektGT> ListaRozliczenKosztowWSubiektGT
        {
            get { return listaRozliczenKosztowWSubiektGT; }
            set { listaRozliczenKosztowWSubiektGT = value; RaisePropertyChanged(); }
        }


        public vwFVKosztowezSubiektGT WybraneRozliczenieKosztow
        {
            get { return vwFVKosztowezSubiektGT; }
            set { vwFVKosztowezSubiektGT = value; RaisePropertyChanged(); }
        }
        #endregion


        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        #region CTOR
        public ZapotrzebowanieRozliczenieFVEwidencjaViewModel(IUnitOfWork unitOfWork, IViewService viewService,IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.messenger = messenger;

            OnWindowLoadedCommand = new RelayCommand(OnWindowLoadedCommandExecute);
            PokazSzczegolyCommand = new RelayCommand(PokazSzczegolyCommandExecute);

        }

        private void PokazSzczegolyCommandExecute()
        {
            viewService.Show<ZapotrzebowanieRozliczenieFVSzczegolyViewModel>();
            messenger.Send(WybraneRozliczenieKosztow);
        }

        private async void OnWindowLoadedCommandExecute()
        {
            ListaRozliczenKosztowWSubiektGT = await unitOfWork.vwFVKosztowezSubiektGT.GetAllAsync();
            ListaRozliczenKosztowWSubiektGT = ListaRozliczenKosztowWSubiektGT.OrderByDescending(d => d.DataOtrzymania);

            Tytul = "Ewidencja rozliczeń zapotrzebowań";
        }
        #endregion

        #region Methods

        #endregion


    }
}
