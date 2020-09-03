using System.Collections.Generic;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using System.Threading.Tasks;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.PobieranieBadanZPliku;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.ViewModel.MainMenu;
using System;
using GAT_Produkcja.Utilities.ZebraPrinter;
using PropertyChanged;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Konfekcja.DrukEtykiet;
using GAT_Produkcja.Helpers.Theme;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Services;

namespace GAT_Produkcja.UI.ViewModel.Login
{
    public class LoginViewModel : ViewModelServiceBase
    {
        private string kodKreskowy;
        private readonly IViewModelService viewModelService;

        public RelayCommand<object> ZalogujCommand { get; private set; }
        public RelayCommand AnulujCommand { get; set; }
        public tblPracownikGAT ZalogowanyUzytkownik { get; set; }
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }

        #region Properties


        /// <summary>
        /// przechowuje wybranego w comoboxie pracownika - zbindowane z combobox -> 
        /// haslo jest przechwytywane dopiero w ZalogujCommandExecute i ZalogujCommandCanExecute jako parametr
        /// </summary>
        public tblPracownikGAT PracownikGAT { get; set; }

        /// <summary>
        /// Lista pracownikow zbindowana z comobobox
        /// </summary>
        public List<tblPracownikGAT> Pracownicy { get; set; }

        /// <summary>
        /// Flaga sluzaca do pokazywania wiadomosci o blednym hasle
        /// </summary>
        public bool CzyPokazacWiadomosc { get; set; }

        public string BladneLogowanieText { get; set; } = "Błędne hasło. Spróbuj ponownie";

        public string KodKreskowy
        {
            get => kodKreskowy;
            set
            {
                kodKreskowy = value;
                ZalogujZKoduKreskowego();
            }
        }
        #endregion

        #region CTOR
        public LoginViewModel(IViewModelService viewModelService)
            :base(viewModelService)
        {
            this.viewModelService = viewModelService;

            ZalogujCommand = new RelayCommand<object>(ZalogujCommandExecute, ZalogujCommandCanExecute);
            AnulujCommand = new RelayCommand(AnulujCommandExecute);
            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);
        }
        #endregion

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            try
            {
                await PobierzPracownikowDoLogowaniaAsync();
            }
            catch (Exception ex)
            {
                DialogService.ShowInfo_BtnOK(ex.Message);
            }
        }

        private void AnulujCommandExecute()
        {
            System.Windows.Application.Current.Shutdown();
        }


        #region BtnZaloguj
        // Predykat zawsze BOOL
        public bool ZalogujCommandCanExecute(object parameter)
        {
            PasswordBox passwordBox = (PasswordBox)parameter;
            if (PracownikGAT != null && !string.IsNullOrEmpty(passwordBox.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Metdoa EXECUTE zawsze VOID
        public void ZalogujCommandExecute(object parameter)
        {
            PasswordBox passwordBox = (PasswordBox)parameter;

            if (passwordBox.Password == PracownikGAT.HasloPracownika)
            {
                CzyPokazacWiadomosc = false;
                UzytkownikZalogowany.Uzytkownik = PracownikGAT;
                ZamknijBiezaceOknoIUruchomGlowne();
                Messenger.Send(PracownikGAT, "MainMenu");
            }
            else
            {
                CzyPokazacWiadomosc = true;
            }
        }
        #endregion

        private async Task PobierzPracownikowDoLogowaniaAsync()
        {
            try
            {
                Pracownicy = await UnitOfWork.tblPracownikGAT.PobierzPracownikowPracujacychAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ZamknijBiezaceOknoIUruchomGlowne()
        {
            ViewService.Show<MainMenuViewModel>();
            ViewService.Close<LoginViewModel>();
        }

        private async void ZalogujZKoduKreskowego()
        {
            var pracownik = await UnitOfWork.tblPracownikGAT.SingleOrDefaultAsync(p => p.KodKreskowy == KodKreskowy).ConfigureAwait(false);
            if (pracownik!=null)
            {
                UzytkownikZalogowany.Uzytkownik = pracownik;
                ViewService.Show<MainMenuViewModel>();
                Messenger.Send(pracownik,"MainMenu");
                ViewService.Show<DrukEtykietViewModel>();
                ViewService.Close<LoginViewModel>();
            }
        }

    }
}
