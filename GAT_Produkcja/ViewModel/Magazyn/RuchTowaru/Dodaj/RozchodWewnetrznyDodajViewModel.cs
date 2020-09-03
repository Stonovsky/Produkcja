using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using PropertyChanged;
using GalaSoft.MvvmLight.Messaging;

namespace GAT_Produkcja.ViewModel.Magazyn.RozchodWewnetrzny.Dodaj
{
    [AddINotifyPropertyChangedInterface]
    public class RozchodWewnetrznyDodajViewModel : ViewModelBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;
        #region Properties
        public tblRuchTowar WybranyTowarRuch { get; set; }
        public IEnumerable<tblTowar> ListaTowarow { get; set; }
        public tblTowar WybranyTowar { get; set; }
        public IEnumerable<tblMagazyn> ListaMagazynow { get; set; }
        public tblMagazyn WybranyMagazyn { get; set; }
        #endregion

        #region Commands

        #endregion

        #region CTOR
        public RozchodWewnetrznyDodajViewModel(IUnitOfWork unitOfWork, IViewService viewService, IDialogService dialogService, IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.messenger = messenger;

            messenger.Register<vwStanTowaru>("DodajTowar",GdyPrzeslanoDaneTowaru);
        }

        private void GdyPrzeslanoDaneTowaru(vwStanTowaru obj)
        {

            WybranyMagazyn = ListaMagazynow.FirstOrDefault(m => m.IDMagazyn == obj.IDMagazyn);
            WybranyTowarRuch.IDTowar = obj.IDTowar;
            throw new NotImplementedException();
        }


        #endregion
    }
}
