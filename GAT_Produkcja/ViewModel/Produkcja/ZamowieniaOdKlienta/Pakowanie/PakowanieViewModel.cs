using GalaSoft.MvvmLight;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using GAT_Produkcja.UI.Services;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.Pakowanie
{
    [AddINotifyPropertyChangedInterface]

    public class PakowanieViewModel : ViewModelBase, IPakowanieViewModel
    {
        #region Properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;
        private tblZamowienieHandlowe zamowienieHandlowe;
        private tblZamowienieHandlowePakowanie wybranePakowanie;
        private ObservableCollection<tblZamowienieHandlowePakowanie> listaPakowanie;

        public IEnumerable<tblZamowienieHandlowePakowanieRodzaj> ListaRodzajowPakowania { get; set; }
        public tblZamowienieHandlowePakowanieRodzaj WybranyRodzajPakowania { get; set; }
        public ObservableCollection<tblZamowienieHandlowePakowanie> ListaPakowanie { get; set; }
        public tblZamowienieHandlowePakowanie WybranePakowanie { get; set; }

        #endregion

        public RelayCommand UsunCommand { get; set; }
        public RelayCommand ZaladujPodczasUruchomieniaCommand { get; set; }
        public RelayCommand PoEdycjiKomorkiDataGridCommand { get; set; }
        public bool IsValid { get; private set; }

        #region CTOR
        public PakowanieViewModel(IUnitOfWork unitOfWork, IDialogService dialogService, IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.dialogService = dialogService;
            this.messenger = messenger;
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            ZaladujPodczasUruchomieniaCommand = new RelayCommand(ZaladujPodczasUruchomieniaCommandExecute);
            PoEdycjiKomorkiDataGridCommand = new RelayCommand(PoEdycjiKomorkiDataGridCommandExecute);
            messenger.Register<tblZamowienieHandlowe>(this, "PrzeslaneZamowienie", GdyPrzeslanoZamowinieHandlowe);
            messenger.Register<tblZamowienieHandlowe>(this, "ZapiszTowar", GdyPrzeslanoZapisz);


            ListaPakowanie = new ObservableCollection<tblZamowienieHandlowePakowanie>();
            IsValid = true;
        }

        private void PoEdycjiKomorkiDataGridCommandExecute()
        {
            ValidujModel();
        }

        private async void GdyPrzeslanoZapisz(tblZamowienieHandlowe obj)
        {
            zamowienieHandlowe = obj;
            await Zapisz();
        }

        private async void GdyPrzeslanoZamowinieHandlowe(tblZamowienieHandlowe obj)
        {
            zamowienieHandlowe = obj;
            ListaPakowanie = new ObservableCollection<tblZamowienieHandlowePakowanie>
                                (await unitOfWork.tblZamowienieHandlowePakowanie
                                    .WhereAsync(p => p.IDZamowienieHandlowe == zamowienieHandlowe.IDZamowienieHandlowe));
        }

        private async void ZaladujPodczasUruchomieniaCommandExecute()
        {
            ListaRodzajowPakowania = await unitOfWork.tblZamowienieHandlowePakowanieRodzaj.GetAllAsync();
        }

        private bool UsunCommandCanExecute()
        {
            if (WybranePakowanie != null)
            {
                return true;
            }
            return false;
        }

        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć wybrany sposób pakowania") == false)
                return;

            if (WybranePakowanie.IDZamowienieHandlowePakowanie != 0)
            {
                unitOfWork.tblZamowienieHandlowePakowanie.Remove(WybranePakowanie);
                await unitOfWork.SaveAsync();
            }
            ListaPakowanie.Remove(WybranePakowanie);
        }


        public void ValidujModel()
        {
            if (ListaPakowanie == null)
            {
                IsValid = true;
                return;
            }

            foreach (var pakowania in ListaPakowanie)
            {
                if (!pakowania.IsValid)
                {
                    IsValid = false;
                    return;
                }
            }
            IsValid = true;
        }


        private async Task Zapisz()
        {
            if (ListaPakowanie == null || ListaPakowanie.Count() == 0)
                return;

            foreach (var pakowanie in ListaPakowanie)
            {
                pakowanie.IDZamowienieHandlowe = zamowienieHandlowe.IDZamowienieHandlowe;

                if (pakowanie.IDZamowienieHandlowePakowanie == 0)
                {
                    unitOfWork.tblZamowienieHandlowePakowanie.Add(pakowanie);
                }
            }
            await unitOfWork.SaveAsync();
        }
        #endregion
    }
}

