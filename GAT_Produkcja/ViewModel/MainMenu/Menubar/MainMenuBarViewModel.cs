using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.Helpers.Theme;
using GAT_Produkcja.Services;
using GAT_Produkcja.Utilities.BaseClasses;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny;
using GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.ImportBadanZRaportuXls;
using GAT_Produkcja.ViewModel.Finanse;
using GAT_Produkcja.ViewModel.Finanse.Banki.StanKont.EwidencjaStanKont;
using GAT_Produkcja.ViewModel.Finanse.PodsumowanieFinansowe;
using GAT_Produkcja.ViewModel.Finanse.Sprzedaz.Ewidencja;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.RuchTowaru;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.StanTowaru;
using GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja;
using GAT_Produkcja.ViewModel.Magazyn.PrzyjecieZewnetrzne;
using GAT_Produkcja.ViewModel.Magazyn.RuchTowaru;
using GAT_Produkcja.ViewModel.Produkcja.CenyTransferowe.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Geokomorka.Przerob.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Konfekcja.DrukEtykiet;
using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.Naglowek;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Ewidencja;
using GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Ewidencja;
using GAT_Produkcja.ViewModel.ZamowienieOdKlienta;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Analiza;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Ewidencja;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.RozliczenieFV.Ewidencja;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.ViewModel.Konfiguracja.KonfiguracjaUrzadzen;
using GAT_Produkcja.ViewModel.Kontrahent.Ewidencja;

namespace GAT_Produkcja.ViewModel.MainMenu.MenuBar
{
    public class MainMenuBarViewModel : ViewModelServiceBase
    {
        private readonly IThemeChangerHelper themeChangerHelper;

        #region Properties
        public tblPracownikGAT ZalogowanyUzytkownik { get; set; }
        #endregion

        #region Commands

        public RelayCommand<CancelEventArgs> ZamknijAplikacjeCommand { get; set; }



        #region Zapotrzebowanie
        public RelayCommand OtworzZapotrzebowanieEwidencjaCommand { get; set; }
        public RelayCommand DodajZapotrzebowanieCommand { get; set; }
        public RelayCommand OtworzEwidencjeRozliczenZapotrzebowanCommand { get; set; }
        public RelayCommand OtworzAnalizeZapotrzebowanCommand { get; set; }

        #endregion

        #region Badania
        public RelayCommand OtworzEwidencjeBadanCommand { get; set; }
        public RelayCommand DodajBadanieCommand { get; set; }
        #endregion

        #region Kontrahent
        public RelayCommand PokazEwidencjeKontrahentowCommand { get; set; }
        public RelayCommand DodajKontrahentaCommand { get; set; }
        #endregion

        #region Drukowanie Etykiet

        public RelayCommand OtworzDrukowanieEtykietCommand { get; set; }

        #endregion

        #region Magazyn
        public RelayCommand OtworzPrzyjecieZewnetrzneCommand { get; set; }

        public RelayCommand OtworzStanMagazynowyCommand { get; set; }
        public RelayCommand OtworzEwidencjeRuchuTowarowCommand { get; set; }
        public RelayCommand<string> OtworzRuchTowaruCommand { get; set; }

        public RelayCommand OtowrzMagazynEwidencjaSubiektCommand { get; set; }
        #endregion

        #region Produkcja
        public RelayCommand OtworzEwidencjeProdukcjiCommand { get; set; }
        public RelayCommand<object> RejestracjaProdukcjiOtworOknoCommand { get; set; }

        public RelayCommand<object> OtworzZlecenieCommand { get; set; }
        public RelayCommand OtworzZamowienieOdKlientowCommand { get; set; }
        public RelayCommand OtworzEwidencjeRozliczenProdukcjiCommand { get; set; }
        public RelayCommand OtworzEwidencjeCenTransfeorwychCommand { get; set; }
        public RelayCommand OtworzEwidencjePrzerobuGokomorkiCommand { get; set; }
        #endregion

        #region Finanse
        public RelayCommand OtworzNaleznosciIZobowiazaniaCommand { get; set; }
        public RelayCommand OtworzEwidencjeSprzedazyAGGCommand { get; set; }
        public RelayCommand OtworzEwidencjeStanuKontCommand { get; set; }
        public RelayCommand OtworzPodsumowanieFinansoweCommand { get; set; }
        #endregion

        #region Theme
        public RelayCommand<object> ZmienKolorCommand { get; set; }
        #endregion

        #region Konfiguracja
        public RelayCommand OtworzKonfiguracjeUrzadzenCommand { get; set; }
        #endregion

        #endregion

