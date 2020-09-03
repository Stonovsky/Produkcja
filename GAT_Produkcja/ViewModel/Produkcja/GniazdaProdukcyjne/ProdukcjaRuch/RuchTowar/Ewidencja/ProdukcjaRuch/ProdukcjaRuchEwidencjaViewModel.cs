using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Services;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Ewidencja
{
    [AddINotifyPropertyChangedInterface]

    public class ProdukcjaRuchEwidencjaViewModel : ListCommandViewModelBase
    {
        #region Fields

        #endregion

        #region Properties
        public string Tytul { get; set; }
        public IProdukcjaRuchEwidencjaUCViewModel EwidencjaUCViewModel { get; }

        #endregion

        #region Commands
        public  RelayCommand  AddCommand{ get; set; }
        #endregion

        #region CTOR

        public ProdukcjaRuchEwidencjaViewModel(IViewModelService viewModelService,
                                               IProdukcjaRuchEwidencjaUCViewModel produkcjaRuchEwidencjaUCViewModel
            )
            : base(viewModelService)
        {
            EwidencjaUCViewModel = produkcjaRuchEwidencjaUCViewModel;

            AddCommand = new RelayCommand(AddCommandExecute);

            Tytul = "Ewidencja ruchu na poszczególnych gniazdach";
        }

        private void AddCommandExecute()
        {
            ViewService.Show<GPRuchNaglowekViewModel>();
        }


        #endregion

        /// <summary>
        /// Pobiera na UC -> EwidencjaUCViewModel liste wyprodykowanych towarow
        /// </summary>
        protected override async void LoadCommandExecute()
        {
           await EwidencjaUCViewModel.LoadAsync(null);
        }
    }
}
