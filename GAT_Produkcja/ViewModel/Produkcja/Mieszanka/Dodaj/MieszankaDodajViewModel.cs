using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using GAT_Produkcja.db;
using System.Collections.ObjectModel;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using PropertyChanged;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Controls;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Ewidencja;

namespace GAT_Produkcja.ViewModel.Produkcja.Mieszanka.Dodaj
{
    [AddINotifyPropertyChangedInterface]
    public class MieszankaDodajViewModel : ViewModelBase
    {
        #region Properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IMessenger messenger;

        public ObservableCollection<tblMieszankaSklad> SkladMieszanki { get; set; }
        public tblMieszankaSklad WybranaPozycjaMieszanki { get; set; }
        public IEnumerable<tblTowar> ListaSurowcow { get; set; }
        public tblTowar WybranySurowiec { get; set; }
        public tblMieszanka Mieszanka { get; set; }
        public IEnumerable<tblJm> ListaJm { get; set; }
        public tblJm WybranaJm { get; set; }
        public string Tytul { get; set; }

        private IEnumerable<tblTowar> surowceZdb;

        public IEnumerable<tblPracownikGAT> PracownicyGAT { get; set; }
        public tblPracownikGAT WybranyPracownikGAT { get; set; }


        public RelayCommand<CancelEventArgs> ZamknijOknoCommand { get; set; }
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand PoEdycjiKomorkiDataGridCommand { get; set; }
        public RelayCommand<AddingNewItemEventArgs> PodczasDodawaniaNowegoWierszaCommand { get; set; }

        #endregion

        #region CTOR
        public MieszankaDodajViewModel(IUnitOfWork unitOfWork, IViewService viewService, IDialogService dialogService, IMessenger messenger)
        {
            this.unitOfWork = unitOfWork;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.messenger = messenger;

            ZamknijOknoCommand = new RelayCommand<CancelEventArgs>(ZamknijOknoCommandExecute);
            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);
            PoEdycjiKomorkiDataGridCommand = new RelayCommand(PoEdycjiKomorkiDataGridCommandExecute);
            PodczasDodawaniaNowegoWierszaCommand = new RelayCommand<AddingNewItemEventArgs>(PodczasDodawaniaNowegoWierszaCommandExecute);

            Mieszanka = new tblMieszanka();
            Mieszanka.DataUtworzenia = DateTime.Now.Date;
            Mieszanka.IDPracownikGAT = UzytkownikZalogowany.Uzytkownik!=null? UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT : 1;

            SkladMieszanki = new ObservableCollection<tblMieszankaSklad>();

            Tytul = "Nowa mieszanka";
        }

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            surowceZdb = await unitOfWork.tblTowar.PobierzSurowceAsync().ConfigureAwait(false);
            PracownicyGAT = await unitOfWork.tblPracownikGAT.PobierzPracownikowMogacychZglaszacZapotrzebowaniaAsync().ConfigureAwait(false);
            ListaJm = await unitOfWork.tblJm.GetAllAsync();

            ListaSurowcow = surowceZdb;
        }


        private void ZamknijOknoCommandExecute(CancelEventArgs obj)
        {
            if (!unitOfWork.tblMieszanka.HasChanges())
                return;

            if (dialogService.ShowQuestion_BoolResult("Czy chcesz zamknąć okno? \rWprowadzone zmiany zostaną utracone..."))
                obj.Cancel = false;

            obj.Cancel = true;
        }

        private void PodczasDodawaniaNowegoWierszaCommandExecute(AddingNewItemEventArgs obj)
        {
        }

        private void PoEdycjiKomorkiDataGridCommandExecute()
        {
            foreach (var item in SkladMieszanki)
            {
                if (item.IDTowar == 0)
                    item.Udzial = 0;
                else if (SkladMieszanki.Sum(s => s.Ilosc) == 0)
                    item.Udzial = 0;
                else
                    item.Udzial = item.Ilosc / SkladMieszanki.Sum(s => s.Ilosc);
            }

            RaisePropertyChanged(nameof(SkladMieszanki));
            RaisePropertyChanged(nameof(Mieszanka));

            Mieszanka.Ilosc = SkladMieszanki.Sum(s => s.Ilosc);
        }


        private async void UsunCommandExecute()
        {
            if (dialogService.ShowQuestion_BoolResult("Czy usunąć mieszankę?"))
            {
                unitOfWork.tblMieszanka.Remove(Mieszanka);
                await unitOfWork.SaveAsync();
            }
        }

        private bool UsunCommandCanExecute()
        {
            if (Mieszanka.IDMieszanka == 0)
                return false;

            return true;
        }

        private async void ZapiszCommandExecute()
        {
            if (Mieszanka.IDMieszanka == 0)
                unitOfWork.tblMieszanka.Add(Mieszanka);

            foreach (var item in SkladMieszanki)
            {
                item.IDMieszanka = Mieszanka.IDMieszanka;

                if (item.IDMieszankaSklad == 0)
                    unitOfWork.tblMieszankaSklad.Add(item);
            }

            await unitOfWork.SaveAsync();
            messenger.Send<string, MieszankaEwidencjaViewModel>("Odswiez");
        }

        private bool ZapiszCommandCanExecute()
        {
            if (!Mieszanka.IsValid)
                return false;

            if (Mieszanka.Ilosc == 0)
                return false;

            if (SkladMieszanki.Count() == 0)
                return false;

            foreach (var pozycja in SkladMieszanki)
            {
                if (!pozycja.IsValid)
                    return false;
            }

            return true;
        }
        #region Methods
        #endregion


        #endregion
    }
}