        #region CTOR
        public MainMenuBarViewModel(IViewModelService viewModelService,
                                 IThemeChangerHelper themeChangerHelper)
            : base(viewModelService)
        {

            this.themeChangerHelper = themeChangerHelper;

            ZamknijAplikacjeCommand = new RelayCommand<CancelEventArgs>(ZamknijAplikacjeCommandExecute);


            #region MaterialDesingThemes
            ZmienKolorCommand = new RelayCommand<object>(ZmienKolorCommandExecute);
            Task.Run(() => themeChangerHelper.SetThemeFromDB());
            #endregion



            OtworzZapotrzebowanieEwidencjaCommand = new RelayCommand(OtworzZapotrzebowanieEwidencjaCommandExecute);
            OtworzEwidencjeRozliczenZapotrzebowanCommand = new RelayCommand(OtworzEwidencjeRozliczenZapotrzebowanCommandExecute);
            DodajZapotrzebowanieCommand = new RelayCommand(DodajZapotrzebowanieCommandExecute);
            OtworzAnalizeZapotrzebowanCommand = new RelayCommand(OtworzAnalizeZapotrzebowanCommandExecute);
            OtworzEwidencjeBadanCommand = new RelayCommand(OtworzEwidencjeBadanCommandExecute);
            DodajBadanieCommand = new RelayCommand(DodajBadanieCommandExecute);
            PokazEwidencjeKontrahentowCommand = new RelayCommand(PokazEwidencjeKontrahentowCommandExecute);
            DodajKontrahentaCommand = new RelayCommand(DodajKontrahentaCommandExecute);
            OtworzDrukowanieEtykietCommand = new RelayCommand(OtworzDrukowanieEtykietCommandExecute);

            #region Magazyn
            OtworzPrzyjecieZewnetrzneCommand = new RelayCommand(OtworzPrzyjecieZewnetrzneCommandExecute);
            OtworzStanMagazynowyCommand = new RelayCommand(OtworzStanMagazynowyCommandExecute);
            OtworzEwidencjeRuchuTowarowCommand = new RelayCommand(OtworzEwidencjeRuchuTowarowCommandExecute);
            OtworzRuchTowaruCommand = new RelayCommand<string>(OtworzRuchTowaruCommandExecute);
            OtowrzMagazynEwidencjaSubiektCommand = new RelayCommand(OtowrzMagazynEwidencjaSubiektCommandExecute);

            #endregion


            #region Produkcja
            RejestracjaProdukcjiOtworOknoCommand = new RelayCommand<object>(RejestracjaProdukcjiOtworOknoCommandExecute);
            OtworzZlecenieCommand = new RelayCommand<object>(OtworzZlecenieCommandExecute);
            OtworzEwidencjeProdukcjiCommand = new RelayCommand(OtworzEwidencjeProdukcjiCommandExecute);
            OtworzZamowienieOdKlientowCommand = new RelayCommand(OtworzZamowienieOdKlientowCommandExecute);
            OtworzEwidencjeRozliczenProdukcjiCommand = new RelayCommand(OtworzEwidencjeRozliczenProdukcjiCommandExecute);
            OtworzEwidencjeCenTransfeorwychCommand = new RelayCommand(OtworzEwidencjeCenTransfeorwychCommandExecute);
            OtworzEwidencjePrzerobuGokomorkiCommand = new RelayCommand(OtworzEwidencjePrzerobuGokomorkiCommandExecute);

            #endregion

            #region Finanse
            OtworzNaleznosciIZobowiazaniaCommand = new RelayCommand(OtworzNaleznosciIZobowiazaniaCommandExecute);
            OtworzEwidencjeSprzedazyAGGCommand = new RelayCommand(OtworzEwidencjeSprzedazyAGGCommandExecute);
            OtworzEwidencjeStanuKontCommand = new RelayCommand(OtworzEwidencjeStanuKontCommandExecute);
            OtworzPodsumowanieFinansoweCommand = new RelayCommand(OtworzPodsumowanieFinansoweCommandExecute);

            #endregion

            #region Konfiguracja
            OtworzKonfiguracjeUrzadzenCommand = new RelayCommand(OtworzKonfiguracjeUrzadzenCommandExecute);

            #endregion
        }

        private void OtworzKonfiguracjeUrzadzenCommandExecute()
        {
            ViewService.ShowDialog<KonfiguracjaUrzadzenViewModel>();
        }
        #endregion


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

        private async void OtworzPodsumowanieFinansoweCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<PodsumowanieFinansoweViewModel>();
        }

