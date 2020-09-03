using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.UI.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GAT_Produkcja.ViewModel.Magazyn.RuchTowaru.KodKreskowy
{
    [AddINotifyPropertyChangedInterface]

    public class RuchTowaruKodKreskowyDodajViewModel : ViewModelBase
    {
        private string kodKreskowy;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        #region Properties

        public string KodKreskowy
        {
            get => kodKreskowy;
            set
            {
                kodKreskowy = value;
                WyslijWiadomoscZKodemKreskowym();
            }
        }


        #endregion

        #region CTOR
        public RuchTowaruKodKreskowyDodajViewModel(IViewService viewService, IMessenger messenger)
        {
            this.viewService = viewService;
            this.messenger = messenger;
        }
        #endregion

        private void WyslijWiadomoscZKodemKreskowym()
        {
            messenger.Send(KodKreskowy, "KodKreskowy");
            viewService.Close<RuchTowaruKodKreskowyDodajViewModel>();
        }








    }


}
