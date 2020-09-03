using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Kontrahent.Dodaj;
using GAT_Produkcja.Utilities.FilesManipulations;
using System.Collections.ObjectModel;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj.DodajPozycje;
using GAT_Produkcja.Utilities.MailSenders;
using GAT_Produkcja.ViewModel.Zapotrzebowanie.Pliki;
using GAT_Produkcja.UI.Utilities.Dostep;
using GAT_Produkcja.Utilities.MailSenders.MailMessages;
using GAT_Produkcja.Utilities.MailSenders.MailAddresses;
using PropertyChanged;
using System.ComponentModel;
using GAT_Produkcja.UI.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.ViewModel.Kontrahent.Ewidencja;
using GAT_Produkcja.db.Enums;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj
{
    [AddINotifyPropertyChangedInterface]

    public class ZapotrzebowanieDodajViewModel : ViewModelBase
    {
        #region Fields
        private tblZapotrzebowanie zapotrzebowanie;
        private tblKontrahent kontrahent;
        private IEnumerable<tblPracownikGAT> listaPracownikowGAT;
        private tblPracownikGAT wybranyPracownikGAT;
        private tblZapotrzebowanieStatus wybranyStatus;
        private IEnumerable<tblZapotrzebowanieStatus> listaStatusow;
        private tblFirma wybranaFirma;
        private IEnumerable<tblFirma> listaFirm;
        private IEnumerable<tblJm> listaJendostekMiar;
        private tblJm wybranaJednostkaMiary;
        private ObservableCollection<tblPliki> listaPlikow;
        private tblPliki wybranyPlik;
        private ObservableCollection<tblZapotrzebowaniePozycje> listaPozycjiZapotrzebowan;
        private tblZapotrzebowaniePozycje wybranaPozycjaZapotrzebowania;
        private string tytul;
        private IEnumerable<tblKlasyfikacjaOgolna> listaKlasyfikacjiOgolnej;
        private IEnumerable<tblKlasyfikacjaSzczegolowa> listaKlasyfikacjiSzczegolowej;
        private IEnumerable<tblUrzadzenia> listaUrzadzen;
        private tblKlasyfikacjaOgolna wybranaKlasyfikacjaOgolna;
        private tblKlasyfikacjaSzczegolowa wybranaKlasyfikacjaSzczegolowa;
        private tblUrzadzenia wybraneUrzadzenie;
        private string tooltipZapiszCommand;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IViewService viewService;
        private readonly IDialogService dialogService;
        private readonly IFilesManipulation filesManipulation;
        private readonly IGmailSender gmailSender;
        private readonly IPlikiCRUD plikiCRUD;
        private readonly IMessenger messenger;
        private readonly IOutlookMailSender outlookMailSender;
        private bool pozycjeZapotrzebowania_IsChanged;
        private bool czyZapotrzebowanieZweryfikowane; 
        #endregion
        
        #region Properties
        public RelayCommand PokazEwidencjeKontrahentowCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public RelayCommand DodajPlikCommand { get; set; }
        public RelayCommand UsunPlikCommand { get; set; }
        public RelayCommand OtworzPlikCommand { get; set; }
        public RelayCommand DodajKosztCommand { get; set; }
        public RelayCommand EdytujKosztCommand { get; set; }
        public RelayCommand UsunPozycjeKosztowaCommand { get; set; }
        public RelayCommand PokazPozycjeKosztowaCommand { get; set; }
        public RelayCommand<CancelEventArgs> ZamknijOknoCommand { get; set; }
        public RelayCommand ZaladujWartosciPoczatkoweCommand { get; set; }


        public string TooltipZapiszCommand
        {
            get { return tooltipZapiszCommand; }
            set { tooltipZapiszCommand = value; RaisePropertyChanged(); }
        }

        public IEnumerable<tblKlasyfikacjaOgolna> ListaKlasyfikacjiOgolnej
        {
            get { return listaKlasyfikacjiOgolnej; }
            set
            {
                listaKlasyfikacjiOgolnej = value;
                RaisePropertyChanged();
            }
        }

        public tblKlasyfikacjaOgolna WybranaKlasyfikacjaOgolna
        {
            get { return wybranaKlasyfikacjaOgolna; }
            set
            {
                wybranaKlasyfikacjaOgolna = value;
                RaisePropertyChanged();

                if (WybranaKlasyfikacjaOgolna != null)
                    PobierzListeKlasyfikacjiSzczegolowejIPlatnika();
            }
        }


        public IEnumerable<tblKlasyfikacjaSzczegolowa> ListaKlasyfikacjiSzczegolowej
        {
            get { return listaKlasyfikacjiSzczegolowej; }
            set { listaKlasyfikacjiSzczegolowej = value; RaisePropertyChanged(); }
        }

        public tblKlasyfikacjaSzczegolowa WybranaKlasyfikacjaSzczegolowa
        {
            get { return wybranaKlasyfikacjaSzczegolowa; }
            set
            {
                wybranaKlasyfikacjaSzczegolowa = value;
                RaisePropertyChanged();
                PobierzListeUrzadzen();
            }
        }

        public IEnumerable<tblUrzadzenia> ListaUrzadzen
        {
            get { return listaUrzadzen; }
            set { listaUrzadzen = value; RaisePropertyChanged(); }
        }


        public tblUrzadzenia WybraneUrzadzenie
        {
            get { return wybraneUrzadzenie; }
            set { wybraneUrzadzenie = value; RaisePropertyChanged(); }
        }


        public tblZapotrzebowanie Zapotrzebowanie
        {
            get { return zapotrzebowanie; }
            set { zapotrzebowanie = value; RaisePropertyChanged(); }
        }

        public tblZapotrzebowanie ZapotrzebowanieOrg { get; set; }
        private bool isChanged;
        private vwZapotrzebowanieEwidencja zapotrzebowanieEwidencja;

        public tblKontrahent Kontrahent
        {
            get { return kontrahent; }
            set { kontrahent = value; RaisePropertyChanged(); }
        }


        public IEnumerable<tblPracownikGAT> ListaPracownikowGAT
        {
            get { return listaPracownikowGAT; }
            set { listaPracownikowGAT = value; RaisePropertyChanged(); }
        }


        public tblPracownikGAT WybranyPracownikGAT
        {
            get { return wybranyPracownikGAT; }
            set { wybranyPracownikGAT = value; RaisePropertyChanged(); }
        }

        public tblPracownikGAT WybranyPracownikOdpZaZakup { get; set; }

        public IEnumerable<tblZapotrzebowanieStatus> ListaStatusow
        {
            get { return listaStatusow; }
            set { listaStatusow = value; RaisePropertyChanged(); }
        }

        public tblZapotrzebowanieStatus WybranyStatus
        {
            get { return wybranyStatus; }
            set
            {
                wybranyStatus = value;
                Zapotrzebowanie.IDZapotrzebowanieStatus = wybranyStatus?.IDZapotrzebowanieStatus;
                RaisePropertyChanged();
                //Task.Run(() => WyslijMailaZOutlooka(Zapotrzebowanie));
            }
        }

        public IEnumerable<tblFirma> ListaFirm
        {
            get { return listaFirm; }
            set { listaFirm = value; RaisePropertyChanged(); }
        }

        public tblFirma WybranaFirma
        {
            get { return wybranaFirma; }
            set { wybranaFirma = value; RaisePropertyChanged(); }
        }


        public IEnumerable<tblJm> ListaJednostekMiar
        {
            get { return listaJendostekMiar; }
            set { listaJendostekMiar = value; RaisePropertyChanged(); }
        }

        public tblJm WybranaJednostkaMiary
        {
            get { return wybranaJednostkaMiary; }
            set { wybranaJednostkaMiary = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<tblPliki> ListaPlikow
        {
            get { return listaPlikow; }
            set { listaPlikow = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<tblPliki> ListaPlikowOrg { get; set; }

        public tblPliki WybranyPlik
        {
            get { return wybranyPlik; }
            set { wybranyPlik = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<tblZapotrzebowaniePozycje> ListaPozycjiZapotrzebowan
        {
            get { return listaPozycjiZapotrzebowan; }
            set { listaPozycjiZapotrzebowan = value; RaisePropertyChanged(); }
        }

        public tblZapotrzebowaniePozycje WybranaPozycjaZapotrzebowania
        {
            get { return wybranaPozycjaZapotrzebowania; }
            set { wybranaPozycjaZapotrzebowania = value; RaisePropertyChanged(); }
        }


        public string Tytul
        {
            get { return tytul; }
            set { tytul = value; RaisePropertyChanged(); }
        }


        public bool IsChanged
        {

            get
            {
                var test = !Zapotrzebowanie.Compare(ZapotrzebowanieOrg);
                return isChanged = !Zapotrzebowanie.Compare(ZapotrzebowanieOrg)
                                || pozycjeZapotrzebowania_IsChanged
                                || !listaPlikow.Compare(ListaPlikowOrg);
                //TODO IsChanged nie reaguje na zmiany w Plikach (dodawanie) - do zrobienia
            }
            set => isChanged = value;
        }


        public bool CzyZapotrzebowanieZweryfikowane
        {
            get
            {
                czyZapotrzebowanieZweryfikowane = Zapotrzebowanie.CzyZweryfikowano;
                return czyZapotrzebowanieZweryfikowane;
            }
            set
            {
                czyZapotrzebowanieZweryfikowane = value;
                Zapotrzebowanie.CzyZweryfikowano = value;
                WyslijMailaZGmailaPoZweryfikowaniu();
            }
        }
        #endregion

        #region CTOR
        public ZapotrzebowanieDodajViewModel(
                                            IUnitOfWork unitOfWork,
                                            IUnitOfWorkFactory unitOfWorkFactory,
                                            IViewService viewService,
                                            IDialogService dialogService,
                                            IFilesManipulation filesManipulation,
                                            IGmailSender gmailSender,
                                            IPlikiCRUD plikiCRUD,
                                            IOutlookMailSender outlookMailSender,
                                            IMessenger messenger
                                            )
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.viewService = viewService;
            this.dialogService = dialogService;
            this.filesManipulation = filesManipulation;
            this.gmailSender = gmailSender;
            this.plikiCRUD = plikiCRUD;
            this.messenger = messenger;
            this.outlookMailSender = outlookMailSender;

            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);
            UsunCommand = new RelayCommand(UsunCommandExecute, UsunCommandCanExecute);

            DodajPlikCommand = new RelayCommand(DodajPlikCommandExecute);
            UsunPlikCommand = new RelayCommand(UsunPlikCommandExecute);
            OtworzPlikCommand = new RelayCommand(OtworzPlikCommandExecute);

            PokazEwidencjeKontrahentowCommand = new RelayCommand(PokazEwidencjeKontrahentowCommandExecute);
            DodajKosztCommand = new RelayCommand(DodajKosztCommandExecute);
            EdytujKosztCommand = new RelayCommand(EdytujKosztCommandExecute);
            UsunPozycjeKosztowaCommand = new RelayCommand(UsunPozycjeKosztowaCommandExecute);
            ZamknijOknoCommand = new RelayCommand<CancelEventArgs>(ZamknijOknoCommandExecute);

            ZaladujWartosciPoczatkoweCommand = new RelayCommand(ZaladujWartosciPoczatkoweCommandExecute);

            ListaPlikow = new ObservableCollection<tblPliki>();
            ListaPozycjiZapotrzebowan = new ObservableCollection<tblZapotrzebowaniePozycje>();

            messenger.Register<vwZapotrzebowanieEwidencja>(this, GdyPrzeslanoZapotrzebowanie);
            messenger.Register<tblKontrahent>(this, GdyPrzeslanoKontrahenta);
            messenger.Register<tblZapotrzebowaniePozycje>(this, "Dodaj", GdyPrzeslanoPozycjeDoDodania);
            messenger.Register<tblZapotrzebowaniePozycje>(this, "Usun", GdyPrzeslanoPozycjeDoUsuniecia);
            messenger.Register<tblZapotrzebowaniePozycje>(this, "Edytuj", GdyPrzeslanoPozycjeDoEdycji);

            Zapotrzebowanie = new tblZapotrzebowanie();
            Zapotrzebowanie.DataZgloszenia = DateTime.Now;

            KlonujZapotrzebowanie();
        }


        private void KlonujZapotrzebowanie()
        {
            ZapotrzebowanieOrg = Zapotrzebowanie.DeepClone();
            ListaPlikowOrg = ListaPlikow.DeepClone();
        }

        #region WartosciPoczatkowe

        private async void ZaladujWartosciPoczatkoweCommandExecute()
        {
            await PobierzWartosciPoczatkowe();
            KlonujZapotrzebowanie();
        }

        private async Task PobierzWartosciPoczatkowe()
        {
            try
            {
                using (var unitOfWork = unitOfWorkFactory.Create())
                {
                    try
                    {
                        WybranyPracownikGAT = UzytkownikZalogowany.Uzytkownik;
                        WybranyPracownikOdpZaZakup = UzytkownikZalogowany.Uzytkownik;
                        ListaPracownikowGAT = await unitOfWork.tblPracownikGAT.PobierzPracownikowPracujacychAsync().ConfigureAwait(false);
                        ListaFirm = await unitOfWork.tblFirma.GetAllAsync().ConfigureAwait(false);
                        ListaJednostekMiar = await unitOfWork.tblJm.GetAllAsync().ConfigureAwait(false);
                        ListaStatusow = await unitOfWork.tblZapotrzebowanieStatus.GetAllAsync().ConfigureAwait(true);
                        ListaKlasyfikacjiOgolnej = await unitOfWork.tblKlasyfikacjaOgolna.GetAllAsync().ConfigureAwait(true);
                        WybranyStatus = ListaStatusow.SingleOrDefault(l => l.StatusZapotrzebowania == "Oczekuje");


                        Zapotrzebowanie.Nr = await unitOfWork.tblZapotrzebowanie.PobierzNowyNrZamowieniaAsync().ConfigureAwait(true);
                        Zapotrzebowanie.IDPracownikGAT = WybranyPracownikGAT.ID_PracownikGAT;
                        Zapotrzebowanie.IDPracownikOdpZaZakup = WybranyPracownikGAT.ID_PracownikGAT;
                        Zapotrzebowanie.DataZapotrzebowania = DateTime.Now.Date;
                        Zapotrzebowanie.IDZapotrzebowanieStatus = WybranyStatus.IDZapotrzebowanieStatus;
                        //RaisePropertyChanged(nameof(Zapotrzebowanie));

                        if (zapotrzebowanieEwidencja is null) return;

                        await PobierzDaneZEwidencjiZapotrzebowan();

                    }
                    catch (Exception ex)
                    {
                        dialogService.ShowError_BtnOK(ex.Message);

                    }
                }
                //RaisePropertyChanged(nameof(Zapotrzebowanie));
            }
            catch (Exception e)
            {
                dialogService.ShowInfo_BtnOK(e.Message);
            }
        }

        private async Task PobierzWartosciPoczatkoweRownolegle()
        {

            var pracownicy = unitOfWork.tblPracownikGAT.PobierzPracownikowPracujacychAsync();
            var firmy = unitOfWork.tblFirma.GetAllAsync();
            var jednostki = unitOfWork.tblJm.GetAllAsync();
            var statusy = unitOfWork.tblZapotrzebowanieStatus.GetAllAsync();
            var klasOgolne = unitOfWork.tblKlasyfikacjaOgolna.GetAllAsync();
            var nrZapotrzebowania = unitOfWork.tblZapotrzebowanie.PobierzNowyNrZamowieniaAsync();

            var tasks = new List<Task>();
            tasks.Add(pracownicy);
            tasks.Add(firmy);
            tasks.Add(jednostki);
            tasks.Add(statusy);
            tasks.Add(klasOgolne);
            tasks.Add(nrZapotrzebowania);

            foreach (var task in tasks)
            {
                await task.ConfigureAwait(false);
            }
            //await Task.WhenAll(tasks);

            ListaPracownikowGAT = await pracownicy;
            ListaFirm = await firmy;
            ListaJednostekMiar = await jednostki;
            ListaStatusow = await statusy;
            ListaKlasyfikacjiOgolnej = await klasOgolne;

            Zapotrzebowanie.Nr = await nrZapotrzebowania;
            //RaisePropertyChanged(nameof(Zapotrzebowanie));
            WybranyStatus = ListaStatusow.Where(l => l.StatusZapotrzebowania == "Oczekuje").SingleOrDefault();
            //RaisePropertyChanged(nameof(WybranyStatus));  

        }


        #endregion
        #endregion

        #region ZamknijOkno

        private void ZamknijOknoCommandExecute(CancelEventArgs args)
        {
            if (!IsChanged)
            {
                viewService.Close(this.GetType().Name);
                return;
            }

            if (dialogService.ShowQuestion_BoolResult("Wprowadzone zmiany nie będą zapisane. Czy kontynuować?"))
            {
                viewService.Close(this.GetType().Name);
            }
            else
            {
                if (args != null)
                    args.Cancel = true;
            }
        }
        #endregion


        #region Pliki
        private void DodajPlikCommandExecute()
        {
            var listaPlikowDoDodania = plikiCRUD.PobierzListePlikowDoDodania(Zapotrzebowanie);

            if (listaPlikowDoDodania == null)
                return;

            if (listaPlikowDoDodania.Count > 0)
            {
                foreach (var plik in listaPlikowDoDodania)
                {
                    ListaPlikow.Add(plik);
                }
            }
        }
        private async void UsunPlikCommandExecute()
        {
            if (WybranyPlik == null)
                return;

            if (WybranyPlik.IDPlik == 0)
            {
                ListaPlikow.Remove(WybranyPlik);
            }
            else
            {
                unitOfWork.tblPliki.Remove(WybranyPlik);
                await unitOfWork.SaveAsync();

                List<tblPliki> lista = new List<tblPliki>();
                lista.Add(WybranyPlik);
                plikiCRUD.UsunPlikZSerwera(lista);

                ListaPlikow.Remove(WybranyPlik);
            }
        }
        private void OtworzPlikCommandExecute()
        {
            if (WybranyPlik != null)
            {
                if (WybranyPlik.IDPlik == 0)
                {
                    System.Diagnostics.Process.Start(WybranyPlik.SciezkaLokalnaPliku);
                }
                else
                {
                    System.Diagnostics.Process.Start(WybranyPlik.SciezkaPliku);
                }
            }
        }
        #endregion

        #region PozycjeKosztowe
        private async void UsunPozycjeKosztowaCommandExecute()
        {
            if (WybranaPozycjaZapotrzebowania == null)
                return;

            if (WybranaPozycjaZapotrzebowania.IDZapotrzebowaniePozycja == 0)
            {
                ListaPozycjiZapotrzebowan.Remove(WybranaPozycjaZapotrzebowania);
            }
            else
            {
                try
                {
                    unitOfWork.tblZapotrzebowaniePozycje.Remove(WybranaPozycjaZapotrzebowania);
                    await unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {
                    dialogService.ShowInfo_BtnOK(ex.Message);
                }
            }
        }

        private void DodajKosztCommandExecute()
        {
            viewService.Show<DodajPozycjeZapotrzebowaniaViewModel>();
        }

        private async void GdyPrzeslanoPozycjeDoUsuniecia(tblZapotrzebowaniePozycje obj)
        {
            pozycjeZapotrzebowania_IsChanged = true;

            if (obj != null && obj.IDZapotrzebowaniePozycja != 0)
            {
                unitOfWork.tblZapotrzebowaniePozycje.Remove(obj);
                await unitOfWork.SaveAsync();

                ListaPozycjiZapotrzebowan.Remove(obj);
            }
            else if (obj != null && obj.IDZapotrzebowaniePozycja == 0)
            {
                try
                {
                    ListaPozycjiZapotrzebowan.Remove(obj);
                }
                catch (Exception ex)
                {
                    dialogService.ShowInfo_BtnOK(ex.Message);
                }
            }
        }

        private void GdyPrzeslanoPozycjeDoDodania(tblZapotrzebowaniePozycje obj)
        {
            pozycjeZapotrzebowania_IsChanged = true;

            if (obj != null && obj.IDZapotrzebowaniePozycja == 0)
            {
                ListaPozycjiZapotrzebowan.Add(obj);
                RaisePropertyChanged(nameof(ListaPozycjiZapotrzebowan));
            }
        }

        private void EdytujKosztCommandExecute()
        {
            DodajKosztCommandExecute();
            messenger.Send(WybranaPozycjaZapotrzebowania);
        }

        private void GdyPrzeslanoPozycjeDoEdycji(tblZapotrzebowaniePozycje obj)
        {
            pozycjeZapotrzebowania_IsChanged = true;
            var pozycjaDoEycji = ListaPozycjiZapotrzebowan.SingleOrDefault(p => p.IDZapotrzebowaniePozycja == obj.IDZapotrzebowaniePozycja);

            if (pozycjaDoEycji == null)
                return;

            pozycjaDoEycji.Nazwa = obj.Nazwa;
            pozycjaDoEycji.Ilosc = obj.Ilosc;
            pozycjaDoEycji.Cena = obj.Cena;
            pozycjaDoEycji.Koszt = obj.Koszt;
            pozycjaDoEycji.Uwagi = obj.Uwagi;

        }

        #endregion

        #region MessengerDelegaty

        private void GdyPrzeslanoKontrahenta(tblKontrahent obj)
        {
            if (obj != null)
            {
                Kontrahent = obj;
                Zapotrzebowanie.IDKontrahent = Kontrahent.ID_Kontrahent;
            }

            viewService.Close<DodajKontrahentaViewModel_old>();
            viewService.Close<EwidencjaKontrahentowViewModel_old>();
        }
        private void GdyPrzeslanoZapotrzebowanie(vwZapotrzebowanieEwidencja obj)
        {
            if (obj is null) return;

            zapotrzebowanieEwidencja = obj;

            messenger.Unregister<vwZapotrzebowanieEwidencja>(this, GdyPrzeslanoZapotrzebowanie);
        }

        private async Task PobierzDaneZEwidencjiZapotrzebowan()
        {
            await PobierzNiezbedneDane(zapotrzebowanieEwidencja);
            KlonujZapotrzebowanie();

        }
        #endregion

        private void PokazEwidencjeKontrahentowCommandExecute()
        {
            viewService.Show<EwidencjaKontrahentowViewModel>();
            messenger.Send<ListViewModelStatesEnum, EwidencjaKontrahentowViewModel>(ListViewModelStatesEnum.Select);
        }

        #region Usun
        private bool UsunCommandCanExecute()
        {
            if (Zapotrzebowanie.IDZapotrzebowanie == 0)
            {
                return false;
            }
            return true;
        }

        private async void UsunCommandExecute()
        {
            if (Zapotrzebowanie.IDZapotrzebowanie == 0)
            {
                return;
            }

            if (dialogService.ShowQuestion_BoolResult("Czy usunąć zapotrzebowanie?"))
            {
                try
                {
                    var listaZapotrzebowan = await unitOfWork.tblZapotrzebowaniePozycje.WhereAsync(p => p.IDZapotrzebowanie == Zapotrzebowanie.IDZapotrzebowanie);
                    var pliki = await unitOfWork.tblPliki.WhereAsync(p => p.IDZapotrzebowanie == Zapotrzebowanie.IDZapotrzebowanie);

                    unitOfWork.tblPliki.RemoveRange(pliki);
                    unitOfWork.tblZapotrzebowaniePozycje.RemoveRange(listaZapotrzebowan);
                    unitOfWork.tblZapotrzebowanie.Remove(Zapotrzebowanie);

                    await unitOfWork.SaveAsync();
                    plikiCRUD.UsunPlikZSerwera(pliki);
                }
                catch (Exception ex)
                {
                    dialogService.ShowInfo_BtnOK(ex.Message);
                }
            }

            messenger.Send(Zapotrzebowanie);
            KlonujZapotrzebowanie();
            viewService.Close<ZapotrzebowanieDodajViewModel>();
        }
        #endregion

        #region Zapisz
        private bool ZapiszCommandCanExecute()
        {
            bool odpowiedz = false;

            if (Zapotrzebowanie.IsValid)
            {
                odpowiedz = true;
                TooltipZapiszCommand = "";
            }

            if (Zapotrzebowanie.IDKontrahent == 0)
            {
                odpowiedz = false;
                TooltipZapiszCommand = "Brak wybranego kontrahenta";
                goto Koniec;
            }

            if (Zapotrzebowanie.DataZgloszenia.GetValueOrDefault().Date > Zapotrzebowanie.DataZapotrzebowania)
            {
                odpowiedz = false;
                TooltipZapiszCommand = "Data zapotrzebowania jest błędna.\r\nData zapotrzebowania powinna być późniejsza niż data zgłoszenia.";
                goto Koniec;
            }

            if (ListaPozycjiZapotrzebowan.Count() == 0)
            {
                odpowiedz = false;
                TooltipZapiszCommand = "Brak pozycji kosztowych";
                goto Koniec;

            }

            if (IsChanged == false)
            {
                odpowiedz = false;
                TooltipZapiszCommand = "Nie dokonano zmian";
                goto Koniec;
            }

        Koniec:
            return odpowiedz;
        }

        private async void ZapiszCommandExecute()
        {
            var czyZapotrzebowanieDodawane = Zapotrzebowanie.IDZapotrzebowanie == 0;

            await ZapiszZapotrzebowanie();
            await ZapiszPozycjeZapotrzebowan();
            ZapiszPliki();
            await WyslijMailaZGmailaPoZapisie(czyZapotrzebowanieDodawane);

            messenger.Send(Zapotrzebowanie);

            if (czyZapotrzebowanieDodawane)
                dialogService.ShowInfo_BtnOK("Zapotrzebowanie zostało dodane.");

            if (CzyZmienionoStatus())
                if (dialogService.ShowQuestion_BoolResult("Zmieniono status zapotrzebowania. Czy wysłać maila do osoby zgłaszającej zapotrzebowanie?"))
                {
                    var mailMessage = new ZapotrzebowanieMailItem(Zapotrzebowanie, new List<string>
                                                                            {
                                                                                Zapotrzebowanie.tblPracownikGAT.Email
                                                                                , "dyrektor.produkcji@gtex.pl"
                                                                            });
                    await outlookMailSender.SendAsync(mailMessage.Create());
                }



            KlonujZapotrzebowanie();
            viewService.Close<ZapotrzebowanieDodajViewModel>();
        }

        private bool CzyZmienionoStatus()
        {
            return Zapotrzebowanie.IDZapotrzebowanieStatus != ZapotrzebowanieOrg.IDZapotrzebowanieStatus;
        }

        private async Task ZapiszZapotrzebowanie()
        {
            if (Zapotrzebowanie.IDZapotrzebowanie == 0)
                unitOfWork.tblZapotrzebowanie.Add(Zapotrzebowanie);

            try
            {
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                dialogService.ShowInfo_BtnOK(ex.Message);
            }
        }

        private async Task ZapiszPozycjeZapotrzebowan()
        {
            if (ListaPozycjiZapotrzebowan.Count() == 0)
                return;

            foreach (var koszt in ListaPozycjiZapotrzebowan)
            {
                koszt.IDZapotrzebowanie = Zapotrzebowanie.IDZapotrzebowanie;

                if (koszt.IDZapotrzebowaniePozycja == 0)
                {
                    unitOfWork.tblZapotrzebowaniePozycje.Add(koszt);
                }
            }

            try
            {
                await unitOfWork.SaveAsync();
                pozycjeZapotrzebowania_IsChanged = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ZapiszPliki()
        {
            if (ListaPlikow.Count() == 0) return;

            foreach (var plik in ListaPlikow)
            {
                plik.IDZapotrzebowanie = Zapotrzebowanie.IDZapotrzebowanie;

                if (plik.IDPlik == 0)
                {
                    unitOfWork.tblPliki.Add(plik);
                    plikiCRUD.KopiujPlikNaSerwer(plik);
                }
            }
            await unitOfWork.SaveAsync();

        }

        #endregion

        #region WysylanieMaili

        private async void WyslijMailaZGmailaPoZweryfikowaniu()
        {
            if (CzyZapotrzebowanieZweryfikowane == false)
                return;

            if (dialogService.ShowQuestion_BoolResult("Zweryfikowano zapotrzebowanie. Czy wysłać maila do Szefa celem akceptacji?"))
            {
                //DodajZalezneTabeleDoZapotrzebowania();

                try
                {
                    var mailMessage = new ZapotrzebowanieMailMessage(zapotrzebowanie, new MailAddressesFromTo
                    {
                        gmailAddressFrom = "produkcja.gat@gmail.com",
                        mailAddressesTo = new List<string> { "ceo@gtex.pl" }
                    });
                    await gmailSender.SendMessageAsync(mailMessage.Create());
                    dialogService.ShowInfo_BtnOK("Wiadomość została wysłana");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        private async Task WyslijMailaZGmailaPoZapisie(bool czyDodajemyZapotrzebowanieDoBazy)
        {
            if (!czyDodajemyZapotrzebowanieDoBazy)
                return;

            DodajZalezneTabeleDoZapotrzebowania();

            var mailMessage = new ZapotrzebowanieMailMessage(Zapotrzebowanie,
                                                            new MailAddressesFromTo
                                                            {
                                                                gmailAddressFrom = "produkcja.gat@gmail.com",
                                                                mailAddressesTo = new List<string> { "dyrektor.produkcji@gtex.pl" }
                                                            });
            await gmailSender.SendMessageAsync(mailMessage.Create());
        }
        private void DodajZalezneTabeleDoZapotrzebowania()
        {
            Zapotrzebowanie.tblKontrahent = Kontrahent;
            Zapotrzebowanie.tblPracownikGAT = UzytkownikZalogowany.Uzytkownik;
            Zapotrzebowanie.tblKlasyfikacjaOgolna = WybranaKlasyfikacjaOgolna;
            Zapotrzebowanie.tblKlasyfikacjaSzczegolowa = WybranaKlasyfikacjaSzczegolowa;
            Zapotrzebowanie.tblUrzadzenia = WybraneUrzadzenie;
        }
        private async void WyslijMailaZOutlooka(tblZapotrzebowanie zapotrzebowanie)
        {
            if (!dialogService.ShowQuestion_BoolResult("Czy wysłać maila do osoby zgłaszającej zapotrzebowanie?"))
                return;

            var mailMessage = new ZapotrzebowanieMailItem(zapotrzebowanie, new List<string>
                                                                            {
                                                                                zapotrzebowanie.tblPracownikGAT.Email
                                                                                , "dyrektor.produkcji@gtex.pl"
                                                                            });
            try
            {
                await outlookMailSender.SendAsync(mailMessage.Create());
            }
            catch (Exception)
            {
                throw;
            }

            dialogService.ShowInfo_BtnOK("Wiadomość została wysłana");
        }



        #endregion

        private async void PobierzListeKlasyfikacjiSzczegolowejIPlatnika()
        {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
                if (WybranaKlasyfikacjaOgolna != null)
                {
                    ListaKlasyfikacjiSzczegolowej = await unitOfWork.tblKlasyfikacjaSzczegolowa
                        .PobierzKlasyfikacjeSzczegolowaDlaWybranejKlasyfikacjiOgolnejAsync(WybranaKlasyfikacjaOgolna.IDKlasyfikacjaOgolna).ConfigureAwait(false);
                    WybranaFirma = ListaFirm.Where(f => f.IDFirma == WybranaKlasyfikacjaOgolna.IDFirma).SingleOrDefault();
                }
            }
        }
        private async void PobierzListeUrzadzen()
        {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
                if (WybranaKlasyfikacjaSzczegolowa != null &&
                    WybranaKlasyfikacjaSzczegolowa.IDKlasyfikacjaSzczegolowa >= 7)
                {
                    ListaUrzadzen = await unitOfWork.tblUrzadzenia.
                        PobierzUrzadzeniaDlaWybranejKlasyfikacjiSzczegolowej(WybranaKlasyfikacjaSzczegolowa.IDKlasyfikacjaSzczegolowa).ConfigureAwait(false);
                }
            }
        }
        private async Task PobierzNiezbedneDane(vwZapotrzebowanieEwidencja obj)
        {
            if (obj == null)
                return;

            //await Task.Delay(100);

            try
            {
                Zapotrzebowanie = new tblZapotrzebowanie();
                Zapotrzebowanie = await unitOfWork.tblZapotrzebowanie.GetByIdAsync(obj.IDZapotrzebowanie);//.ConfigureAwait(false);

                Kontrahent = await unitOfWork.tblKontrahent.GetByIdAsync(Zapotrzebowanie.IDKontrahent).ConfigureAwait(false);
                ListaPlikow = new ObservableCollection<tblPliki>(await unitOfWork.tblPliki.WhereAsync(l => l.IDZapotrzebowanie == Zapotrzebowanie.IDZapotrzebowanie).ConfigureAwait(false));
                var listaJm = await unitOfWork.tblJm.GetAllAsync();
                var listaZapotrzebowan = await unitOfWork.tblZapotrzebowaniePozycje.WhereAsync(z => z.IDZapotrzebowanie == Zapotrzebowanie.IDZapotrzebowanie).ConfigureAwait(false);
                ListaPozycjiZapotrzebowan = new ObservableCollection<tblZapotrzebowaniePozycje>(listaZapotrzebowan);

                foreach (var pozycja in ListaPozycjiZapotrzebowan)
                {
                    if (pozycja.IDJm != null)
                        pozycja.Jm = listaJm.SingleOrDefault(j => j.IDJm == pozycja.IDJm).Jm;
                }

                WybranyPracownikGAT = ListaPracownikowGAT.Single(p => p.ID_PracownikGAT == Zapotrzebowanie.IDPracownikGAT);
                WybranaKlasyfikacjaOgolna = ListaKlasyfikacjiOgolnej.SingleOrDefault(k => k.IDKlasyfikacjaOgolna == Zapotrzebowanie.IDKlasyfikacjaOgolna);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
