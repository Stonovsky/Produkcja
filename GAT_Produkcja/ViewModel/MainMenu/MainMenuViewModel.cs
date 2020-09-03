using GAT_Produkcja.db;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.ComponentModel;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly;
using GAT_Produkcja.ViewModel.MainMenu.Zapotrzebowanie;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.Utilities.CurrencyService.NBP;
using System.Threading.Tasks;
using System;
using GAT_Produkcja.ViewModel.MainMenu.Models;
using GAT_Produkcja.Utilities.CurrencyService.NBP.Model;
using GAT_Produkcja.UI.Utilities.Dostep;

namespace GAT_Produkcja.ViewModel.MainMenu
{
    public class MainMenuViewModel : ViewModelServiceBase
    {
        #region Fields
        private readonly IViewModelService viewModelService;
        private readonly INBPService nbpService;

        #endregion

        #region Properties
        public tblPracownikGAT ZalogowanyUzytkownik { get; set; }
        public IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel ZamOdKlientowSzczegolyUCViewModel { get; }
        public IMainMenuZapotrzebowanieViewModel ZapotrzebowanieViewModel { get; }
        public KursyWalutModel  KursyWalut { get; set; }
        #endregion

        #region Commands
        public RelayCommand OdswiezCommand { get; set; }
        public RelayCommand<CancelEventArgs> ZamknijAplikacjeCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        #endregion
        
        #region CTOR
        public MainMenuViewModel(IViewModelService viewModelService,
                                 INBPService nbpService,
                                 IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel szczegolyUCViewModel,
                                 IMainMenuZapotrzebowanieViewModel zapotrzebowanieViewModel
                                 )
            : base(viewModelService)
        {
            this.viewModelService = viewModelService;
            this.nbpService = nbpService;

            ZamOdKlientowSzczegolyUCViewModel = szczegolyUCViewModel;
            ZapotrzebowanieViewModel = zapotrzebowanieViewModel;


            OdswiezCommand = new RelayCommand(OdswiezCommandExecute, OdswiezCommandCanExecute);
            LoadCommand = new RelayCommand(LoadCommandExecute);
            ZamknijAplikacjeCommand = new RelayCommand<CancelEventArgs>(ZamknijAplikacjeCommandExecute);

            Messenger.Register<tblPracownikGAT>(this, "MainMenu", GdyPrzeslanoPracownika);
        }
        #endregion

        private async void LoadCommandExecute()
        {
            ZamOdKlientowSzczegolyUCViewModel.SetState(this.GetType().Name);
            await ZamOdKlientowSzczegolyUCViewModel.LoadAsync(null);
            await ZapotrzebowanieViewModel.LoadAsync(null);
            await PobierzKursyWalut();
            await PobierzKonfiguracjeUrzadzen();
        }

        private async Task PobierzKonfiguracjeUrzadzen()
        {
            UzytkownikZalogowany.KonfiguracjaUrzadzen = await UnitOfWork.tblKonfiguracjaUrzadzen.SingleOrDefaultAsync(k => k.NazwaKomputera == Environment.MachineName);
        }

        private async Task PobierzKursyWalut()
        {
            KursyWalut = new KursyWalutModel();

            try
            {
                KursyWalut.EUR = await nbpService.GetActualCurrencyRate(CurrencyShorcutEnum.EUR);
                KursyWalut.USD = await nbpService.GetActualCurrencyRate(CurrencyShorcutEnum.USD);
                KursyWalut.RUB = await nbpService.GetActualCurrencyRate(CurrencyShorcutEnum.RUB);
            }
            catch (Exception ex)
            {
                
            }

        }

        private async void GdyPrzeslanoPracownika(tblPracownikGAT obj)
        {
            await ActivityLogger.LogUserActivityAsync("Zalogowano");
            ZalogowanyUzytkownik = obj;
        }



        private bool OdswiezCommandCanExecute()
        {
            return ZamOdKlientowSzczegolyUCViewModel.IsButtonActive
                && ZapotrzebowanieViewModel.IsButtonActive;
        }
        private async void OdswiezCommandExecute()
        {
            await ZamOdKlientowSzczegolyUCViewModel.LoadAsync(null);
            await ZapotrzebowanieViewModel.LoadAsync(null);
            await ActivityLogger.LogUserActivityAsync();
        }

        private async void ZamknijAplikacjeCommandExecute(CancelEventArgs obj)
        {
            if (DialogService.ShowQuestion_BoolResult("Czy zamknąć aplikację?"))
            {
                await ActivityLogger.LogUserActivityAsync();
                Application.Current.Shutdown();
            }
            else
            {
                if (obj != null)
                    obj.Cancel = true;
            }
        }


    }
}
