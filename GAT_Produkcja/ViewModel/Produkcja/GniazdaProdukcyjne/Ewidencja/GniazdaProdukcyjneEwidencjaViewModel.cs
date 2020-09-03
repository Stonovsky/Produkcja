using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Dodaj;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Ewidencja
{
    [AddINotifyPropertyChangedInterface]

    public class GniazdaProdukcyjneEwidencjaViewModel : ViewModelBase
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;
        private tblTowarGrupa wybranaGrupaDlaGniazd;

        #endregion

        #region Properties
        public IEnumerable<tblProdukcjaGniazdoProdukcyjne> ListaGniazdProdukcyjnych { get; set; }
        public tblProdukcjaGniazdoProdukcyjne WybraneGniazdoProdukcyjne { get; set; }
        public IEnumerable<tblTowarGrupa> ListaGrupDlaGniazd { get; set; }
        public tblTowarGrupa WybranaGrupaDlaGniazd
        {
            get => wybranaGrupaDlaGniazd;
            set => wybranaGrupaDlaGniazd = value;
        }
        #endregion

        #region Commands
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand WyslijMessageZGniazdemCommand { get; set; }
        public RelayCommand DodajGniazdoCommand { get; set; }
        public RelayCommand UsunGniazdoCommand { get; set; }
        public RelayCommand ZmianaElementuNaTreeViewCommand { get; set; }
        #endregion

        #region CTOR
        public GniazdaProdukcyjneEwidencjaViewModel(IUnitOfWork unitOfWork,
                                                    IUnitOfWorkFactory unitOfWorkFactory,
                                                    IViewService viewService,
                                                    IDialogService dialogService,
                                                    IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.messenger = messenger;

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            WyslijMessageZGniazdemCommand = new RelayCommand(WyslijMessageZGniazdemCommandExecute);
            DodajGniazdoCommand = new RelayCommand(DodajGniazdoCommandExecute);
            UsunGniazdoCommand = new RelayCommand(UsunGniazdoCommandExecute, UsunGniazdoCommandCanExecute);
            ZmianaElementuNaTreeViewCommand = new RelayCommand(ZmianaElementuNaTreeViewCommandExecute);

            messenger.Register<string>(this, GdyPrzeslanoOdswiez);

            WybraneGniazdoProdukcyjne = new tblProdukcjaGniazdoProdukcyjne();
        }

        private void GdyPrzeslanoOdswiez(string obj)
        {
            if (obj=="Odswiez")
            {
                ZaladujWartosciPoczatkoweCommandExecute();
            }
        }

        private async void ZmianaElementuNaTreeViewCommandExecute()
        {
            ListaGniazdProdukcyjnych = await unitOfWork.tblProdukcjaGniazdoProdukcyjne.WhereAsync(l => l.IDTowarGrupa == WybranaGrupaDlaGniazd.IDTowarGrupa);
        }

        private async void UsunGniazdoCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć wybrane gniazdo produkcyjne?"))
            {
                unitOfWork.tblProdukcjaGniazdoProdukcyjne.Remove(WybraneGniazdoProdukcyjne);
                await unitOfWork.SaveAsync();
                messenger.Send("Odswiez");
            }
        }

        private bool UsunGniazdoCommandCanExecute()
        {
            if (WybraneGniazdoProdukcyjne == null)
                return false;
            if (WybraneGniazdoProdukcyjne.IDProdukcjaGniazdoProdukcyjne == 0)
                return false;

            return true;
        }

        private void DodajGniazdoCommandExecute()
        {
            viewService.ShowDialog<GniazdaProdukcyjneDodajViewModel>();
        }
        #endregion
        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            ListaGniazdProdukcyjnych = await unitOfWork.tblProdukcjaGniazdoProdukcyjne.GetAllAsync();
            ListaGrupDlaGniazd = await unitOfWork.tblTowarGrupa.PobierzGrupeTowarowDlaGniazdAsync();
        }

        private void WyslijMessageZGniazdemCommandExecute()
        {
            if (WybraneGniazdoProdukcyjne != null)
                messenger.Send(WybraneGniazdoProdukcyjne);
        }

    }
}
