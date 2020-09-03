using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja;
using GAT_Produkcja.Utilities.BarCodeGenerator;

namespace GAT_Produkcja.ViewModel.Magazyn.PrzyjecieZewnetrzne.DodajTowarDoMagazynu
{
    public class DodajTowarDoMagazynuViewModel : ViewModelBase
    {
        #region Properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        private tblTowar towar;
        private tblRuchTowar towarRuch;
        private IEnumerable<tblJm> listaJM;
        private tblJm wybranaJM;
        private string tytul;

        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand PokazEwidencjeTowarowCommand { get; set; }
        public RelayCommand PobierzUniqueIDCommand { get; set; }

        public RelayCommand DrukujEtykieteCommand { get; set; }

        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        public tblTowar Towar
        {
            get { return towar; }
            set { towar = value; RaisePropertyChanged(); }
        }

        public tblRuchTowar TowarRuch
        {
            get { return towarRuch; }
            set { towarRuch = value; RaisePropertyChanged(); }
        }

        public IEnumerable<tblJm> ListaJM
        {
            get { return listaJM; }
            set { listaJM = value; RaisePropertyChanged(); }
        }

        public tblJm WybranaJM
        {
            get { return wybranaJM; }
            set { wybranaJM = value; RaisePropertyChanged(); }
        }
        #endregion


        #region CTOR
        public DodajTowarDoMagazynuViewModel(IUnitOfWorkFactory unitOfWorkFactory,
                                             IDialogService dialogService,
                                             IViewService viewService,
                                             IMessenger messenger)
        {
            unitOfWork = unitOfWorkFactory.Create();
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.messenger = messenger;

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExectue, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            PokazEwidencjeTowarowCommand = new RelayCommand(PokazEwidencjeTowarowCommandExecute);
            PobierzUniqueIDCommand = new RelayCommand(PobierzUniqueIDCommandExecute);
            DrukujEtykieteCommand = new RelayCommand(DrukujEtykieteCommandExecute);
            messenger.Register<tblTowar>(this, GdyPrzeslanoTowar);

            TowarRuch = new tblRuchTowar();
            //TowarRuch.MetaSetUp();
        }

        private void DrukujEtykieteCommandExecute()
        {
            throw new NotImplementedException();
        }

        private void PobierzUniqueIDCommandExecute()
        {
            TowarRuch.NrParti = BarCodeGenerator.GetUniqueId();
            RaisePropertyChanged(nameof(TowarRuch));
        }

        private void GdyPrzeslanoTowar(tblTowar obj)
        {
            Towar = obj;
            TowarRuch.IDTowar = Towar.IDTowar;
            viewService.Close<TowarEwidencjaViewModel>();
        }

        private void PokazEwidencjeTowarowCommandExecute()
        {
            viewService.Show<TowarEwidencjaViewModel>();
        }

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            Tytul = "Doda do magazynu";
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
                ListaJM = await unitOfWork.tblJm.GetAllAsync();
            }
        }
        #endregion

        private bool UsunCommandCanExecute()
        {
            if (TowarRuch.IDRuchTowar == 0)
                return false;

            return true;
        }


        private void UsunCommandExecute()
        {
            throw new NotImplementedException();
        }

        private void ZapiszCommandExectue()
        {
            messenger.Send(TowarRuch);
            viewService.Close<DodajTowarDoMagazynuViewModel>();
        }

        private bool ZapiszCommandCanExecute()
        {
            if (TowarRuch.IDTowar!=0)
            {
                Task.Run(()=>ObliczIloscPrzedIPo());
            }
            if (TowarRuch.IsValid)
                return true;

            return false;
        }

        private async Task ObliczIloscPrzedIPo()
        {
            var pozycjeDlaTowaru = await unitOfWork.tblRuchTowar.WhereAsync(t => t.IDTowar == Towar.IDTowar);
            TowarRuch.IloscPrzed = pozycjeDlaTowaru.Sum(s => s.Ilosc);
            TowarRuch.IloscPo = TowarRuch.IloscPrzed  + TowarRuch.Ilosc ?? 0;
            RaisePropertyChanged(nameof(TowarRuch));
        }
    }
}
