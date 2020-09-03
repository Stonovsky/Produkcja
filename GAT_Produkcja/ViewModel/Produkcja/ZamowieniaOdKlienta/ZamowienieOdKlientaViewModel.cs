using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GAT_Produkcja.db;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Services;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.Messages;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.Pakowanie;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeokomorka;
using GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeowloknina;
using PropertyChanged;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta
{
    [AddINotifyPropertyChangedInterface]
    public class ZamowienieOdKlientaViewModel : ViewModelBase
    {
        #region Properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IDialogService dialogService;
        private readonly IViewService viewService;
        private readonly IMessenger messenger;
        private bool czyAdresDostawyJakOdbiorcy;

        public IEnumerable<tblPracownikGAT> ListaPracownikowGAT { get; set; }
        public tblPracownikGAT WybranyPracownikGAT { get; set; }
        public IEnumerable<tblKontrahent> ListaOdboiorcow { get; set; }
        public tblKontrahent WybranyOdbiorca { get; set; }
        public IEnumerable<tblKontrahent> ListaPrzewoznikow { get; set; }
        public tblKontrahent WybranyPrzewoznik { get; set; }
        public IEnumerable<tblZamowieniaPrzesylkaKoszt> ListaKosztowPrzesylek { get; set; }
        public tblZamowieniaPrzesylkaKoszt WybranyKosztPrzesylki { get; set; }
        public IEnumerable<tblZamowieniaWarunkiPlatnosci> ListaWarunkowPlatnosci { get; set; }
        public tblZamowieniaWarunkiPlatnosci WyrbanyWarunekPlatnosci { get; set; }
        public IEnumerable<tblZamowieniaTerminPlatnosci> ListaTerminowPlatnosci { get; set; }
        public tblZamowieniaTerminPlatnosci WybranyTerminPlatnosci { get; set; }
        public IEnumerable<tblZamowienieHandlowePakowanieRodzaj> ListaPakowanie { get; set; }
        public tblZamowienieHandlowePakowanieRodzaj WybranePakowanie { get; set; }
        public ObservableCollection<tblZamowienieHandloweTowarGeowloknina> ListaTowarowZamawianych { get; set; }
        public tblZamowienieHandloweTowarGeowloknina WybranyTowarZamowiony { get; set; }
        public bool CzyAdresDostawyJakOdbiorcy { get => czyAdresDostawyJakOdbiorcy; set { czyAdresDostawyJakOdbiorcy = value; RaisePropertyChanged(); GdyZmienionoToggleButtonDlaAdresu(); } }

        public int LiczbaPozycjiGeokomorek { get; set; }
        public int LiczbaPozycjiGeowloknin { get; set; }
        public int LiczbaPozycjiInne { get; set; }

        public bool CzyListaGeokomorekNieJestPusta { get; set; }
        public bool CzyListaGeowlokninNieJestPusta { get; set; }

        public string Tytul { get; set; }

        public tblZamowienieHandlowe Zamowienie { get; set; }


        public RelayCommand ZaladujPodczasUruchomieniaCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunCommand { get; set; }
        public IPakowanieViewModel PakowanieViewModel { get; }
        public ITowarGeowlokninaViewModel TowarGeowlokninaViewModel { get; }
        public ITowarGeokomorkaViewModel TowarGeokomorkaViewModel { get; }
        #endregion

        #region CTOR

        public ZamowienieOdKlientaViewModel(
                                            IUnitOfWork unitOfWork,
                                            IUnitOfWorkFactory unitOfWorkFactory,
                                            IDialogService dialogService,
                                            IViewService viewService,
                                            IPakowanieViewModel pakowanieViewModel,
                                            ITowarGeowlokninaViewModel towarGeowlokninaViewModel,
                                            ITowarGeokomorkaViewModel towarGeokomorkaViewModel,
                                            IMessenger messenger
                                            )
        {
            this.unitOfWork = unitOfWork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.dialogService = dialogService;
            this.viewService = viewService;
            this.messenger = messenger;

            PakowanieViewModel = pakowanieViewModel;
            TowarGeowlokninaViewModel = towarGeowlokninaViewModel;
            TowarGeokomorkaViewModel = towarGeokomorkaViewModel;
            ZaladujPodczasUruchomieniaCommand = new RelayCommand(ZaladujPodczasUruchomieniaCommandExecute);
            ZapiszCommand = new RelayCommand(ZapiszCommandExecute, ZapiszCommandCanExecute);

            messenger.Register<tblZamowienieHandlowe>(this, GdyPrzeslanoZamowienieHandlowe);
            messenger.Register<TabTablesMessage>(this, GdyPrzeslanoMsgZTabel);
            Zamowienie = new tblZamowienieHandlowe();
            ListaTowarowZamawianych = new ObservableCollection<tblZamowienieHandloweTowarGeowloknina>();
        }

        private void GdyPrzeslanoMsgZTabel(TabTablesMessage obj)
        {

            if (obj.KlasaPrzesylajaca.GetType() == typeof(TowarGeokomorkaViewModel))
            {
                LiczbaPozycjiGeokomorek = obj.IloscPozycjiWTabeli;
            }
        }

        private async void GdyPrzeslanoZamowienieHandlowe(tblZamowienieHandlowe obj)
        {
            Zamowienie = await unitOfWork.tblZamowienieHandlowe.GetByIdAsync(obj.IDZamowienieHandlowe).ConfigureAwait(false);
            messenger.Send(Zamowienie, "PrzeslaneZamowienie");
        }

        private async void ZaladujPodczasUruchomieniaCommandExecute()
        {
            Zamowienie.DataWysylki = DateTime.Now.Date;
            Zamowienie.DataZamowienia = DateTime.Now.Date;
            Tytul = "Nowe zamówienia od Klienta";

            using (var uow = unitOfWorkFactory.Create())
            {
                ListaPracownikowGAT = await uow.tblPracownikGAT.PobierzPracownikowPracujacychAsync();
                ListaOdboiorcow = await uow.tblKontrahent.GetAllAsync();
                ListaPrzewoznikow = await uow.tblKontrahent.GetAllAsync();
                ListaTerminowPlatnosci = await uow.tblZamowieniaTerminPlatnosci.GetAllAsync();
                ListaWarunkowPlatnosci = await uow.tblZamowieniaWarunkiPlatnosci.GetAllAsync();
                ListaKosztowPrzesylek = await uow.tblZamowieniaPrzesylkaKoszt.GetAllAsync();
                ListaPakowanie = await uow.tblZamowienieHandlowePakowanieRodzaj.GetAllAsync();
            }
        }

        #endregion

        #region Commands
        private bool ZapiszCommandCanExecute()
        {
            //TODO - ogarnac temat messengerami!!!
            CzyListaGeokomorekNieJestPusta = TowarGeokomorkaViewModel.ListaPozycjiGeokomorek.Count() > 0;
            CzyListaGeowlokninNieJestPusta = TowarGeowlokninaViewModel.ListaPozycjiGeowloknin.Count() > 0;

            if (!PakowanieViewModel.IsValid)
                return false;

            if (!TowarGeowlokninaViewModel.IsValid)
                return false;

            if (!Zamowienie.IsValid)
                return false;

            return true;

        }

        private async void ZapiszCommandExecute()
        {
            if (Zamowienie.IDZamowienieHandlowe == 0)
            {
                unitOfWork.tblZamowienieHandlowe.Add(Zamowienie);
            }

            await unitOfWork.SaveAsync();

            messenger.Send(Zamowienie, "ZapiszTowar");
        }

        #endregion

        private void GdyZmienionoToggleButtonDlaAdresu()
        {
            if (WybranyOdbiorca == null)
                return;

            if (CzyAdresDostawyJakOdbiorcy)
            {
                Zamowienie.UlicaDostawy = WybranyOdbiorca.Ulica;
                Zamowienie.KodPocztowyDostawy = WybranyOdbiorca.KodPocztowy;
                Zamowienie.MiastoDostawy = WybranyOdbiorca.Miasto;
            }
            else
            {
                Zamowienie.UlicaDostawy = String.Empty;
                Zamowienie.KodPocztowyDostawy = String.Empty;
                Zamowienie.MiastoDostawy = String.Empty;
            }
        }
    }
}