        private async void OtworzEwidencjePrzerobuGokomorkiCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<EwidencjaPrzerobProdukcjaGeokomorkaViewModel>();
        }

        private async void OtworzEwidencjeStanuKontCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<EwidencjaStanKontViewModel>();
        }

        private async void OtworzEwidencjeCenTransfeorwychCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<EwidencjaCenTransferowychViewModel>();
        }

        private async void OtworzEwidencjeRozliczenProdukcjiCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<RozliczenieMsAccessEwidencjaViewModel>();
        }

        private async void OtworzEwidencjeSprzedazyAGGCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<SprzedazEwidencjaViewModel>();
        }

        private async void OtworzNaleznosciIZobowiazaniaCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<FinanseZobowiazaniaNaleznosciViewModel>();
        }


        private async void OtworzZamowienieOdKlientowCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<ZamowienieOdKlientaEwidencjaViewModel>();
        }

        private async void OtowrzMagazynEwidencjaSubiektCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<MagazynEwidencjaSubiektViewModel>();
        }

        private async void OtworzEwidencjeProdukcjiCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<ProdukcjaEwidencjaViewModel>();
        }

        private async void OtworzZlecenieCommandExecute(object obj)
        {
            await ActivityLogger.LogUserActivityAsync();

            string naglowek = (string)obj;
            if (naglowek is null) return;

            switch (naglowek)
            {
                case "Zlecenie produkcyjne":
                    ViewService.Show<ZlecenieProdukcyjneEwidencjaViewModel>();
                    break;

                case "Zlecenie cięcia":
                    ViewService.Show<ZlecenieCieciaEwidencjaViewModel>();
                    break;

                default:
                    break;
            }
        }

        private async void RejestracjaProdukcjiOtworOknoCommandExecute(object obj)
        {
            await ActivityLogger.LogUserActivityAsync();

            string naglowek = (string)obj;

            if (naglowek == null)
                return;

            switch (naglowek)
            {
                case "Linia włóknin":
                    ViewService.Show<GPRuchNaglowekViewModel>();
                    Messenger.Send(new tblProdukcjaGniazdoProdukcyjne
                    {
                        IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaWloknin
                    });
                    break;
                case "Linia do kalandrowania":
                    ViewService.Show<GPRuchNaglowekViewModel>();
                    Messenger.Send(new tblProdukcjaGniazdoProdukcyjne
                    {
                        IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKalandowania
                    });
                    break;
                case "Linia do konfekcji":
                    ViewService.Show<GPRuchNaglowekViewModel>();
                    Messenger.Send(new tblProdukcjaGniazdoProdukcyjne
                    {
                        IDProdukcjaGniazdoProdukcyjne = (int)GniazdaProdukcyjneEnum.LiniaDoKonfekcji
                    });
                    break;
                default:
                    break;
            }
        }



        private async void OtworzRuchTowaruCommandExecute(string obj)
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<RuchTowaruViewModel>();
            switch (obj)
            {
                case "PZ":
                    Messenger.Send(StatusRuchuTowarowEnum.PrzyjecieZewnetrne_PZ);
                    break;
                case "WZ":
                    Messenger.Send(StatusRuchuTowarowEnum.WydanieZewnetrzne_WZ);
                    break;
                case "RW":
                    Messenger.Send(StatusRuchuTowarowEnum.RozchodWewnetrzny_RW);
                    break;
                case "MM":
                    Messenger.Send(StatusRuchuTowarowEnum.PrzesuniecieMiedzymagazynowe_MM);
                    break;
                default:
                    break;
            }
        }

        private async void OtworzEwidencjeRuchuTowarowCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<MagazynRuchTowaruViewModel>();
        }

        private async void OtworzStanMagazynowyCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<MagazynStanTowaruViewModel>();
        }

        private async void OtworzPrzyjecieZewnetrzneCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<PrzyjecieZewnetrzneViewModel>();
        }

        private async void OtworzAnalizeZapotrzebowanCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<AnalizaZapotrzebowanViewModel>();
        }


        private async void OtworzEwidencjeRozliczenZapotrzebowanCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<ZapotrzebowanieRozliczenieFVEwidencjaViewModel>();
        }

        private async void DodajKontrahentaCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<DodajKontrahentaViewModel>();
        }

        private async void PokazEwidencjeKontrahentowCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<EwidencjaKontrahentowViewModel>();
        }

        private async void DodajBadanieCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<ImportBadanZRaportuXlsViewModel>();
        }

        private async void OtworzEwidencjeBadanCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<BadaniaGeowlokninViewModel>();
        }


        private async void DodajZapotrzebowanieCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<ZapotrzebowanieDodajViewModel>();
        }

        private async void OtworzZapotrzebowanieEwidencjaCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<ZapotrzebowanieEwidencjaViewModel>();
        }

        private async void GdyPrzeslanoPracownika(tblPracownikGAT obj)
        {
            await ActivityLogger.LogUserActivityAsync("Zalogowano");
            ZalogowanyUzytkownik = obj;
        }

        private async void OtworzDrukowanieEtykietCommandExecute()
        {
            await ActivityLogger.LogUserActivityAsync();
            ViewService.Show<DrukEtykietViewModel>();
        }


        private async void ZmienKolorCommandExecute(object obj)
        {
            await ActivityLogger.LogUserActivityAsync();

            var color = (string)obj;
            ThemeColorEnum themeColorEnum = ThemeColorEnum.Dark;

            if (color.ToLower().Contains("jasny"))
            {
                themeColorEnum = ThemeColorEnum.Light;
            }
            else if (color.ToLower().Contains("Ciemny"))
            {
                themeColorEnum = ThemeColorEnum.Dark;
            }

            themeChangerHelper.ChangeTheme(themeColorEnum);
            await themeChangerHelper.AddToDataBase(ZalogowanyUzytkownik?.ID_PracownikGAT, themeColorEnum);
        }
    }
}
