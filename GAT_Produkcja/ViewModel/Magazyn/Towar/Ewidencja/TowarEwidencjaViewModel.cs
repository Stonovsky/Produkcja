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
using GAT_Produkcja.ViewModel.Magazyn.Towar.Szczegoly;

namespace GAT_Produkcja.ViewModel.Magazyn.Towar.Ewidencja
{
    public class TowarEwidencjaViewModel:ViewModelBase
    {
        #region Properties

        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        private tblTowar wybranyTowar;
        private IEnumerable<tblTowar> listaTowarow;

        private string tytul;


        private string nazwaTowaruDoWyszukania;

        public string NazwaTowaruDoWyszukania
        {
            get { return nazwaTowaruDoWyszukania; }
            set { nazwaTowaruDoWyszukania = value; RaisePropertyChanged(); }
        }

        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }

        public IEnumerable<tblTowar> ListaTowarow
        {
            get { return listaTowarow; }
            set { listaTowarow = value; RaisePropertyChanged(); }
        }

        public tblTowar WybranyTowar
        {
            get { return wybranyTowar; }
            set { wybranyTowar = value; RaisePropertyChanged(); }
        }

        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand PokazSzczegolyCommand { get; set; }
        public RelayCommand DodajTowarCommand { get; set; }
        public RelayCommand SzukajCommand { get; set; }
        public RelayCommand MouseDoubleClickCommand { get; set; }
        #endregion

        #region CTOR
        public TowarEwidencjaViewModel( IUnitOfWorkFactory unitOfWorkFactory,
                                        IDialogService dialogService,
                                        IViewService viewService,
                                        IMessenger messenger
                                        )
        {
            this.unitOfWork = unitOfWorkFactory.Create();
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.messenger = messenger;

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            PokazSzczegolyCommand = new RelayCommand(PokazSzczegolyCommandExecute);
            MouseDoubleClickCommand = new RelayCommand(MouseDoubleClickCommandExecute);
            DodajTowarCommand = new RelayCommand(DodajTowarCommandExecute);
            SzukajCommand = new RelayCommand(SzukajCommandExecute, SzukajCommandCanExecute);

            messenger.Register<string>(this, async s =>  
            {
                if (s == "Odswiez")
                {
                    ListaTowarow = await unitOfWork.tblTowar.GetAllAsync().ConfigureAwait(false);
                    ListaTowarow = ListaTowarow.OrderByDescending(t => t.IDTowar);
                }
            });
        }

        private bool SzukajCommandCanExecute()
        {
            if (String.IsNullOrEmpty(NazwaTowaruDoWyszukania))
                return false;

            return true;
        }


        private async void SzukajCommandExecute()
        {
            ListaTowarow = await unitOfWork.tblTowar.WhereAsync(n => n.Nazwa.Contains(NazwaTowaruDoWyszukania));
        }

        private void DodajTowarCommandExecute()
        {
            viewService.Show<TowarSzczegolyViewModel>();
        }

        private void MouseDoubleClickCommandExecute()
        {
            messenger.Send(WybranyTowar);
        }

        #endregion

        private void PokazSzczegolyCommandExecute()
        {
            messenger.Send(WybranyTowar);
        }
        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            Tytul = "Ewidencja towarów";

            ListaTowarow = await unitOfWork.tblTowar.GetAllInclRelatedTablesAsync().ConfigureAwait(false);
            ListaTowarow = ListaTowarow.OrderBy(t => t.Nazwa);
        }
    }
}
